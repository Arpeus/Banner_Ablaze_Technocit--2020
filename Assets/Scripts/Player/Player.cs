using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private List<CharacterManager> m_characters;

    public int _countMax = 20;

    [SerializeField]
    private int m_currentCount;


    // Start is called before the first frame update
    void Awake()
    {
        GameManager.Instance._players.Add(this);
        m_characters = new List<CharacterManager>();
        m_currentCount = _countMax;
    }

    public void AddCharacter(CharacterManager character)
    {
        if (CheckEnoughPoint(character))
        {
            m_characters.Add(character);
            m_currentCount -= character._character._unitCost;
        }
    }

    public void RemoveCharacter(CharacterManager character)
    {
        m_characters.Remove(character);
        m_currentCount += character._character._unitCost;
        if (m_currentCount > _countMax)
            m_currentCount = _countMax;
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
        foreach(CharacterManager character in m_characters)
        {
            if(character._character.type == TypePawn.Cavalier)
            { 
                return true;
            }
        }
        return false;
    }

    public bool CheckSwordMan()
    {
        foreach (CharacterManager character in m_characters)
        {
            if (character._character.type == TypePawn.SwordMan)
            {
                return true;
            }
        }
        
        return false;
    }

    public bool CheckLancer(Button btnAdd)
    {
        foreach (CharacterManager character in m_characters)
        {
            if (character._character.type == TypePawn.Lancer)
            {
                return true;
            }
        }
        
        return false;
    }

    public bool CheckEnoughPoint(CharacterManager character)
    {
        return m_currentCount - character._character._unitCost >= 0;
    }
}
