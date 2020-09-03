using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRangeDefense : AnimationDefense
{
    protected GameObject m_placeDamageAttack;
    public GameObject prefabFx;

    // Start is called before the first frame update
    protected override void Start()
    {
        m_placeDamageAttack = HUDInGame.Instance._placeDefenseRangeUnits;
        base.Start();
    }


    public void TriggerAnimDamage()
    {
        SoundManager.PlaySound(character._character.soundAttackRange);
        Instantiate(prefabFx, m_placeDamageAttack.transform);
    }

    public override void Animation()
    {
        base.Animation();
        StartCoroutine(AnimFx());
    }

    IEnumerator AnimFx()
    {

        yield return new WaitForSeconds(3 + character._character._timeBeforeSpell);
    
        TriggerAnimDamage();
    }

    public override void DamageHealthBar(int currentHealth, int damage, float second)
    {
        second += character._character._timeBeforeSpell;
        base.DamageHealthBar(currentHealth, damage, second);
    }
}
