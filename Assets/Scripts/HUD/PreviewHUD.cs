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
    

    [Header("Right preview")]
    public Image spriteRight;
    public TextMeshProUGUI nameUnitRight;
    public TextMeshProUGUI hitRateRight;

    public void SetHUDPreview(CharacterManager characterAttack, CharacterManager characterDefense)
    {
        spriteLeft.sprite = characterAttack._character.spritePreview;
        nameUnitLeft.text = characterAttack._character._name;
        hitRateLeft.text = GetDodge(characterDefense, characterAttack);

        spriteRight.sprite = characterDefense._character.spritePreview;
        nameUnitRight.text = characterDefense._character._name;
        hitRateRight.text = GetDodge(characterAttack, characterDefense); 

        this.gameObject.SetActive(true);
    }

    private string GetDodge(CharacterManager characterDefense, CharacterManager characterAttack)
    {
        return (100 - (characterDefense.GetDodge(characterAttack) + characterAttack._character._dodge)).ToString();
    }

}
