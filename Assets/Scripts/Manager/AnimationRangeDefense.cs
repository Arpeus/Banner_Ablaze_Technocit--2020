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
        int second = 0;
        while (second < 4)
        {
            yield return new WaitForSeconds(1);
            second++;
        }
        TriggerAnimDamage();
    }


}
