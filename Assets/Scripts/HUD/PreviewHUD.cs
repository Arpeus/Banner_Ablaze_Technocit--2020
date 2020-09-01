using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreviewHUD : MonoBehaviour
{
    bool rangeAttack = false;
    [Header("Left preview")]
    public Image spriteLeft;
    public TextMeshProUGUI nameUnitLeft;
    public TextMeshProUGUI hitRateLeft;
    public Slider leftSliderCurrentHealth;
    public Slider leftSliderHealthAfter;
    CharacterManager characterAttack;

    [Header("Right preview")]
    public Image spriteRight;
    public TextMeshProUGUI nameUnitRight;
    public TextMeshProUGUI hitRateRight;
    public Slider rightSliderCurrentHealth;
    public Slider rightSliderHealthAfter;
    CharacterManager characterDefense;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(rangeAttack)
            {
                characterDefense.TakeDamageRange(characterAttack);
            }
            else
            {
                characterDefense.TakeDamage(characterAttack);
            }
            characterAttack.SetHasAlreadyPlayed(true);
            characterAttack.ClearEnemy();
            characterAttack.SetStateTurn();
            CancelAttack();
        }
        if(Input.GetMouseButtonDown(1))
        {
            CancelAttack();
        }
    }

    public void SetHUDPreview(CharacterManager characterAttack, CharacterManager characterDefense, bool range)
    {
        this.characterAttack = characterAttack;
        this.characterDefense = characterDefense;
        this.rangeAttack = range;
        
        spriteLeft.sprite = characterAttack._spritePreview;
        nameUnitLeft.text = characterAttack._character._name;
        hitRateLeft.text = GetDodge(characterDefense, characterAttack);
        leftSliderHealthAfter.maxValue = leftSliderCurrentHealth.maxValue = characterAttack.m_lifeManager.MaxHealth;
        leftSliderCurrentHealth.value = characterAttack.m_lifeManager.Health;
        leftSliderHealthAfter.value = characterDefense.hasAttacked ? characterAttack.m_lifeManager.Health - 0 : characterAttack.m_lifeManager.Health - GetDamage(characterAttack, characterDefense);
        

        spriteRight.sprite = characterDefense._spritePreview;
        nameUnitRight.text = characterDefense._character._name;
        hitRateRight.text = GetDodge(characterAttack, characterDefense);
        rightSliderHealthAfter.maxValue = rightSliderCurrentHealth.maxValue = characterDefense.m_lifeManager.MaxHealth;
        rightSliderCurrentHealth.value = characterDefense.m_lifeManager.Health;
        rightSliderHealthAfter.value = characterDefense.m_lifeManager.Health - GetDamage(characterDefense, characterAttack);
        this.gameObject.SetActive(true);
    }

    private string GetDodge(CharacterManager characterDefense, CharacterManager characterAttack)
    {
        return (100 - (characterDefense.GetDodge(characterAttack) + characterAttack._character._dodge)).ToString();
    }

    private int GetDamage(CharacterManager characterDefense, CharacterManager characterAttack)
    {
        return characterDefense.m_lifeManager.GetDamage(characterAttack, characterDefense.BonusDamage(characterAttack), characterDefense.GetArmor());
    }


    private void CancelAttack()
    {
        this.gameObject.SetActive(false);
    }
}
