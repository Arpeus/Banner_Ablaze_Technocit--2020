using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDefense : AnimationManager
{    
    // Start is called before the first frame update
    protected override void Start()
    {
        placeAttack = HUDInGame.Instance._placeDefenseUnits[index];
        base.Start();
    }

    public override void Animation()
    {
        base.Animation();
        StartCoroutine(AnimDefense());
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
}
