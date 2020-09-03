using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AnimationManager : MonoBehaviour
{
    protected CharacterManager character;

    protected GameObject placeAttack;
    protected GameObject terrain;
    protected GameObject placeMissFx;
    protected GameObject placeHitFX;
    public GameObject backGroundWar;
    protected Animator animatorAttack;
    protected SpriteRenderer spriteEquip;
    protected Image healthBarUI;

    protected GameObject UIHealthBar;
    protected HealthBar healthBar;

    public GameObject prefabMissFX;
    public GameObject prefabHitFX;
    public int index;
    public RuntimeAnimatorController animatorTeam;
    public Sprite spriteTeam;
    public Sprite spriteHealthBarUI;
    
    protected virtual void Start()
    {
        backGroundWar = HUDInGame.Instance._backgroundAnim;
        character = GetComponent<CharacterManager>();
        spriteEquip = placeAttack.GetComponent<SpriteRenderer>();
        animatorAttack = placeAttack.GetComponent<Animator>();
    }

    public void SetActiveAttackGameObject(bool active, Sprite sprite)
    {
        if(active)
        {
            healthBar.SetLifeBar(character.m_lifeManager.MaxHealth, character.m_lifeManager.Health);
            healthBarUI.sprite = spriteHealthBarUI;
            spriteEquip.sprite = spriteTeam;
            animatorAttack.runtimeAnimatorController = animatorTeam;
            terrain.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        backGroundWar.SetActive(active);
        healthBarUI.enabled = active;
        spriteEquip.enabled = active;
        placeAttack.SetActive(active);
        terrain.SetActive(active);
        UIHealthBar.SetActive(active);
    }

    public void TriggerAnimAttack()
    {
        SoundManager.PlaySound(character._character.soundAttack);
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

    public void AnimationDodgeFX(float second)
    {
        StartCoroutine(AnimDodge(second));
    }

    IEnumerator AnimDodge(float second)
    {
        yield return new WaitForSeconds(second);
        TriggerDodgeFX();
    }

    public void TriggerDodgeFX()
    {
        SoundManager.PlaySound(Sound.Miss);
        Instantiate(prefabMissFX, placeMissFx.transform);
    }

    public void TriggerHitFX()
    {
        SoundManager.PlaySound(Sound.Hit);
        Instantiate(prefabHitFX, placeHitFX.transform);
    }

    public void Blink(float second)
    {
        StartCoroutine(Die(second));
    }

    public virtual void DamageHealthBar(int currentHealth, int damage, float second)
    {
        Debug.Log(damage);
        StartCoroutine(LifeBar(currentHealth, damage, second));
    }

    IEnumerator LifeBar(int currentHealth, int damage, float second)
    {
        yield return new WaitForSeconds(second);
        TriggerHitFX();
        healthBar._value = currentHealth - damage;       
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
