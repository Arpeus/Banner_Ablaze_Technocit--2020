using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRangeAttack : AnimationDefense
{
    [SerializeField] private GameObject m_placeDamageAttack;
    public GameObject prefabFx;
    //[SerializeField] private Animator m_animatorDamage;

    // Start is called before the first frame update
    protected override void Start()
    {
        m_placeDamageAttack = HUDInGame.Instance._placeAttackRangeUnits[index % 3];
        //m_animatorDamage = m_placeDamageAttack.GetComponent<Animator>();
        base.Start();
    }

    public void TriggerAnimDamage()
    {
        Instantiate(prefabFx, m_placeDamageAttack.transform);
    }

    public override void Animation()
    {
        base.Animation();
        TriggerAnimDamage();
    }
}
