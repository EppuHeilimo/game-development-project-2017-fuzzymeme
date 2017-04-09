using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyAnimation : MonoBehaviour
{

    public enum StandingState
    {
        RunForward = 0,
        Idle,
        RunBackward
    }

    public StandingState StandState = StandingState.Idle;
    public bool Standing = false;
    public bool Jump = false;


    private Animator animator;
    private bool locked = false;
    private BunnyAI ai;
    private float lockTimer = 0;
    private float lockTime = 0;
    private float runSpeed = 3f;

	// Use this for initialization
	void Start ()
	{
        animator = transform.FindChild("bunny").GetComponent<Animator>();
	    ai = transform.root.GetComponent<BunnyAI>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (animator.GetInteger("Run") != (int)StandState)
	    {
	        animator.speed = runSpeed;
            animator.SetInteger("Run", (int)StandState);
        }

	    if (animator.GetBool("Standing") != Standing)
	    {
            animator.SetBool("Standing", Standing);
	        locked = true;
	        lockTime = animator.GetCurrentAnimatorStateInfo(0).length;
            ai.locked = locked;
        }
	    if (locked)
	    {
	        lockTimer += Time.deltaTime;
	        if (lockTimer >= lockTime)
	        {
	            locked = false;
                ai.locked = locked;
            }
	    }

        if (Jump)
	    {
            animator.SetBool("Jump", true);
	        Jump = false;
	    }
        
    }

    public void Init(float speed)
    {
        runSpeed = speed/2.5f;
    }
}
