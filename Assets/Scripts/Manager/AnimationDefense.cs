using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDefense : AnimationManager
{
    // Start is called before the first frame update
    protected override void Start()
    {
        healthBar = HUDInGame.Instance.healthBarScriptDefense;
        healthBarUI = HUDInGame.Instance.healthBarUIDefense;
        terrain = HUDInGame.Instance._terrainDefense;
        placeMissFx = HUDInGame.Instance._missFXDefense;
        placeAttack = HUDInGame.Instance._placeDefenseUnits[index];
        base.Start();
    }

    public override void Animation()
    {
        base.Animation();
        StartCoroutine(AnimDefense());
    }

    public override void AnimationDodgeFX()
    {
        StartCoroutine(AnimDodge());
    }

    IEnumerator AnimDefense()
    {
       int second = 0;
       while (second < 3)
       {
           yield return new WaitForSeconds(1);
           second++;
       }
       TriggerAnimAttack();
       second = 0;    
    }

    IEnumerator AnimDodge()
    {
        int second = 0;
        while (second < 1)
        {
            yield return new WaitForSeconds(1);
            second++;
        }
        TriggerDodgeFX(); 
    }
}
