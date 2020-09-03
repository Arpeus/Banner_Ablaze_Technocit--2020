using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    private int m_maxHealth;
    [SerializeField]private int m_health;
    private int m_armor;
    private int m_armorMagic;
    private int m_dodge;

    public int Health { get => m_health; set => m_health = value; }
    public int MaxHealth { get => m_maxHealth; set => m_maxHealth = value; }
    

    public void SetHealth(int health)
    {
        this.Health = health;
        MaxHealth = health;
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
        
        int tmpDodge = characterDefense.GetDodge(characterAttack);
        int tmpArmor = characterDefense.GetArmor();
        dodge = Random.Range(1, 100) <= (tmpDodge + m_dodge);

        if (!dodge)
        {
            int damage = GetDamage(characterAttack, bonusDamage, tmpArmor);
            
            if (damage > 0)
            {

                if (!counterAttack)
                {
                    characterDefense.animDefense.DamageHealthBar(Health, damage, characterAttack._character._timeBeforeHit);
                    
                }
                else
                    characterDefense.animattack.DamageHealthBar(Health, damage, 4);
                Health -= damage;
            }

        }
        else
        {
            if (!counterAttack)
                characterDefense.animDefense.AnimationDodgeFX();
            else
                characterDefense.animattack.AnimationDodgeFX();
        }

        if (Health <= 0)
        {
            if (counterAttack)
                characterDefense.animattack.Blink(4);
            else
                characterDefense.animDefense.Blink(1.5f);
            StartCoroutine(Die(gameObject.GetComponent<CharacterManager>()));
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
            return;
        }
        else
        {
            characterDefense.hasAttacked = true;
            characterDefense.SetSpriteCounterAttack(false);
            characterAttack.TakeDamage(characterDefense, true);
        }
    }

    public void ReceiveHeal(CharacterManager characterHealer)
    {
        Health += (characterHealer._character._attackDamage/2);
        if (Health > MaxHealth)
            Health = MaxHealth;
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

    public int GetDamage(CharacterManager characterAttack, int bonusDamage, int tmpArmor)
    {
        switch (characterAttack._character.typeDamage)
        {
            case TypeDamage.Physic:
                int tmpCharacterAttack = characterAttack._character._attackDamage;
                /*if (counterAttack == true)
                {
                    if (characterAttack._character.type == TypeCharacter.Lancer)
                    {
                        tmpCharacterAttack = characterAttack._character._attackDamage / 2;
                    }
                }*/
                return tmpCharacterAttack + bonusDamage - (m_armor + tmpArmor);
            case TypeDamage.Magic:
                return characterAttack._character._attackDamage + bonusDamage - m_armorMagic;
        }
        return 0;
    }
}
