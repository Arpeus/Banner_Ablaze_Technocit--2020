using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealer : CharacterManager
{
    public List<CharacterManager> _allyNeighbor;

    public override void Travel(List<HexCell> path)
    {
        base.Travel(path);
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
                //transform.localRotation = Quaternion.LookRotation(d);
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
            //transform.localRotation = Quaternion.LookRotation(d);
            yield return null;
        }

        transform.localPosition = location.Position;
        //orientation = transform.localRotation.eulerAngles.y;
        ListPool<HexCell>.Add(pathToTravel);
        bool isEnemyAround, isAllyAround;
        isEnemyAround = CheckEnemy();
        isAllyAround = CheckAllies();
        if (isEnemyAround && isAllyAround)
        {
            m_hudInGame.ShowActionHealAttackUI(this);
        }
        else if (isAllyAround)
        {
            m_hudInGame.ShowActionHealNoAttackUI(this);
        }
        else if (isEnemyAround)
        {
            m_hudInGame.ShowActionAttackUi(this);
        }
        else
        {
            m_hudInGame.ShowActionNoAttackUi(this);
        }
        
        pathToTravel = null;
        _animator.SetBool("_IsMoving", false);
    }

    public void Heal()
    {
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            HexCell neighbor = null;
            if (location.GetNeighbor(d) != null)
            {
                neighbor = location.GetNeighbor(d);
                if (neighbor.CharacterManager != null && neighbor.CharacterManager._playerNumberType == this._playerNumberType)
                {
                    _allyNeighbor.Add(neighbor.CharacterManager);
                }
                if (_character._range > 1)
                {
                    for (HexDirection e = HexDirection.NE; e <= HexDirection.NW; e++)
                    {
                        HexCell neighborTest = null;
                        if (neighbor.GetNeighbor(e) != null)
                        {
                            neighborTest = neighbor.GetNeighbor(e);
                            if (
                                    neighborTest.CharacterManager != null 
                                    && neighborTest.CharacterManager._playerNumberType == this._playerNumberType 
                                    && neighborTest.CharacterManager != this
                                )
                            {
                                _allyNeighbor.Add(neighborTest.CharacterManager);
                            }
                        }
                    }
                }
            }
        }
    }

    public bool CheckAllies()
    {
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            HexCell neighbor = null;
            if (location.GetNeighbor(d) != null)
            {
                neighbor = location.GetNeighbor(d);
                if (neighbor.CharacterManager != null && neighbor.CharacterManager._playerNumberType == this._playerNumberType)
                    return true;
                if (_character._range > 1)
                {
                    for (HexDirection e = HexDirection.NE; e <= HexDirection.NW; e++)
                    {
                        HexCell neighborTest = null;
                        if (neighbor.GetNeighbor(e) != null)
                        {
                            neighborTest = neighbor.GetNeighbor(e);
                            if (
                                neighborTest.CharacterManager != null
                                && neighborTest.CharacterManager._playerNumberType == this._playerNumberType
                                && neighborTest.CharacterManager != this
                                )
                                return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public void ClearAlly()
    {
        _allyNeighbor.Clear();
    }

}
