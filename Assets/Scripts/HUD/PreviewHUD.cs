using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreviewHUD : MonoBehaviour
{
    [Header("Left preview")]
    public Image spriteLeft;
    public TextMeshProUGUI nameUnitLeft;
    public TextMeshProUGUI hitRateLeft;
    public Slider leftSliderCurrentHealth;
    public Slider leftSliderHealthAfter;

    [Header("Right preview")]
    public Image spriteRight;
    public TextMeshProUGUI nameUnitRight;
    public TextMeshProUGUI hitRateRight;
    public Slider rightSliderCurrentHealth;
    public Slider rightSliderHealthAfter;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {

        }
        if(Input.GetMouseButtonDown(1))
        {
            CancelAttack();
        }
    }

    public void SetHUDPreview(CharacterManager characterAttack, CharacterManager characterDefense)
    {
        spriteLeft.sprite = characterAttack._character.spritePreview;
        nameUnitLeft.text = characterAttack._character._name;
        hitRateLeft.text = GetDodge(characterDefense, characterAttack);
        leftSliderHealthAfter.maxValue = leftSliderCurrentHealth.maxValue = characterAttack.m_lifeManager.MaxHealth;
        leftSliderCurrentHealth.value = characterAttack.m_lifeManager.Health;
        leftSliderHealthAfter.value = GetDamage(characterAttack, characterDefense);
  

        spriteRight.sprite = characterDefense._character.spritePreview;
        nameUnitRight.text = characterDefense._character._name;
        hitRateRight.text = GetDodge(characterAttack, characterDefense);
        rightSliderCurrentHealth.maxValue = characterDefense.m_lifeManager.MaxHealth;
        rightSliderCurrentHealth.value = characterDefense.m_lifeManager.Health;
        rightSliderHealthAfter.value = GetDamage(characterDefense, characterAttack);
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
