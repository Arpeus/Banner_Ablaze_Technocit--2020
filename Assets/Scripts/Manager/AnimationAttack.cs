using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAttack : AnimationManager
{
    protected override void Start()
    {
        terrain = HUDInGame.Instance._terrainAttack;
        placeAttack = HUDInGame.Instance._placeAttackUnits[index];
        placeMissFx = HUDInGame.Instance._missFXAttack;
        base.Start();
    }

    public override void Animation()
    {
        base.Animation();
        TriggerAnimAttack();
    }
}
