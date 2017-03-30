using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{


    public Transform playerTransform;
    private Transform LockedTo;

    
    private bool translating = false;
    private float translationSpeed = 35f;


    private EntryPoint targetEntryPoint;
    private bool playerTeleported = false;
	// Use this for initialization
	void Start ()
	{
	    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	    LockedTo = playerTransform;
	    transform.position = LockedTo.position;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
	    if (!translating && !playerTeleported)
	        transform.position = LockedTo.position;
	    else if (translating)
	    {
	        MoveTowardsTransform(translationSpeed*Time.deltaTime);
	        if (Vector3.Distance(transform.position, LockedTo.position) < 0.3f)
	        {
	            translating = false;
	        }
	    }
	    else if (playerTeleported)
	    {
	        TeleportToTransform(targetEntryPoint.cameraTarget);
	        translating = true;
	        playerTeleported = false;
	        LockedTo = playerTransform;
	    }

	}

    public void ToEntryPoint(Transform t1, EntryPoint targetEntrypoint)
    {
        LockedTo = t1;
        targetEntryPoint = targetEntrypoint;
        translating = true;
        playerTeleported = true;
    }

    void MoveTowardsTransform(float step)
    {
        transform.position = Vector3.MoveTowards(transform.position, LockedTo.position, step);
    }

    void TeleportToTransform(Transform trans)
    {
        transform.position = trans.position;
    }


}
