using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonUnit : MonoBehaviour
{
    CharacterManager characterManager;
    private TextMeshProUGUI m_Texthealth;
    private TextMeshProUGUI m_TextArmor;
    private TextMeshProUGUI m_TextMovement;
    private TextMeshProUGUI m_TextResistanceMagic;
    
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisplayInformations()
    {
        m_Texthealth.text = characterManager.m_lifeManager.Health.ToString();
        m_TextArmor.text = characterManager._character._armor.ToString();
        //m_.text = characterManager.m_lifeManager.Health.ToString();
        m_TextArmor.text = characterManager.m_lifeManager.Health.ToString();
    }
}
