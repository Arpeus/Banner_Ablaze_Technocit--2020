﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Custom/Character", order = 0)]
public class CharacterData : ScriptableObject
{
    public Sprite spriteInfoTroop;

    public Sprite spritePreviewTeamOne;
    public Sprite spritePreviewTeamTwo;
    public Sprite spriteMapDay;
    public Sprite spriteMapNight;
    public Sprite spriteAnimDayTeamOne;
    public Sprite spriteAnimDayTeamTwo;
    public Sprite spriteAnimNightTeamOne;
    public Sprite spriteAnimNightTeamTwo;

    public Sprite healthBarCombatTeamOne;
    public Sprite healthBarCombatTeamTwo;


    public RuntimeAnimatorController animatorMapDay;
    public RuntimeAnimatorController animatorMapNight;
    public RuntimeAnimatorController animatorDayTeamOne;
    public RuntimeAnimatorController animatorDayTeamTwo;
    public RuntimeAnimatorController animatorNightTeamOne;
    public RuntimeAnimatorController animatorNightTeamTwo;



    public string albedoColorGreen = "#8DB5A5";
    public string albedoColorRed = "#E0AA93";
    public string albedoColorGray = "#525252";
    public string _name;

    public int _health;
    public int _attackDamage;
    public int _armor;
    public int _resistanceMagic;
    public int _dodge;
    public int _movement;
    public int _range;
    public int _unitCost;
    public int _damageTriangle;

    public float _timeBeforeHit;
    public float _timeBeforeSpell;

    public TypeCharacter type;
    public TypeCharacter typeBonusDamage;
    public TypeDamage typeDamage;
    public TypeRange typeRange;
    public Sound soundAttack;
    public Sound soundAttackRange;
}
