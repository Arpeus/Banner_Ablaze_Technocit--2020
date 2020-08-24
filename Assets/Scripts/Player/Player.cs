﻿using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private HUDMenu m_hudMenu = null;

    [Header("Game info")]
    public List<CharacterManager> m_characters = new List<CharacterManager>();
    [SerializeField] private int _countMax = 20;



    [SerializeField] private int m_currentCount = 0;

    [SerializeField] private int[] m_nbtroops = new int[6];
    [SerializeField] private bool[] m_fromZeroTroop = new bool[6];
   
    [SerializeField] private int m_nbCavalier = 0;
    private bool m_fromZeroCavalier = false;
    [SerializeField] private int m_nbSwordMan = 0;
    private bool m_fromZeroSwordMan = false;
    [SerializeField] private int m_nbLancer = 0;
    private bool m_fromZeroLancer = false;
    [SerializeField] private int m_nbRedMage = 0;
    private bool m_fromZeroRedMage = false;
    [SerializeField] private int m_nbWhiteMage = 0;
    private bool m_fromZeroWhiteMage = false;
    [SerializeField] private int m_nbBlackMage = 0;
    private bool m_fromZeroBlackMage = false;

    private string m_displayName = "Loading";
    

    public HUDMenu HudMenu { get => m_hudMenu; set => m_hudMenu = value; }
   
    public int NbCavalier { get => m_nbCavalier; set => m_nbCavalier = value; }
    public int NbSwordMan { get => m_nbSwordMan; set => m_nbSwordMan = value; }
    public int NbLancer { get => m_nbLancer; set => m_nbLancer = value; }
    public int NbRedMage { get => m_nbRedMage; set => m_nbRedMage = value; }
    public int NbWhiteMage { get => m_nbWhiteMage; set => m_nbWhiteMage = value; }
    public int NbBlackMage { get => m_nbBlackMage; set => m_nbBlackMage = value; }
    public int[] Nbtroops { get => m_nbtroops; set => m_nbtroops = value; }

    public void SetDisplayName(string displayName)
    {
        this.m_displayName = displayName;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameManager.Instance._players.Add(this);
        m_currentCount = _countMax;
    }
    
    public void AddCharacter(int index)
    {
        CharacterManager character = GameManager.Instance._characters[index];
        if (CheckEnoughPoint(character))
        {
            m_currentCount -= character._character._unitCost;
            SetHUDMenuUI(index, 1);
        }
    }
    public void RemoveCharacter(int index)
    {
        CharacterManager character = GameManager.Instance._characters[index];
        m_currentCount += character._character._unitCost;
        if (m_currentCount > _countMax)
            m_currentCount = _countMax;
        SetHUDMenuUI(index, -1);
    }
    
    public int GetAvailablePoint()
    {
        return m_currentCount;
    }

    public int GetMaxPoint()
    {
        return _countMax;
    }

    public bool CheckEnoughPoint(CharacterManager character)
    {
        return m_currentCount - character._character._unitCost >= 0;
    }

    private void SetHUDMenuUI(int index, int valueToAdd)
    {
        Nbtroops[index] += valueToAdd;
        AddBtnRemove(ref m_fromZeroTroop[index], index, Nbtroops[index]);
        /*switch (index)
        {
            case 0:
                NbCavalier += valueToAdd;
                AddBtnRemove(ref m_fromZeroCavalier, index, m_nbCavalier );
                break;
            case 1:
                NbSwordMan += valueToAdd;
                AddBtnRemove(ref m_fromZeroSwordMan, index, m_nbSwordMan);
                break;
            case 2:
                NbLancer += valueToAdd;
                AddBtnRemove(ref m_fromZeroLancer, index, m_nbLancer);
                break;
            case 3:
                NbRedMage += valueToAdd;
                AddBtnRemove(ref m_fromZeroRedMage, index, m_nbRedMage);
                break;
            case 4:
                NbWhiteMage += valueToAdd;
                AddBtnRemove(ref m_fromZeroWhiteMage, index, m_nbWhiteMage);
                break;
            case 5:
                NbBlackMage += valueToAdd;
                AddBtnRemove(ref m_fromZeroBlackMage, index, m_nbBlackMage);
                break;
        }*/
        for(int i = 0; i < GameManager.Instance._characters.Count; i++)
        {
            m_hudMenu.CheckEnoughPoint(GameManager.Instance._characters[i], i);
        }

        m_hudMenu.SetTextAvailablePoint();
        m_hudMenu.DisplayNumberTroop(index);
    }

    public void AddBtnRemove(ref bool fromZeroUnit, int index, int nbTroop)
    {
        if (!fromZeroUnit && nbTroop == 1)
        {
            m_hudMenu.AddBtnRemove(index);
            fromZeroUnit = !fromZeroUnit;
        }
        else if (fromZeroUnit && nbTroop == 0)
        {
            Debug.Log("remove");
            m_hudMenu.RemoveBtnRemove(index);
            fromZeroUnit = !fromZeroUnit;
        }
    } 
}
