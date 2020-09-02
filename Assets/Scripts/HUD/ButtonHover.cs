using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    public CharacterData character;
    public HUDMenu _hudMenu;

    public void SetInfoTroop()
    {
        _hudMenu.text_Name.SetText(character._name);
        _hudMenu.imageTroop.sprite = character.spriteInfoTroop;
        if (character.typeRange == TypeRange.Cac)
        {
            _hudMenu.text_Attack.SetText(character._attackDamage.ToString());
            _hudMenu.text_AttackMagic.SetText("0");
        }
        else
        {
            _hudMenu.text_Attack.SetText("0");
            _hudMenu.text_AttackMagic.SetText(character._attackDamage.ToString());
        }
        _hudMenu.text_Defense.SetText(character._armor.ToString()); ;
        _hudMenu.text_DefenseMagic.SetText(character._resistanceMagic.ToString()); ;
        _hudMenu.text_Movement.SetText(character._movement.ToString()); ;
        _hudMenu.text_Dodge.SetText(character._dodge.ToString());
        _hudMenu.text_Point.SetText((character._unitCost + " points").ToString());
    }
}
