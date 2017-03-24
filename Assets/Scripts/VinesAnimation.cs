using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinesAnimation : MonoBehaviour
{
    private Animator animator;

    enum DoorState
    {
        IdleClosed = 0,
        Opening,
        IdleOpen,
        Closing
    }

    private DoorState state =
    DoorState.IdleClosed;

    private float animationtime = 0;
    private float timer = 0;
    
	// Use this for initialization
	void Start ()
	{
	    animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (state == DoorState.Opening)
	    {
	        timer += Time.deltaTime;
	        if (timer >= animationtime)
	        {
	            animator.Play("IdleOpen");
                state = DoorState.IdleOpen;
	        }
	    }
        else if (state == DoorState.Closing)
        {
            timer += Time.deltaTime;
            if (timer >= animationtime)
            {
                animator.Play("IdleClose");
                state = DoorState.IdleClosed;
            }
        }
    }

    public void Open()
    {
        if (state == DoorState.IdleClosed)
        {
            timer = 0;
            animator.Play("Open");
            animationtime = animator.GetCurrentAnimatorStateInfo(0).length;
            state = DoorState.Opening;

        }

    }
    public void Close()
    {
        if (state == DoorState.IdleOpen)
        {
            timer = 0;
            animator.Play("Close");
            animationtime = animator.GetCurrentAnimatorStateInfo(0).length;
            state = DoorState.Closing;

        }
    }
}
