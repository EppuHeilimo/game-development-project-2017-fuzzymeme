using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyAnimation : MonoBehaviour
{
    public bool moving = false;
    private Animator anim;
    private float runSpeed = 3f;
	// Use this for initialization
	void Start ()
	{
	    anim = transform.FindChild("teddy").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

	    if (moving)
	    {
	        anim.SetBool("Moving", true);
	        anim.speed = runSpeed;
	    }
	    else
	    {
            anim.SetBool("Moving", false);
        }
	}

    public void Init(float runSpeed)
    {
        this.runSpeed = runSpeed/3;
    }
}
