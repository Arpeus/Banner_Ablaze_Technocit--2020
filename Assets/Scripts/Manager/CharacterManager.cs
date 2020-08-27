using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterData _character;
    public PlayerNumber _playerNumberType;

    public bool hasAlreadyPlayed = false;
    public bool hasMoved = false;
    public bool hasAttacked = false;

    public List<CharacterManager> m_enemyNeighbor;
    public List<CharacterManager> m_enemyNeighborRange;

    public Animator _animator;

    public ParticleSystem _particuleLauncher;

    protected const float rotationSpeed = 0f;
    protected const float travelSpeed = 4f;

    public static CharacterManager unitPrefab;

    public HexGrid Grid { get; set; }

    protected HUDInGame m_hudInGame;
    [HideInInspector]public LifeManager m_lifeManager;
    

    void OnMouseOver()
    {
        Debug.Log("test");
    }

    private void Awake()
    {
        Grid = FindObjectOfType<HexGrid>();
        m_hudInGame = FindObjectOfType<HUDInGame>();
        m_lifeManager = GetComponent<LifeManager>();
        m_lifeManager.SetHealth(_character._health);
        m_lifeManager.SetArmor(_character._armor);
        m_lifeManager.SetArmorMargic(_character._resistanceMagic);
        m_lifeManager.SetDodge(_character._dodge);
    }

    public void Start()
    {
        _animator = GetComponent<Animator>();
    }

    /*private void Update()
    {
        
        if (hasAlreadyPlayed)
        {
            _particuleLauncher.Emit(1);
            Debug.Log("unit Ready");
        }
        if (!hasAlreadyPlayed)
        {
            _particuleLauncher.Emit(0);
            Debug.Log("unit alReady plays");
        }
        
    }*/

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
            
            Grid.IncreaseVisibility(value, VisionRange);
            transform.localPosition = value.Position;
            
        }
    }

    protected HexCell location, currentTravelLocation;

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
            return 1;
        }
    }

    protected float orientation;

    protected List<HexCell> pathToTravel;

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

    public virtual void Travel(List<HexCell> path)
    {
        _animator.SetBool("_IsMoving", true);
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
                        if (neighbor.GetNeighbor(e) != null)
                        {
                            neighborTest = neighbor.GetNeighbor(e);
                            if (neighborTest.CharacterManager != null && neighborTest.CharacterManager._playerNumberType != this._playerNumberType)
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
                if (_character._range > 1)
                {
                    for (HexDirection e = HexDirection.NE; e <= HexDirection.NW; e++)
                    {
                        HexCell neighborTest = null;
                        if (neighbor.GetNeighbor(e) != null)
                        {
                            neighborTest = neighbor.GetNeighbor(e);
                            if (neighborTest.CharacterManager != null && neighborTest.CharacterManager._playerNumberType != this._playerNumberType)
                            {
                                if (!m_enemyNeighbor.Contains(neighborTest.CharacterManager) && !m_enemyNeighborRange.Contains(neighborTest.CharacterManager))
                                {
                                    m_enemyNeighborRange.Add(neighborTest.CharacterManager);
                                }
                            }   
                        }
                    }
                }
            }
        }
    }

    public void SetVisibleAround()
    {
        /*
        location.Visible = true;
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            HexCell neighbor = null;
            if (location.GetNeighbor(d) != null)
            {
                neighbor = location.GetNeighbor(d);
                neighbor.Visible = true;
            }
        }
        */
    }


    public void ReceiveHeal(CharacterManager characterHealer)
    {
        m_lifeManager.ReceiveHeal(characterHealer);
        if (characterHealer._playerNumberType == PlayerNumber.EType_PlayerOne)
            GameManager.Instance.EType_Phase = PhaseType.EType_TurnPhasePlayerOne;
        else
            GameManager.Instance.EType_Phase = PhaseType.EType_TurnPhasePlayerTwo;
    }

    public void TakeDamage(CharacterManager character, bool counterAttack = false)
    {
       
        m_lifeManager.TakeDamage(this, character, BonusDamage(character), counterAttack);
    }

    public void TakeDamageRange(CharacterManager character,  bool counterAttack = false)
    {
        
        bool canCounter = true;
        if (this._character._range < character._character._range)
            canCounter = false;

        m_lifeManager.TakeDamage(this, character, BonusDamage(character), counterAttack, canCounter);
    }

    public int BonusDamage(CharacterManager character)
    {
        int bonusDamage = 0;
        if (character._character.typeBonusDamage == this._character.type)
            bonusDamage = character._character._damageTriangle;
        return bonusDamage;
    }

    public void SetStateTurn()
    {
        if (_playerNumberType == PlayerNumber.EType_PlayerOne)
            GameManager.Instance.EType_Phase = PhaseType.EType_TurnPhasePlayerOne;
        else
            GameManager.Instance.EType_Phase = PhaseType.EType_TurnPhasePlayerTwo;
    }

    public void ReloadUI()
    {

    }

    public void ClearEnemy()
    {
        m_enemyNeighbor.Clear();
        m_enemyNeighborRange.Clear();
    }

    public void SetHasAlreadyPlayed(bool hasAlreadyPlayed)
    {
        this.hasAlreadyPlayed = hasAlreadyPlayed;
    }

    public void SetHasMoved(bool hasMoved)
    {
        this.hasMoved = hasMoved;
    }

    public void SetHasAttacked(bool hasAttacked)
    {
        this.hasAttacked = hasAttacked;
    }
}
