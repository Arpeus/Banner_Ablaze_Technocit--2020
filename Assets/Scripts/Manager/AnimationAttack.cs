using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAttack : MonoBehaviour
{
    public GameObject placeAttackUnit;
    public Animation animation;
    public Animator animator;
    public int index;

    void Start()
    {
        placeAttackUnit = HUDInGame.Instance._placeAttackUnits[index];
        animator = placeAttackUnit.GetComponent<Animator>();
        animation = placeAttackUnit.GetComponent<Animation>();
    }

    public void TriggerAnim()
    {
        placeAttackUnit.SetActive(true);
        animator.SetTrigger("_IsAttack");
    }
}
