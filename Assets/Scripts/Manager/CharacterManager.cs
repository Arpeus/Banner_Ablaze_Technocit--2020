﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterData _character;
    public PlayerNumber _playerNumberType;

    public List<CharacterManager> m_enemyNeighbor;

    const float rotationSpeed = 0f;
    const float travelSpeed = 4f;

    public static CharacterManager unitPrefab;

    public HexGrid Grid { get; set; }

    private HUDInGame m_hudInGame;
    [HideInInspector]public LifeManager m_lifeManager;
    

    private void Awake()
    {
        m_hudInGame = FindObjectOfType<HUDInGame>();
        m_lifeManager = GetComponent<LifeManager>();
        m_lifeManager.SetHealth(_character._health);
        m_lifeManager.SetArmor(_character._armor);
        m_lifeManager.SetArmorMargic(_character._resistanceMagic);
        m_lifeManager.SetDodge(_character._dodge);
    }



    public HexCell Location
    {
        get
        {
            return location;
        }
        set
        {
            if (location)
            {
                Grid.DecreaseVisibility(location, VisionRange);
                location.CharacterManager = null;
            }
            location = value;
            value.CharacterManager = this;
            //Grid.IncreaseVisibility(value, VisionRange);
            transform.localPosition = value.Position;
            //grid.MakeChildOfColumn(transform, value.ColumnIndex);
        }
    }

    HexCell location, currentTravelLocation;

    public float Orientation
    {
        get
        {
            return orientation;
        }
        set
        {
            orientation = value;
            transform.localRotation = Quaternion.Euler(0f, value, 0f);
        }
    }

    public int Speed
    {
        get
        {
            return 1;
        }
    }

    public int VisionRange
    {
        get
        {
            return 3;
        }
    }

    float orientation;

    List<HexCell> pathToTravel;

    public void ValidateLocation()
    {
        transform.localPosition = location.Position;
    }

    public bool IsValidDestination(HexCell cell)
    {
        //return cell.IsExplored && !cell.IsUnderwater && !cell.CharacterManager && !cell.IsMountainLevel;
        return !cell.IsUnderwater && !cell.CharacterManager && !cell.IsMountainLevel;

    }

    public bool IsValidaSpawn(HexCell cell)
    {
        return cell.IsSpanwerP1 && cell.IsSpanwerP2;
    }

    public void Travel(List<HexCell> path)
    {
        location.CharacterManager = null;
        location = path[path.Count - 1];
        location.CharacterManager = this;
        pathToTravel = path;
        StopAllCoroutines();
        StartCoroutine(TravelPath());
    }

    IEnumerator TravelPath()
    {
        
        Vector3 a, b, c = pathToTravel[0].Position;
        //yield return LookAt(pathToTravel[1].Position);

        if (!currentTravelLocation)
        {
            currentTravelLocation = pathToTravel[0];
        }
        //Grid.DecreaseVisibility(currentTravelLocation, VisionRange);
        int currentColumn = currentTravelLocation.ColumnIndex;

        float t = Time.deltaTime * travelSpeed;
        for (int i = 1; i < pathToTravel.Count; i++)
        {
            currentTravelLocation = pathToTravel[i];
            a = c;
            b = pathToTravel[i - 1].Position;

            int nextColumn = currentTravelLocation.ColumnIndex;
            if (currentColumn != nextColumn)
            {
                if (nextColumn < currentColumn - 1)
                {
                    a.x -= HexMetrics.innerDiameter * HexMetrics.wrapSize;
                    b.x -= HexMetrics.innerDiameter * HexMetrics.wrapSize;
                }
                else if (nextColumn > currentColumn + 1)
                {
                    a.x += HexMetrics.innerDiameter * HexMetrics.wrapSize;
                    b.x += HexMetrics.innerDiameter * HexMetrics.wrapSize;
                }
                //Grid.MakeChildOfColumn(transform, nextColumn);
                currentColumn = nextColumn;
            }

            c = (b + currentTravelLocation.Position) * 0.5f;
            //Grid.IncreaseVisibility(pathToTravel[i], VisionRange);

            for (; t < 1f; t += Time.deltaTime * travelSpeed)
            {
                transform.localPosition = Bezier.GetPoint(a, b, c, t);
                Vector3 d = Bezier.GetDerivative(a, b, c, t);
                d.y = 0f;
                transform.localRotation = Quaternion.LookRotation(d);
                yield return null;
            }
            //Grid.DecreaseVisibility(pathToTravel[i], VisionRange);
            t -= 1f;
        }
        currentTravelLocation = null;

        a = c;
        b = location.Position;
        c = b;
        //Grid.IncreaseVisibility(location, VisionRange);
        for (; t < 1f; t += Time.deltaTime * travelSpeed)
        {
            transform.localPosition = Bezier.GetPoint(a, b, c, t);
            Vector3 d = Bezier.GetDerivative(a, b, c, t);
            d.y = 0f;
            transform.localRotation = Quaternion.LookRotation(d);
            yield return null;
        }

        transform.localPosition = location.Position;
        orientation = transform.localRotation.eulerAngles.y;
        ListPool<HexCell>.Add(pathToTravel);
        if(CheckEnemy())
        {
            m_hudInGame.ShowActionUi(this);
            GameManager.Instance.EType_Phase = PhaseType.EType_AttackPhase;
        }
        pathToTravel = null;
    }

    IEnumerator LookAt(Vector3 point)
    {
        if (HexMetrics.Wrapping)
        {
            float xDistance = point.x - transform.localPosition.x;
            if (xDistance < -HexMetrics.innerRadius * HexMetrics.wrapSize)
            {
                point.x += HexMetrics.innerDiameter * HexMetrics.wrapSize;
            }
            else if (xDistance > HexMetrics.innerRadius * HexMetrics.wrapSize)
            {
                point.x -= HexMetrics.innerDiameter * HexMetrics.wrapSize;
            }
        }

        point.y = transform.localPosition.y;
        Quaternion fromRotation = transform.localRotation;
        Quaternion toRotation =
            Quaternion.LookRotation(point - transform.localPosition);
        float angle = Quaternion.Angle(fromRotation, toRotation);

        if (angle > 0f)
        {
            float speed = rotationSpeed / angle;
            for (
                float t = Time.deltaTime * speed;
                t < 1f;
                t += Time.deltaTime * speed
            )
            {
                transform.localRotation =
                    Quaternion.Slerp(fromRotation, toRotation, t);
                yield return null;
            }
        }

        transform.LookAt(point);
        orientation = transform.localRotation.eulerAngles.y;
    }

    public int GetMoveCost(HexCell fromCell, HexCell toCell, HexDirection direction)
    {
        if (!IsValidDestination(toCell))
        {
            return -1;

        }
        HexEdgeType edgeType = fromCell.GetEdgeType(toCell);
        if (edgeType == HexEdgeType.Cliff)
        {
            return -1;
        }
        int moveCost;
        if (fromCell.HasRoadThroughEdge(direction))
        {
            moveCost = 1;
        }
        else if (fromCell.Walled != toCell.Walled)
        {
            return -1;
        }
        else
        {
            moveCost = edgeType == HexEdgeType.Flat ? 0 : 0;
            moveCost +=
                toCell.UrbanLevel + toCell.FarmLevel + toCell.PlantLevel;
        }
        return moveCost;
    }

    public void Die()
    {
        if (location)
        {
            Grid.DecreaseVisibility(location, VisionRange);
        }
        location.CharacterManager = null;
        Destroy(gameObject);
    }

    public void Save(BinaryWriter writer)
    {
        location.coordinates.Save(writer);
        writer.Write(orientation);
    }

    public static void Load(BinaryReader reader, HexGrid grid)
    {
        HexCoordinates coordinates = HexCoordinates.Load(reader);
        float orientation = reader.ReadSingle();
        grid.AddUnit(
            Instantiate(unitPrefab), grid.GetCell(coordinates), orientation
        );
    }

    void OnEnable()
    {
        if (location)
        {
            transform.localPosition = location.Position;
            if (currentTravelLocation)
            {
                Grid.IncreaseVisibility(location, VisionRange);
                Grid.DecreaseVisibility(currentTravelLocation, VisionRange);
                currentTravelLocation = null;
            }
        }
    }

    public bool CheckEnemy()
    {
       for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
       {
            HexCell neighbor = null;
            if (location.GetNeighbor(d) != null)
            {
                neighbor = location.GetNeighbor(d);
                if (neighbor.CharacterManager != null && neighbor.CharacterManager._playerNumberType != this._playerNumberType)
                    return true;
                if (_character._range > 1)
                {
                    for (HexDirection e = HexDirection.NE; e <= HexDirection.NW; e++)
                    {
                        HexCell neighborTest = null;
                        if (location.GetNeighbor(e) != null)
                        {
                            neighborTest = neighbor.GetNeighbor(e);
                            if (neighborTest.CharacterManager != null && neighbor.CharacterManager._playerNumberType != this._playerNumberType)
                                return true;
                        }
                    }
                }            
            }
        }

        return false;
    }

    public void Attack()
    {
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            HexCell neighbor = null;
            if (location.GetNeighbor(d) != null)
            {
                neighbor = location.GetNeighbor(d);
                if (neighbor.CharacterManager != null && neighbor.CharacterManager._playerNumberType != this._playerNumberType)
                {
                    m_enemyNeighbor.Add(neighbor.CharacterManager);
                }
            }
        }
    }

    public void TakeDamage(CharacterManager character)
    {
        Debug.Log(this.m_lifeManager.Health);
        int bonusDamage = 0;
        if (character._character.typeBonusDamage == this._character.type)
            bonusDamage = character._character._damageTriangle;
        m_lifeManager.TakeDamage(this, character, bonusDamage);
        Debug.Log(this.m_lifeManager.Health);
        GameManager.Instance.EType_Phase = PhaseType.EType_TurnPhasePlayerOne;
    }

    public void ClearEnemy()
    {
        m_enemyNeighbor.Clear();
    }
}
