using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{

    public enum UBodyAnimationState
    {
        Idle,
        OneHanded,
        TwoHanded
    }
    public enum LBodyAnimationState
    {
        Idle,
        RunForward,
        RunBackward
    }

    private UBodyAnimationState UBodyState = UBodyAnimationState.Idle;
    private LBodyAnimationState LBodyState = LBodyAnimationState.Idle;


    private Animator animator;
    private bool dead = false;
	// Use this for initialization
	void Start ()
	{
	    animator = GetComponent<Animator>();
        ChangeUpperState(UBodyState);
        ChangeLowerState(LBodyState);
	}
	

    public void ChangeUpperState(UBodyAnimationState state)
    {
        if(!dead)
            animator.SetInteger("UpperAnimationState", (int)state);
    }

    public void ChangeLowerState(LBodyAnimationState state)
    {
        if (!dead)
            animator.SetInteger("LowerAnimationState", (int)state);
    }

    public void Die()
    {
        animator.SetBool("Dead", true);
    }
}
