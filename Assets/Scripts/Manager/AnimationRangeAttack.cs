using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRangeAttack : AnimationAttack
{
    protected GameObject m_placeDamageAttack;
    
    public GameObject prefabFx;

    protected override void Start()
    {
        m_placeDamageAttack = HUDInGame.Instance._placeAttackRangeUnits;
        base.Start();
    }

    public void TriggerAnimDamage()
    {
        Instantiate(prefabFx, m_placeDamageAttack.transform);
        SoundManager.PlaySound(character._character.soundAttackRange);
    }

    public override void Animation()
    {
        base.Animation();
        StartCoroutine(AnimFx());
    }

    IEnumerator AnimFx()
    {
        yield return new WaitForSeconds(character._character._timeBeforeSpell);

        TriggerAnimDamage();
    }

    public override void DamageHealthBar(int currentHealth, int damage, float second)
    {
        second += character._character._timeBeforeSpell;
        base.DamageHealthBar(currentHealth, damage, second);
    }
}
