using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDInGame : MonoBehaviour
{
    [Header("UI GameObject")]
    [SerializeField] private GameObject m_PanelSpawnUI;
    [SerializeField] private GameObject m_panelActionAttackUI;
    [SerializeField] private GameObject m_panelActionNoAttackUI;
    [SerializeField] private GameObject m_panelActionHealNoAttackUI;
    [SerializeField] private GameObject m_panelActionHealAttackUI;
    [SerializeField] private GameObject m_uniteMenu;

    [Header("UI Spawn")]
    [SerializeField] private GameObject[] m_placeBtnSpawn;
    [SerializeField] private GameObject[] m_btnSpawn;
    [SerializeField] private Button m_btnSpawnPlayerTwo;
    [SerializeField] private Button m_btnGoToTurnPhase;

    private CharacterManager m_currentCharacterManager;

    // Start is called before the first frame update
    void Start()
    {
        AddButton(0);
        //m_btnSpawnPlayerTwo.interactable = false;
    }

    void AddButtonSpawn(int index, int nbTroop)
    {
        int i = 0;
        foreach (GameObject go in m_placeBtnSpawn)
        {
            if (go.GetComponentInChildren<ButtonSpawn>() == null)
            {
                GameObject btnSpawn = Instantiate(m_btnSpawn[index], m_placeBtnSpawn[i].transform);
                btnSpawn.GetComponentInChildren<TextMeshProUGUI>().SetText(nbTroop.ToString());
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
                AddButtonSpawn(i, GameManager.Instance._players[indexPlayer].Nbtroops[i]);
            }
        }
    }

    public void ShowActionAttackUi(CharacterManager character)
    {
        SetPanelActive(m_panelActionAttackUI, true, character);
    }

    public void ShowActionNoAttackUi(CharacterManager character)
    {
        SetPanelActive(m_panelActionNoAttackUI, true, character);
    }

    public void ShowActionHealNoAttackUI(CharacterManager character)
    {
        SetPanelActive(m_panelActionHealNoAttackUI, true, character);
    }

    public void ShowActionHealAttackUI(CharacterManager character)
    {
        SetPanelActive(m_panelActionHealAttackUI, true, character);
    }

    public void ShowUnitMenu(CharacterManager character)
    {
        SetPanelActive(m_uniteMenu, true, character);
    }

    public void SetPanelActive(GameObject panel, bool active, CharacterManager character)
    {
        panel.SetActive(active);
        m_currentCharacterManager = character;
    }

    public void Attack()
    {
        GameManager.Instance.EType_Phase = PhaseType.EType_AttackPhase;
        m_currentCharacterManager.SetHasAttacked(true);
        m_currentCharacterManager.Attack();
        SetPanelActive(m_panelActionAttackUI, true, null);
    }

    public void Heal()
    {

    }

    public void Wait()
    {
        m_currentCharacterManager.SetHasAlreadyPlayed(true);
        m_currentCharacterManager.SetHasMoved(true);
        m_currentCharacterManager = null;
        if (m_panelActionAttackUI.activeSelf)
        {
            m_panelActionAttackUI.SetActive(false);
        }
        else
        {
            m_panelActionNoAttackUI.SetActive(false);
        }
    }

    public void SetAllGameObjectInactive()
    {
        m_panelActionAttackUI.SetActive(false);
        m_panelActionNoAttackUI.SetActive(false);
        m_panelActionHealNoAttackUI.SetActive(false);
        m_panelActionHealAttackUI.SetActive(false);
        //m_uniteMenu.SetActive(false);
    }
}
