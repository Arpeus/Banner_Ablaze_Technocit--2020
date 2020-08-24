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
        if (GameManager.Instance._players[indexPlayer].NbCavalier != 0)
        {
            AddButtonSpawn(0, GameManager.Instance._players[indexPlayer].NbCavalier);
        }
        if (GameManager.Instance._players[indexPlayer].NbSwordMan != 0)
        {
            AddButtonSpawn(1, GameManager.Instance._players[indexPlayer].NbSwordMan);
        }
        if (GameManager.Instance._players[indexPlayer].NbLancer != 0)
        {
            AddButtonSpawn(2, GameManager.Instance._players[indexPlayer].NbLancer);
        }
        if (GameManager.Instance._players[indexPlayer].NbRedMage != 0)
        {
            AddButtonSpawn(3, GameManager.Instance._players[indexPlayer].NbRedMage);
        }
        if (GameManager.Instance._players[indexPlayer].NbWhiteMage != 0)
        {
            AddButtonSpawn(4, GameManager.Instance._players[indexPlayer].NbWhiteMage);
        }
        if (GameManager.Instance._players[indexPlayer].NbBlackMage != 0)
        {
            AddButtonSpawn(5, GameManager.Instance._players[indexPlayer].NbBlackMage);
        }
    }

    public void ShowActionAttackUi(CharacterManager character)
    {
        m_panelActionAttackUI.SetActive(true);
        m_currentCharacterManager = character;
    }


    public void ShowActionNoAttackUi(CharacterManager character)
    {
        m_panelActionNoAttackUI.SetActive(true);
        m_currentCharacterManager = character;
    }

    public void Attack()
    {
        m_currentCharacterManager.SetHasAttacked(true);
        m_currentCharacterManager.Attack();
        m_currentCharacterManager = null;
        m_panelActionAttackUI.SetActive(false);
    }

    public void Wait()
    {
        m_currentCharacterManager.SetHasAlreadyPlayed(true);
        m_currentCharacterManager.SetHasMoved(true);
        m_currentCharacterManager = null;
        if(m_panelActionAttackUI.activeSelf) m_panelActionAttackUI.SetActive(false);
        else m_panelActionNoAttackUI.SetActive(false);
    }

    public void ShowUnitMenu(CharacterManager character)
    {
        m_uniteMenu.SetActive(true);
        m_currentCharacterManager = character;
    }
}
