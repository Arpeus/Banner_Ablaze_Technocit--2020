using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRangeAttack : AnimationAttack
{
    [SerializeField] private GameObject m_placeDamageAttack;
    [SerializeField] private Animator m_animatorDamage;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        m_placeDamageAttack = HUDInGame.Instance._placeAttackUnitsRange[index%3];
        m_animatorDamage = m_placeDamageAttack.GetComponent<Animator>();
        base.Start();
    }

    public void TriggerAnimDamage()
    {

    }

    public override void Animation()
    {
        base.Animation();
        TriggerAnimDamage();
    }
}
