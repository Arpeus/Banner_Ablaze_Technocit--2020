using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAttack : MonoBehaviour
{
    public GameObject placeAttackUnit;
    public Animation animation;
    public Animator animator;


    void Start()
    {
        placeAttackUnit = GameObject.FindGameObjectWithTag("CavalierAttack");
       
        animator = placeAttackUnit.GetComponent<Animator>();
        animation = placeAttackUnit.GetComponent<Animation>();
    }

    public void TriggerAnim()
    {
        placeAttackUnit.SetActive(true);
        animator.SetTrigger("_IsAttack");
    }
}
