using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationManager : MonoBehaviour
{
    protected GameObject placeAttack;
    protected GameObject terrain;
    protected GameObject placeMissFx;
    protected Animator animatorAttack;
    protected SpriteRenderer sprite;

    public GameObject prefabMissFX;
    public int index;



    protected virtual void Start()
    {
        sprite = placeAttack.GetComponent<SpriteRenderer>();
        animatorAttack = placeAttack.GetComponent<Animator>();
    }

    public void SetActiveAttackGameObject(bool active, Sprite sprite)
    {
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
