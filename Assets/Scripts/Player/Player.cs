using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    private HUDMenu m_hudMenu = null;

    [Header("Game info")]
    //[SerializeField] private Dictionary<int, CharacterManager> m_characters = new Dictionary<int, CharacterManager>();
    public Dictionary<int, CharacterManager> m_characters = new Dictionary<int, CharacterManager>();
    [SerializeField] private int _countMax = 20;


  
    [SerializeField] private int m_currentCount = 0;

    [SerializeField] private int m_key = 0;

   
    [SerializeField] private int m_nbCavalier = 0;

   
    [SerializeField] private int m_nbSwordMan = 0;

   
    [SerializeField] private int m_nbLancer = 0;

   
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
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //GameManager.Instance._players.Add(this);
        m_currentCount = _countMax;
    }
    
    public void CmdAddCharacter(int index)
    {
        RpcAddCharacter(index);
    }


    public void RpcAddCharacter(int index)
    {
      
            CharacterManager character = GameManager.Instance.character[index];
            if (CheckEnoughPoint(character))
            {
                m_characters.Add(m_key, character);
                m_currentCount -= character._character._unitCost;
                SetHUDMenuUI(index, 1);
                m_key++;
            }
        
    }

   
    public void CmdRemoveCharacter(int index)
    {
        RpcRemoveCharacter(index);
    }

   
    public void RpcRemoveCharacter(int index)
    {
       
            CharacterManager character = GameManager.Instance.character[index];
            int i = 0;
            foreach (var characterManager in m_characters)
            {
                if (characterManager.Value == character)
                {
                    m_characters.Remove(i);
                    break;
                }
                i++;
            }
            m_currentCount += character._character._unitCost;
            if (m_currentCount > _countMax)
                m_currentCount = _countMax;
            SetHUDMenuUI(index, -1);
        
    }

   
    public void CmdLoadMap()
    {
        Debug.Log("Test Cmd");
        RpcLoadMap();
    }
    
    public void RpcLoadMap()
    {
        
            Debug.Log("Création Map");
          
    }

    public int GetAvailablePoint()
    {
        return m_currentCount;
    }

    public int GetMaxPoint()
    {
        return _countMax;
    }

    public bool CheckCavalier()
    {
        if (NbCavalier == 0)
            return false;
        return true;
    }

    public bool CheckSwordMan()
    {
        if (NbSwordMan == 0)
            return false;
        return true;
    }

    public bool CheckLancer()
    {
        if (NbLancer == 0)
            return false;
        return true;
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
                break;
            case 1:
                NbSwordMan += valueToAdd;
                break;
            case 2:
                NbLancer += valueToAdd;
                break;
        }
        m_hudMenu.SetTextAvailablePoint();
        m_hudMenu.DisplayNumberTroop(index);
        m_hudMenu.CheckCavalier(GameManager.Instance.character[0]);
        m_hudMenu.CheckSwordMan(GameManager.Instance.character[1]);
        m_hudMenu.CheckLancer(GameManager.Instance.character[2]);
    }



 
}
