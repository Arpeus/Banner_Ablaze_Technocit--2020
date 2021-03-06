﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterData _character;
    public PlayerNumber _playerNumberType;

    public Sprite _spritePreview;
    public SpriteRenderer _spriteCounterAttack;
    public SpriteRenderer m_spriteOnMap;
    public Sprite spriteMap;

    public bool hasAlreadyPlayed = false;
    public bool hasMoved = false;
    public bool hasAttacked = false;
    
    public List<CharacterManager> m_enemyNeighbor;
    public List<CharacterManager> m_enemyNeighborRange;

    public Animator _animator;   

    public Sprite spriteTerrain;

    public ParticleSystem _particuleLauncher;
    public GameObject _particuleHeal;

    protected const float rotationSpeed = 0f;
    protected const float travelSpeed = 4f;

    private string albedoColorTeam;
    private string albedoColorHasPlayed;

    public static CharacterManager unitPrefab;

    public HexGrid Grid { get; set; }

    protected HUDInGame m_hudInGame;
    [HideInInspector] public LifeManager m_lifeManager;
    public AnimationManager animattack;
    public AnimationManager animDefense;

    public HexGameUI hexGameUI;
    
    private void Awake()
    {
        Grid = FindObjectOfType<HexGrid>();
        m_hudInGame = FindObjectOfType<HUDInGame>();
        m_lifeManager = GetComponent<LifeManager>();
        m_lifeManager.SetHealth(_character._health);
        m_lifeManager.SetArmor(_character._armor);
        m_lifeManager.SetArmorMargic(_character._resistanceMagic);
        m_lifeManager.SetDodge(_character._dodge);
        m_spriteOnMap = GetComponent<SpriteRenderer>();
        spriteMap = m_spriteOnMap.sprite;
    }

    internal void SetNight()
    {
        if (_playerNumberType == PlayerNumber.EType_PlayerOne)
        {        
            animattack.spriteTeam = _character.spriteAnimNightTeamOne;
            animattack.animatorTeam = _character.animatorNightTeamOne;
            animDefense.spriteTeam = _character.spriteAnimNightTeamOne;
            animDefense.animatorTeam = _character.animatorNightTeamOne;
            
        }
        else
        {
            animattack.spriteTeam = _character.spriteAnimNightTeamTwo;
            animattack.animatorTeam = _character.animatorNightTeamTwo;
            animDefense.spriteTeam = _character.spriteAnimNightTeamTwo;
            animDefense.animatorTeam = _character.animatorNightTeamTwo;
        }
        _animator.runtimeAnimatorController = _character.animatorMapNight;
        spriteMap = _character.spriteMapNight;
        m_spriteOnMap.sprite = spriteMap;
    }

    internal void SetDay()
    {
        if (_playerNumberType == PlayerNumber.EType_PlayerOne)
        {
            animattack.spriteTeam = _character.spriteAnimDayTeamOne;
            animattack.animatorTeam = _character.animatorDayTeamOne;
            animDefense.animatorTeam = _character.animatorDayTeamOne;
            animDefense.spriteTeam = _character.spriteAnimDayTeamOne;
        }
        else
        {
            animattack.spriteTeam = _character.spriteAnimDayTeamTwo;
            animattack.animatorTeam = _character.animatorDayTeamTwo;
            animDefense.animatorTeam = _character.animatorDayTeamTwo;
            animDefense.spriteTeam = _character.spriteAnimDayTeamTwo;
        }
        _animator.runtimeAnimatorController = _character.animatorMapDay;
        spriteMap = _character.spriteMapDay;
        m_spriteOnMap.sprite = spriteMap;
    }

    public void Start()
    {
        if(_playerNumberType == PlayerNumber.EType_PlayerOne )
        {
            _spritePreview = _character.spritePreviewTeamOne;
            animattack.spriteTeam = _character.spriteAnimDayTeamOne;
            animattack.animatorTeam = _character.animatorDayTeamOne;
            animDefense.animatorTeam = _character.animatorDayTeamOne;
            AlbedoColorTeam = _character.albedoColorGreen;
            animDefense.spriteHealthBarUI = _character.healthBarCombatTeamOne;
            animattack.spriteHealthBarUI = _character.healthBarCombatTeamOne;
        }
        else
        {
            _spritePreview = _character.spritePreviewTeamTwo;
            animattack.spriteTeam = _character.spriteAnimDayTeamTwo;
            animattack.animatorTeam = _character.animatorDayTeamTwo;
            animDefense.animatorTeam = _character.animatorDayTeamTwo;
            AlbedoColorTeam = _character.albedoColorRed;
            animDefense.spriteHealthBarUI = _character.healthBarCombatTeamTwo;
            animattack.spriteHealthBarUI = _character.healthBarCombatTeamTwo;
        }
        albedoColorHasPlayed = _character.albedoColorGray;
        m_spriteOnMap.sprite = _character.spriteMapDay;
        SetColorAlbedo(AlbedoColorTeam);
        _animator = GetComponent<Animator>();       
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

    public string AlbedoColorTeam { get => albedoColorTeam; set => albedoColorTeam = value; }

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
            moveCost = edgeType == HexEdgeType.Flat ? 1 : 1;
            if (toCell.IsPlantLevel)
            {
                moveCost -= GameManager.Instance.malusForestMove;
            }
            if (toCell.HasRiver)
            {
                moveCost -= GameManager.Instance.malusRiverMove;
            }
            
            //moveCost +=
              //  toCell.UrbanLevel + toCell.FarmLevel + toCell.PlantLevel;
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

    public void HandleHighlight(Color color)
    {
        Location.EnableHighlight(color);
    }

    public void DisableHighlight()
    {
        Location.DisableHighlight();
    }

    public void SetColorAlbedo(string albedoColor)
    {
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString(albedoColor, out myColor);
        m_spriteOnMap.color = myColor;
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
        foreach(CharacterManager character in m_enemyNeighbor)
        {
            character.HandleHighlight(Color.red);
        }
        foreach (CharacterManager character in m_enemyNeighborRange)
        {
           character.HandleHighlight(Color.red);
        }
    }

    private void OnMouseOver()
    {
        if(!hasAlreadyPlayed)
            _animator.SetBool("_isIdle", true);
    }

    private void OnMouseExit()
    {
        _animator.SetBool("_isIdle", false);
    }

    public void ReceiveHeal(CharacterManager characterHealer)
    {
        m_lifeManager.ReceiveHeal(characterHealer);
        Instantiate(_particuleHeal, transform.position, Quaternion.Euler(0,0,0));
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

    public void ClearEnemy()
    {
        foreach (CharacterManager character in m_enemyNeighbor)
        {
            character.DisableHighlight();
        }
        foreach (CharacterManager character in m_enemyNeighborRange)
        {
            character.DisableHighlight();
        }
        m_enemyNeighbor.Clear();
        m_enemyNeighborRange.Clear();
    }

    public void SetHasAlreadyPlayed(bool hasAlreadyPlayed)
    {
        this.hasAlreadyPlayed = hasAlreadyPlayed;
        if(hasAlreadyPlayed) SetColorAlbedo(albedoColorHasPlayed);
        else SetColorAlbedo(AlbedoColorTeam);
    }

    public void SetHasMoved(bool hasMoved)
    {
        this.hasMoved = hasMoved;
    }

    public void SetHasAttacked(bool hasAttacked)
    {
        this.hasAttacked = hasAttacked;
    }

    public void SetSpriteCounterAttack(bool active)
    {
        _spriteCounterAttack.enabled = active;
    }

    public int GetDodge(CharacterManager characterAttack)
    {
        int tmpDodge = 0;

        if (Location.IsPlantLevel)
        {
            tmpDodge += GameManager.Instance.bonusForestDodge;
        }
        if (Location.HasRiver)
        {
            tmpDodge += GameManager.Instance.malusRiverDodge;
        }
        if (Location.IsSpecial)
        {
            tmpDodge += GameManager.Instance.bonusCastleDodge;
        }

        if (_character.type == TypeCharacter.SwordMan && characterAttack._character.type == TypeCharacter.Lancer)
            tmpDodge = tmpDodge * 2;     

        return tmpDodge;
    }

    public int GetArmor()
    {
        int tmpArmor = 0;

        if (Location.IsPlantLevel)
        {
            tmpArmor += GameManager.Instance.bonusForestDefense;
        }
        if (Location.HasRiver)
        {
        }
        if (Location.IsSpecial)
        {
            tmpArmor += GameManager.Instance.bonusCastleDefense;
        }

        return tmpArmor;
    }

    public Sprite GetSpriteTerrain()
    {
        if (Location.IsPlantLevel && GameManager.Instance.cycle == CyclePhase.PhaseDay)
        {
            return HUDInGame.Instance._terrains[2];
        }
        if (Location.IsPlantLevel && GameManager.Instance.cycle == CyclePhase.PhaseNight)
        {
            return HUDInGame.Instance._terrains[3];
        }
        if (Location.HasRiver && GameManager.Instance.cycle == CyclePhase.PhaseDay)
        {
            return HUDInGame.Instance._terrains[4];
        }
        if (Location.HasRiver && GameManager.Instance.cycle == CyclePhase.PhaseNight)
        {
            return HUDInGame.Instance._terrains[5];
        }
        if(GameManager.Instance.cycle == CyclePhase.PhaseNight)
        {
            return HUDInGame.Instance._terrains[1];
        }
        return HUDInGame.Instance._terrains[0];
    }

}
