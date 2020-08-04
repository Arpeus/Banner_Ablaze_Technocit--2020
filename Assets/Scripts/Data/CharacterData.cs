using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Custom/Character", order = 0)]
public class CharacterData : ScriptableObject
{
    public string _name;
    public int _health;
    public int _armor;
    public int _attackDamage;
    public int _resistanceMagic;
    public int dodge;
    public int _movement;
    public int _range;
    public int _unitCost;
}
