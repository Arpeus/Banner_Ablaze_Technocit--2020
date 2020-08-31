using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    private int m_maxHealth;
    private int m_health;
    private int m_armor;
    private int m_armorMagic;
    private int m_dodge;

    public int Health { get => m_health; set => m_health = value; }

    public void SetHealth(int health)
    {
        this.Health = health;
        m_maxHealth = health;
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

    public void TakeDamage(CharacterManager characterDefense, CharacterManager characterAttack, int bonusDamage, bool counterAttack, bool canCounter = true)
    {
        if(!counterAttack)
        {
            characterAttack.animattack.SetActiveAttackGameObject(true, characterAttack.GetSpriteTerrain());
            characterDefense.animDefense.SetActiveAttackGameObject(true, characterDefense.GetSpriteTerrain());
            characterAttack.animattack.Animation();
        }
        else
        {
            characterAttack.animDefense.Animation();
            characterAttack.animDefense.HideHUD();
            characterDefense.animattack.HideHUD();
        }

        bool dodge = false;
        int tmpDodge = 0;
        int tmpArmor = 0;

        if(characterDefense.Location.IsPlantLevel)
        {
            Debug.Log("test");
            tmpArmor += GameManager.Instance.bonusForestDefense;
            tmpDodge += GameManager.Instance.bonusForestDodge;
        }
        if(characterDefense.Location.HasRiver)
        {
            tmpDodge += GameManager.Instance.malusRiverDodge;
        }
        if(characterDefense.Location.IsSpecial)
        {
            tmpDodge += GameManager.Instance.bonusCastleDodge;
            tmpArmor += GameManager.Instance.bonusCastleDefense;
        }

        if (characterDefense._character.type == TypeCharacter.SwordMan && characterAttack._character.type == TypeCharacter.Lancer)
        {
            dodge = Random.Range(1, 100) <= ((tmpDodge + m_dodge) * 2);
        }
        else
        {
             dodge = Random.Range(1, 100) <= (tmpDodge + m_dodge);
        }
        
        if (!dodge)
        {
            int damage = 0;
            switch (characterAttack._character.typeDamage)
            {
                case TypeDamage.Physic:
                    int tmpCharacterAttack = characterAttack._character._attackDamage;
                    if (counterAttack == true)
                    {
                        if(characterAttack._character.type == TypeCharacter.Lancer)
                        {
                            tmpCharacterAttack = characterAttack._character._attackDamage / 2;
                        }
                    }
                    damage = tmpCharacterAttack + bonusDamage - (m_armor + tmpArmor);
                    break;
                case TypeDamage.Magic:
                    damage = characterAttack._character._attackDamage + bonusDamage - m_armorMagic;
                    break;
            }
            if (damage > 0)
                Health -= damage;

        }
        else
        {
            if (!counterAttack)
                characterDefense.animDefense.AnimationDodgeFX();
            else
                characterDefense.animattack.AnimationDodgeFX();
        }

        if (
                characterDefense.hasAttacked ||
                Health <= 0 ||
                canCounter == false ||
                counterAttack == true
            )
        {
            if(!counterAttack)
            {
                characterAttack.animattack.HideHUD();
                characterDefense.animDefense.HideHUD();
            }
            if (Health <= 0)
            {
                StartCoroutine(Die(gameObject.GetComponent<CharacterManager>()));
            }
            return;
        }
        else
        {
            characterDefense.hasAttacked = true;
            characterAttack.TakeDamage(characterDefense, true);
        }
    }

    public void ReceiveHeal(CharacterManager characterHealer)
    {
        Health += (characterHealer._character._attackDamage/2);
        if (Health > m_maxHealth)
            Health = m_maxHealth;
    }

    IEnumerator Die(CharacterManager character)
    {
        yield return new WaitUntil(CheckAnimationPlaying);
  
        switch (character._playerNumberType)
        {
            case PlayerNumber.EType_PlayerOne:
                GameManager.Instance._players[0].m_characters.Remove(character);
                break;
            case PlayerNumber.EType_PlayerTwo:
                GameManager.Instance._players[1].m_characters.Remove(character);
                break;
        }
        DestroyImmediate(gameObject);
        
    }

    private bool CheckAnimationPlaying()
    {
        return GameManager.Instance.EType_StateAnim == AnimState.EType_IsNotPlaying;
    }
}
