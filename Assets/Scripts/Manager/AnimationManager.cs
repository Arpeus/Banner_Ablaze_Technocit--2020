using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AnimationManager : MonoBehaviour
{
    protected GameObject placeAttack;
    protected GameObject terrain;
    protected GameObject placeMissFx;
    protected Animator animatorAttack;
    protected SpriteRenderer spriteEquip;
    protected Image healthBarUI;

    protected HealthBar healthBar;

    public GameObject prefabMissFX;
    public int index;
    public RuntimeAnimatorController animatorTeam;
    public Sprite spriteTeam;
    public Sprite spriteHealthBarUI;
    
    protected virtual void Start()
    {
        spriteEquip = placeAttack.GetComponent<SpriteRenderer>();
        animatorAttack = placeAttack.GetComponent<Animator>();
    }

    public void SetActiveAttackGameObject(bool active, Sprite sprite)
    {
        healthBarUI.sprite = spriteHealthBarUI;
        healthBarUI.enabled = active;
        spriteEquip.enabled = active;
        spriteEquip.sprite = spriteTeam;
        animatorAttack.runtimeAnimatorController = animatorTeam;
        terrain.GetComponent<SpriteRenderer>().sprite = sprite;
        placeAttack.SetActive(active);
        terrain.SetActive(active);
    }

    public void TriggerAnimAttack()
    {
        animatorAttack.SetTrigger("_IsAttack");
    }

    public virtual void Animation()
    {
        GameManager.Instance.EType_StateAnim = AnimState.EType_IsPlaying;
    }

    public void HideHUD()
    {
        StartCoroutine(TimerHideUI(6));
    }
    
    public void SetLayingOrder(SpriteRenderer sprite, int order)
    {
        sprite.sortingOrder = order;
    }

    public virtual void AnimationDodgeFX()
    {
    }

    public void TriggerDodgeFX()
    {
        Instantiate(prefabMissFX, placeMissFx.transform);
    }

    public void Blink(float second)
    {
        StartCoroutine(Die(second));
    }

    IEnumerator Die(float second)
    {
        
        yield return new WaitForSeconds(second);
        float endtime = Time.time + 1;
        while(Time.time < endtime)
        {
            spriteEquip.enabled = false;
            yield return new WaitForSeconds(0.2f);
            spriteEquip.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
        spriteEquip.enabled = false;
    }

    protected IEnumerator TimerHideUI(int maxSecond)
    {
        int second = 0;
        while (second < maxSecond)
        {
            yield return new WaitForSeconds(1);
            second++;
        }
        SetActiveAttackGameObject(false, null);
        GameManager.Instance.EType_StateAnim = AnimState.EType_IsNotPlaying;
    }
    
}
