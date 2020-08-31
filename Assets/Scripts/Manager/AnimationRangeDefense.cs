using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRangeDefense : AnimationDefense
{
    [SerializeField] private GameObject m_placeDamageAttack;
    
    public GameObject prefabFx;

    // Start is called before the first frame update
    protected override void Start()
    {
        m_placeDamageAttack = HUDInGame.Instance._placeDefenseRangeUnits[index % 3];
      
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
