﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDMenu : MonoBehaviour
{
    public Player _player;

    public TextMeshProUGUI _pointAvailable;

    // RESPECT ORDER
    // 0 --> Cavalier
    // 1 --> SwordMan
    // 2 --> Lancer
    [Header("UI Create Team")]
    public List<Button> _btnAddTroops;
    [SerializeField] private GameObject[] m_placeBtnRemoveSpawn;
    [SerializeField] private GameObject[] m_removeTroops;
    
    [Header("Show Info troops")]
    public Image imageTroop;
    public TextMeshProUGUI text_Name;
    public TextMeshProUGUI text_Health;
    public TextMeshProUGUI text_Attack;
    public TextMeshProUGUI text_AttackMagic;
    public TextMeshProUGUI text_Defense;
    public TextMeshProUGUI text_DefenseMagic;
    public TextMeshProUGUI text_Movement;
    public TextMeshProUGUI text_Dodge;
    public TextMeshProUGUI text_Point;
    
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
        _pointAvailable.SetText(_player.GetAvailablePoint().ToString());
        if (_player.GetAvailablePoint() == 0)
        {
            
            foreach (Button btnAdd in _btnAddTroops)
            {
                btnAdd.interactable = false;
            }
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
