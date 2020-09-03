using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationDefense : AnimationManager
{
    // Start is called before the first frame update
    protected override void Start()
    {
        placeHitFX = HUDInGame.Instance._hitFXDefense;
        UIHealthBar = HUDInGame.Instance.healthBarUIDefense;
        healthBar = UIHealthBar.GetComponent<HealthBar>();
        healthBarUI = UIHealthBar.GetComponent<Image>();
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

    public override void DamageHealthBar(int currentHealth, int damage, float second)
    {
        base.DamageHealthBar(currentHealth, damage, second);
    }

    IEnumerator AnimDefense()
    {
       yield return new WaitForSeconds(3);

       TriggerAnimAttack();
    }
}
