using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private int m_health;
    [SerializeField] private int m_armor;
    [SerializeField] private int m_armorMagic;
    [SerializeField] private int m_dodge;

    public int Health { get => m_health; set => m_health = value; }

    public void SetHealth(int health)
    {
        this.Health = health;
    }

    public void SetArmor(int armor)
    {
        this.m_armor = armor;
    }

    public void SetArmorMargic(int armorMagic)
    {
        this.m_armorMagic = armorMagic;
    }

    public void SetDodge(int dodge)
    {
        this.m_dodge = dodge;
    }

    public void TakeDamage(CharacterManager characterDefense, CharacterManager characterAttack, int bonusDamage)
    {
        bool dodge = false;

        if (characterDefense._character.type == TypeCharacter.SwordMan && characterAttack._character.type == TypeCharacter.Lancer)
            dodge = Random.Range(1, 100) > (m_dodge * 2);
        else
            dodge = Random.Range(1, 100) > m_dodge;

        if (dodge)
        {
            switch (characterAttack._character.typeDamage)
            {
                case TypeDamage.Physic:
                    Health -= (characterAttack._character._attackDamage + bonusDamage - m_armor) ;
                    break;
                case TypeDamage.Magic:
                    Health -= (characterAttack._character._attackDamage + bonusDamage - m_armorMagic);
                    break;
            }
        }
        else
        {
            Debug.Log("Dodge");
        }
        
        
    }
}
