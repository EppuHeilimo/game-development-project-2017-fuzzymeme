using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{


    public Transform playerTransform;
    public Transform thirdPersonPosition;
    public Transform LookPoint;
    Vector3 offset;
    private Transform LockedTo;

    private float rotateSpeed = 5f;
    private float damping = 1f;
    private bool translating = false;
    private float translationSpeed = 35f;


    private EntryPoint targetEntryPoint;
    private bool playerTeleported = false;
    private Minimap minimap;

    private int cameraMode = 0;
    // Use this for initialization
    void Start ()
	{
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        thirdPersonPosition = playerTransform.FindChild("ThirdPersonCameraPosition");
        LookPoint = playerTransform.FindChild("LookPoint");
        offset = playerTransform.position - thirdPersonPosition.position;
        LockedTo = playerTransform;
	    transform.position = LockedTo.position;
        minimap = GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<Minimap>();
    }
	
	// Update is called once per frame
	void LateUpdate ()
	{
	    if (!translating && !playerTeleported)
        {
            if(cameraMode == 0)
            {
                transform.position = LockedTo.position;
            }
            else if(cameraMode == 1)
            {
                transform.position = LockedTo.position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, playerTransform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
        } 
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
            if (cameraMode == 0)
                LockedTo = playerTransform;
            else if (cameraMode == 1)
                LockedTo = thirdPersonPosition;
            minimap.SetArea(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetCurrentArea().transform);
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            ToggleCameraMode();
        }

	}

    private void ToggleCameraMode()
    {
        if(cameraMode == 0)
        {
            LockedTo = thirdPersonPosition;
            cameraMode = 1;

        }
        else if(cameraMode == 1)
        {
            LockedTo = playerTransform;
            cameraMode = 0;
        }
        playerTransform.GetComponent<PlayerMovement>().CameraMode = cameraMode;
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
