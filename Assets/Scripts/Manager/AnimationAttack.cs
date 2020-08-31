using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAttack : AnimationManager
{
    protected override void Start()
    {
        placeAttack = HUDInGame.Instance._placeAttackUnits[index];
        base.Start();
    }

    public override void Animation()
    {
        base.Animation();
        TriggerAnimAttack();
        /*SetLayingOrder(spriteAttack, 1);
        Debug.Log(animatorAttack.name);

        if (animDefense)
            StartCoroutine(AnimDefense());
        else
            StartCoroutine(TimerHideUI());*/
    }
}
