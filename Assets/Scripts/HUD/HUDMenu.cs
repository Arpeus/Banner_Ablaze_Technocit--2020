using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDMenu : MonoBehaviour
{
    public Player _player;

    public TextMeshProUGUI _pointAvailable;

    public Button _btnReady;
    // RESPECT ORDER
    // 0 --> Cavalier
    // 1 --> SwordMan
    // 2 --> Lancer
    [Header("UI Create Team")]
    public List<Button> _btnAddTroops;
    [SerializeField] private GameObject[] m_placeBtnRemoveSpawn;
    [SerializeField] private GameObject[] m_removeTroops;
    [SerializeField] private GameObject[] m_panelInfoTroop;
    [Header("UI Show Map")]
    [SerializeField] private GameObject m_goShowCreateTeamUI;
    [SerializeField] private GameObject m_goHideCreateTeamUI;
 
    // Start is called before the first frame update
    void Start()
    {
        SetTextAvailablePoint();
    }

    /// <summary>
    /// Set new value of the available point when smthing is added or removed
    /// Enable or disable button if non in condition
    /// </summary>
    public void SetTextAvailablePoint()
    {
        _pointAvailable.SetText(_player.GetAvailablePoint() + " / " + _player.GetMaxPoint());
        if (_player.GetAvailablePoint() == 0)
        {
            _btnReady.interactable = true;
            foreach (Button btnAdd in _btnAddTroops)
            {
                btnAdd.interactable = false;
            }
        }
        else
        {
            _btnReady.interactable = false;
        }
    }

    /// <summary>
    /// Display new value of number troop 
    /// Index means which troop
    /// </summary>
    /// <param name="index"></param>
    public void DisplayNumberTroop(int index)
    {
        foreach (BtnRemoveScript btnRemove in FindObjectsOfType<BtnRemoveScript>())
        {
            if (btnRemove.indexBtn == index)
            {
                btnRemove.GetComponentInChildren<TextMeshProUGUI>().SetText(_player.Nbtroops[index].ToString());
            }
        }
    }

    public void CheckEnoughPoint(CharacterManager character, int index)
    {
        if (_player.CheckEnoughPoint(character))
        {
            _btnAddTroops[index].interactable = true;
        }
        else
        {
            _btnAddTroops[index].interactable = false;
        }
    }

    public void AddBtnRemove(int index)
    {
        int i = 0;
        foreach(GameObject go in m_placeBtnRemoveSpawn)
        {
            if(go.GetComponentInChildren<Button>() == null)
            {
                Instantiate(m_removeTroops[index], m_placeBtnRemoveSpawn[i].transform);
                break;
            }
            i++;
        }
    }

    public void RemoveBtnRemove(int index)
    {
        foreach(BtnRemoveScript btnRemove in FindObjectsOfType<BtnRemoveScript>())
        {
            if(btnRemove.indexBtn == index)
            {
                btnRemove.Destroy();
            }
        }
    }

    public void DisplayInfoTroop(int index)
    {
        m_panelInfoTroop[index].SetActive(true);
    }

    public void GoToSceneSecondPlayerTeam()
    {
        SceneManager.LoadScene(3);
    }

    public void GoToGameScene()
    {
        GameManager.Instance.EType_Phase = PhaseType.EType_SpawnPhasePlayerOne;
        SceneManager.LoadScene(4);
    }

    public void AddCharacter(int index)
    {
        _player.AddCharacter(index);
    }

    public void RemoveCharacter(int index)
    {
        _player.RemoveCharacter(index);
    }

    public void DisplayUI(bool display)
    {
        m_goShowCreateTeamUI.SetActive(display);
        m_goHideCreateTeamUI.SetActive(!display);
    }
}
