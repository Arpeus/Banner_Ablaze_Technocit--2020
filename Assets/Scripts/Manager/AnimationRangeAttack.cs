using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRangeAttack : AnimationAttack
{
    [SerializeField] private GameObject m_placeDamageAttack;
    public GameObject prefabFx;
    private GameObject prefab;
    //[SerializeField] private Animator m_animatorDamage;

    // Start is called before the first frame update
    protected override void Start()
    {
        m_placeDamageAttack = HUDInGame.Instance._placeAttackRangeUnits;
        base.Start();
    }

    public void TriggerAnimDamage()
    {
        prefab = Instantiate(prefabFx, m_placeDamageAttack.transform);        
    }

    public override void Animation()
    {
        base.Animation();
        StartCoroutine(AnimFx());
    }

    IEnumerator AnimFx()
    {
        int second = 0;
        while (second < 1)
        {
            yield return new WaitForSeconds(1);
            second++;
        }
        TriggerAnimDamage();
    }
 

}
