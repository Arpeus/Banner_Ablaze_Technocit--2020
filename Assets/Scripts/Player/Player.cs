using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        m_characters = new List<CharacterManager>();
        m_currentCount = _countMax;
    }

    public void AddCharacter(CharacterManager character)
    {
        if(m_currentCount - character._character._unitCost >= 0)
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
}
