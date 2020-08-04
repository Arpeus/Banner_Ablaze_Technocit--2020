using System.Collections;
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
    
    public List<Button> _btnAddTroops;
    // RESPECT ORDER
    // 0 --> Cavalier
    // 1 --> SwordMan
    // 2 --> Lancer
    public List<Button> _btnRemoveTroops;
    // SAME AS ABOVE

    // Start is called before the first frame update
    void Start()
    {
        SetTextAvailablePoint();
        foreach (Button btnRemove in _btnRemoveTroops)
        {
            btnRemove.interactable = false;
        }
    }

    /// <summary>
    /// Set new value of the available point when smthing is added or removed
    /// Enable or disable button if non in condition
    /// </summary>
    public void SetTextAvailablePoint()
    {
        _pointAvailable.SetText(_player.GetAvailablePoint() + " / " + _player.GetMaxPoint());
        if(_player.GetAvailablePoint() == 0)
        {    
            _btnReady.interactable = true;
            foreach(Button btnAdd in _btnAddTroops)
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
    /// Add one when add a troop to TextMeshPro
    /// </summary>
    /// <param name="txtNumberTroop"></param>
    public void IncrementNumberTroop(TextMeshProUGUI txtNumberTroop)
    {
        int tmpNumberTroop;
        int.TryParse(txtNumberTroop.text, out tmpNumberTroop);
        tmpNumberTroop++;
        txtNumberTroop.SetText(tmpNumberTroop.ToString());
    }

    /// <summary>
    /// Remove one when remove a troop to TextMshPro
    /// </summary>
    /// <param name="txtNumberTroop"></param>
    public void DecrementNumberTroop(TextMeshProUGUI txtNumberTroop)
    {
        int tmpNumberTroop;
        int.TryParse(txtNumberTroop.text, out tmpNumberTroop);
        tmpNumberTroop--;
        txtNumberTroop.SetText(tmpNumberTroop.ToString());    
    }

    /// <summary>
    /// Call Method in Player
    /// Check if there is a cavalier in the list (enable btn remove if yes)
    /// Check if there is enough point to add (enable btn add if yes)
    /// </summary>
    /// <param name="btnAddCavalier"></param>
    public void CheckCavalier(CharacterManager character)
    {
        if (_player.CheckCavalier())
        {
            _btnRemoveTroops[0].interactable = true;
        }
        else
        {
            _btnRemoveTroops[0].interactable = false;
        }
        CheckEnoughPoint(character, 0);
    }

    /// <summary>
    /// Look above for SwordMan
    /// </summary>
    /// <param name="btnAddSwordMan"></param>
    public void CheckSwordMan(CharacterManager character)
    {
        if (_player.CheckSwordMan())
            _btnRemoveTroops[1].interactable = true;
        else
            _btnRemoveTroops[1].interactable = false;
        CheckEnoughPoint(character, 1);
    }

    /// <summary>
    /// Look above for Lancer
    /// </summary>
    /// <param name="btnAddLancer"></param>
    public void CheckLancer(CharacterManager character)
    {
        if (_player.CheckLancer(_btnAddTroops[2]))
            _btnRemoveTroops[2].interactable = true;
        else
            _btnRemoveTroops[2].interactable = false;
        CheckEnoughPoint(character, 2);
    }

    private void CheckEnoughPoint(CharacterManager character, int index)
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

    public void GoToSceneSecondPlayerTeam()
    {
        SceneManager.LoadScene("MedericCreateTeamSecondPlayer");
    }
}
