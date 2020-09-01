using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PreviewHUD : MonoBehaviour
{
    [Header("Left preview")]
    public Sprite spriteLeft;
    public TextMeshProUGUI nameUnitLeft;
    public TextMeshProUGUI hitRateLeft;
    

    [Header("Right preview")]
    public Sprite spriteRight;
    public TextMeshProUGUI nameUnitRight;
    public TextMeshProUGUI hitRateRight;

    public void SetHUDPreview(CharacterManager characterAttack, CharacterManager characterDefense)
    {
        spriteLeft = characterAttack._character.spritePreview;
        nameUnitLeft.text = characterAttack._character._name;
        
        this.gameObject.SetActive(true);
    }

}
