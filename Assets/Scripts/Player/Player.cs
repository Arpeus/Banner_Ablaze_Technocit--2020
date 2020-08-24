using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]private HUDMenu m_hudMenu = null;

    [Header("Game info")]
    public List<CharacterManager> m_characters = new List<CharacterManager>();
    [SerializeField] private int _countMax = 20;


  
    [SerializeField] private int m_currentCount = 0;
   
    [SerializeField] private int m_nbCavalier = 0;
    private bool m_fromZeroCavalier = false;
    [SerializeField] private int m_nbSwordMan = 0;
    private bool m_fromZeroSwordMan = false;
    [SerializeField] private int m_nbLancer = 0;
    private bool m_fromZeroLancer = false;

    private string m_displayName = "Loading";
    

    public HUDMenu HudMenu { get => m_hudMenu; set => m_hudMenu = value; }
   
    public int NbCavalier { get => m_nbCavalier; set => m_nbCavalier = value; }
    public int NbSwordMan { get => m_nbSwordMan; set => m_nbSwordMan = value; }
    public int NbLancer { get => m_nbLancer; set => m_nbLancer = value; }

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
        switch (index)
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
        }
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
