using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAttack : MonoBehaviour
{
    public GameObject placeAttackUnit;
    public GameObject placeDefenseUnit;
    public Animation animationAttack;
    public Animator animatorAttack;
    public Animation animationDefense;
    public Animator animatorDefense;
    public int index;
    private CharacterManager m_characterManager;
    private SpriteRenderer spriteAttack;
    private SpriteRenderer spriteDefense;

    public CharacterManager CharacterManager { get => m_characterManager; set => m_characterManager = value; }

    void Start()
    {
        placeAttackUnit = HUDInGame.Instance._placeAttackUnits[index];
        placeDefenseUnit = HUDInGame.Instance._placeDefenseUnits[index];
        spriteAttack = placeAttackUnit.GetComponent<SpriteRenderer>();
        spriteDefense = placeDefenseUnit.GetComponent<SpriteRenderer>();
        animatorAttack = placeAttackUnit.GetComponent<Animator>();
        animationAttack = placeAttackUnit.GetComponent<Animation>();
        animatorDefense = placeDefenseUnit.GetComponent<Animator>();
        animationDefense = placeDefenseUnit.GetComponent<Animation>();
    }

    public void SetActiveAttackGameObject(bool active)
    {
        placeAttackUnit.SetActive(active);
    }

    public void SetActiveDefenseGameObject(bool active)
    {
        placeDefenseUnit.SetActive(active);
    }

    public void TriggerAnimAttack()
    {
        animatorAttack.SetTrigger("_IsAttack");
    }

    public void TriggerAnimDefense()
    {
        animatorDefense.SetTrigger("_IsAttack");
    }

    public void Animation()
    {
        GameManager.Instance.EType_StateAnim = AnimState.EType_IsPlaying;
        TriggerAnimAttack();
        SetLayingOrder(spriteAttack, 1);
        Debug.Log(animatorAttack.name);


        StartCoroutine(AnimDefense());
    }

    bool AnimatorIsPlaying()
    {
        return !animationAttack.isPlaying;
    }

    public void SetLayingOrder(SpriteRenderer sprite, int order)
    {
        sprite.sortingOrder = order;
    }

    IEnumerator AnimDefense()
    {
        int second = 0;
        while(second < 3)
        {
            yield return new WaitForSeconds(1);
            second++;
        }
        SetLayingOrder(spriteAttack, 0);
        m_characterManager.animattack.SetLayingOrder(m_characterManager.animattack.spriteAttack, 1);
        m_characterManager.animattack.TriggerAnimDefense();
        second = 0;
        while (second < 3)
        {
            yield return new WaitForSeconds(1);
            second++;
        }
        SetActiveAttackGameObject(false);
        m_characterManager.animattack.SetActiveDefenseGameObject(false);
        m_characterManager = null;
        GameManager.Instance.EType_StateAnim = AnimState.EType_IsNotPlaying;
    }
}
