using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDMenu : MonoBehaviour
{
    public Player _player;
    public TextMeshProUGUI _pointAvailable;
    // Start is called before the first frame update
    void Start()
    {
        SetTextAvailablePoint();
    }

    public void SetTextAvailablePoint()
    {
        _pointAvailable.SetText(_player.GetAvailablePoint() + " / " + _player.GetMaxPoint());
    }
}
