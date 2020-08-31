using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationManager : MonoBehaviour
{
    public GameObject placeAttack;
    public Animation animationAttack;
    public Animator animatorAttack;
    public GameObject placeMissFx;
    public Animator missFx;
    private SpriteRenderer sprite;

    public int index;
   

    protected virtual void Start()
    {
        sprite = placeAttack.GetComponent<SpriteRenderer>();
        animatorAttack = placeAttack.GetComponent<Animator>();
        animationAttack = placeAttack.GetComponent<Animation>();
        //missFx = placeMissFx.GetComponent<Animator>();
    }

    public void SetActiveAttackGameObject(bool active)
    {
        placeAttack.SetActive(active);
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
   
    
    protected IEnumerator TimerHideUI(int maxSecond)
    {
        int second = 0;
        while (second < maxSecond)
        {
            yield return new WaitForSeconds(1);
            second++;
        }
        SetActiveAttackGameObject(false);
        GameManager.Instance.EType_StateAnim = AnimState.EType_IsNotPlaying;
    }
    
}
