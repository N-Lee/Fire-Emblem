using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSync : MonoBehaviour
{
    static AnimationSync instance;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public static AnimationSync myInstance
    {
        get 
        {
            if (instance == null)
            {
                instance  = FindAnyObjectByType<AnimationSync>();
            }
            
            return instance;
        }
    }

    public float GetAnimationFrame()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
