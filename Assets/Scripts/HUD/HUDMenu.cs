using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDMenu : MonoBehaviour
{
    public Player _player;

    public TextMeshProUGUI _pointAvailable;

    public Button _btnReady;
    public List<Button> _btnAddTroops;
    public Button _btnRemoveCavalier;
    public Button _btnRemoveSwordMan;
    public Button _btnRemoveLancer;




    // Start is called before the first frame update
    void Start()
    {
        SetTextAvailablePoint();
        _btnRemoveCavalier.interactable = false;
        _btnRemoveSwordMan.interactable = false;
        _btnRemoveLancer.interactable = false;
    }

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
            foreach (Button btnAdd in _btnAddTroops)
            {
                
                btnAdd.interactable = true;
            }
        }
    }

    public void IncrementNumberTroop(TextMeshProUGUI txtNumberTroop)
    {
        int tmpNumberTroop;
        int.TryParse(txtNumberTroop.text, out tmpNumberTroop);
        tmpNumberTroop++;
        txtNumberTroop.SetText(tmpNumberTroop.ToString());
    }

    public void DecrementNumberTroop(TextMeshProUGUI txtNumberTroop)
    {
        int tmpNumberTroop;
        int.TryParse(txtNumberTroop.text, out tmpNumberTroop);
        tmpNumberTroop--;
        txtNumberTroop.SetText(tmpNumberTroop.ToString());    
    }

    public void CheckCavalier()
    {
        if (_player.CheckCavalier())
            _btnRemoveCavalier.interactable = true;
        else
            _btnRemoveCavalier.interactable = false;
    }

    public void CheckSwordMan()
    {
        if (_player.CheckSwordMan())
            _btnRemoveSwordMan.interactable = true;
        else
            _btnRemoveSwordMan.interactable = false;
    }

    public void CheckLancer()
    {
        if (_player.CheckLancer())
            _btnRemoveLancer.interactable = true;
        else
            _btnRemoveLancer.interactable = false;
    }
}
