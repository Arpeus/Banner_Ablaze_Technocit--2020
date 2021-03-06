﻿using UnityEngine;
using UnityEngine.UI;

public class HUDInGame : MonoBehaviour
{

    public static HUDInGame Instance;
    [Header("UI Turn Player")]
    public GameObject _turnPlayerOne;
    public GameObject _turnPlayerTwo;

    [Header("UI GameObject")]
    [SerializeField] private GameObject m_PanelSpawnUI;
    [SerializeField] private GameObject m_panelActionAttackUI;
    [SerializeField] private GameObject m_panelButtonActionAttackUI;
    [SerializeField] private GameObject m_panelButtonActionWaitUI;
    [SerializeField] private GameObject m_panelButtonActionHealUI;
    [SerializeField] private GameObject m_uniteMenu;
    [SerializeField] private GameObject m_panelVictory;

    [Header("Preview Combat")]
    public PreviewHUD _panelPreview;


    [Header("Animation Scene")]
    public GameObject[] _placeAttackUnits;
    public GameObject _placeAttackRangeUnits;
    public GameObject[] _placeDefenseUnits;
    public GameObject _placeDefenseRangeUnits;
    public GameObject _missFXAttack;
    public GameObject _hitFXAttack;
    public GameObject _missFXDefense;
    public GameObject _hitFXDefense;
    public GameObject _terrainAttack;
    public GameObject _terrainDefense;
    public Sprite[] _terrains;
    public GameObject healthBarUIAttack;
    public GameObject healthBarUIDefense;
    public GameObject _backgroundAnim;


    [Header("UI Spawn")]
    [SerializeField] private GameObject[] m_placeBtnSpawn;
    [SerializeField] private GameObject[] m_btnSpawn;
    [SerializeField] private Button m_btnSpawnPlayerTwo;
    [SerializeField] private Button m_btnGoToTurnPhase;

    [SerializeField]private CharacterManager m_currentCharacterManager;

    private HexGameUI hexgrid;

    public GameObject UniteMenu { get => m_uniteMenu; set => m_uniteMenu = value; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        hexgrid = FindObjectOfType<HexGameUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        AddButton(0);
        //m_btnSpawnPlayerTwo.interactable = false;
    }

    void AddButtonSpawn(int index, int indexPlayer)
    {
        int i = 0;
        foreach (GameObject go in m_placeBtnSpawn)
        {
            if (go.GetComponentInChildren<ButtonSpawn>() == null)
            {
                GameObject btnSpawn = Instantiate(m_btnSpawn[index], m_placeBtnSpawn[i].transform);
                btnSpawn.GetComponent<ButtonSpawn>().Player = GameManager.Instance._players[indexPlayer];
                break;
            }
            i++;
        }
    }

    public void ChangeStateSpawn(int indexPlayer)
    {
        RemoveSpawnButton();
        GameManager.Instance.EType_Phase = PhaseType.EType_SpawnPhasePlayerTwo;
        m_btnSpawnPlayerTwo.gameObject.SetActive(false);
        m_btnGoToTurnPhase.gameObject.SetActive(true);
        AddButton(indexPlayer);
    }

    public void ChangeStateTurnPhase()
    {
        m_PanelSpawnUI.SetActive(false);
        SetActiveTurnPlayer(true);
        GameManager.Instance._players[1].SetCounterAttack();
        GameManager.Instance.EType_Phase = PhaseType.EType_TurnPhasePlayerOne;
    }

    public void RemoveSpawnButton()
    {
        foreach (ButtonSpawn btnSpawn in FindObjectsOfType<ButtonSpawn>())
        {
            btnSpawn.Destroy();
        }
    }

    public void AddButton(int indexPlayer)
    {
        for(int i = 0; i < GameManager.Instance._players[indexPlayer].Nbtroops.Length; i++)
        {
            if (GameManager.Instance._players[indexPlayer].Nbtroops[i] != 0)
            {
                AddButtonSpawn(i, indexPlayer);
            }
        }
    }

    public void ShowActionAttackUi(CharacterManager character, bool attack, bool wait, bool heal)
    {
        m_panelButtonActionAttackUI.SetActive(attack);
        m_panelButtonActionWaitUI.SetActive(wait);
        m_panelButtonActionHealUI.SetActive(heal);

        SetPanelActive(m_panelActionAttackUI, true, character);
    }

    public void ShowUnitMenu(CharacterManager character)
    {
        SetPanelActive(UniteMenu, true, character);
    }

    public void SetPanelActive(GameObject panel, bool active, CharacterManager character)
    {
        panel.SetActive(active);
        m_currentCharacterManager = character;
    }

    public void SetActiveTurnPlayer(bool active)
    {
        _turnPlayerOne.SetActive(active);
        _turnPlayerTwo.SetActive(!active);
    }

    public void Attack()
    {
        GameManager.Instance.EType_Phase = PhaseType.EType_AttackPhase;
        m_currentCharacterManager.SetHasAttacked(true);
        m_currentCharacterManager.Attack();
        SetAllGameObjectInactive();
    }

    public void Heal()
    {
        GameManager.Instance.EType_Phase = PhaseType.EType_HealPhase;
        CharacterHealer character = m_currentCharacterManager as CharacterHealer;
        character.Heal();
        SetAllGameObjectInactive();
    }

    public void Wait()
    {
        m_currentCharacterManager.SetHasAlreadyPlayed(true);
        m_currentCharacterManager.SetHasMoved(true);
        SetAllGameObjectInactive();
    }

    public void SetAllGameObjectInactive()
    {
        m_panelActionAttackUI.SetActive(false);
        m_currentCharacterManager = null;
        //m_uniteMenu.SetActive(false);
    }

    public void ActivePanelVictory()
    {
        _turnPlayerOne.SetActive(false);
        _turnPlayerTwo.SetActive(false);
        SetAllGameObjectInactive();
        m_panelVictory.SetActive(true);
    }

    public void EndTurn()
    {
        hexgrid.EndTurn();
        UniteMenu.SetActive(false);
    }

    public void Cancel()
    {
        UniteMenu.SetActive(false);
    }
}
