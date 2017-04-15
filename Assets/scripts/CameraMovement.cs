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

    private float rotateSpeed = 2f;
    private float damping = 5f;
    private bool translating = false;
    private float translationSpeed = 35f;


    private EntryPoint targetEntryPoint;
    private bool playerTeleported = false;
    private Minimap minimap;
    private float orthoSizeDefault;
    private int cameraMode = 0;
    private GameObject crosshair;
    private float maxYDistance = 20f;
    private float minYDistance = 5f;
    private PlayerMovement player;
    // Use this for initialization
    void Start ()
	{
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerTransform.GetComponent<PlayerMovement>();
        thirdPersonPosition = playerTransform.FindChild("ThirdPersonCameraPosition");
        LookPoint = playerTransform.FindChild("LookPoint");
        offset = playerTransform.position - thirdPersonPosition.position;
        LockedTo = playerTransform;
	    transform.position = LockedTo.position;
        minimap = GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<Minimap>();
        orthoSizeDefault = Camera.main.orthographicSize;
        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        crosshair.SetActive(false);
	}

    public GameObject GetCrosshair()
    {
        if(crosshair == null)
            crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        return crosshair;
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
                float desiredAngle = playerTransform.transform.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
                transform.position = playerTransform.transform.position - (rotation * offset);
                transform.LookAt(playerTransform.transform.position + playerTransform.forward * player.GetLookYPoint());
            }
        } 
	    else if (translating)
	    {
	        MoveTowardsTransform(translationSpeed * Time.deltaTime);
	        if (Vector3.Distance(transform.position, LockedTo.position) < 0.3f)
	        {
	            translating = false;
	            player.locked = false;
	        }
	    }
	    else if (playerTeleported)
	    {
	        player.locked = true;
	        TeleportToTransform(targetEntryPoint.cameraTarget);
	        translating = true;
	        playerTeleported = false;
	        if (cameraMode == 0)
	        {
                LockedTo = playerTransform;
            }   
            else if (cameraMode == 1)
            {
                LockedTo = thirdPersonPosition;
                playerTransform.LookAt(targetEntryPoint.transform);
                float desiredAngle = playerTransform.transform.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
                transform.position = playerTransform.transform.position - (rotation * offset);
                transform.LookAt(playerTransform.transform.position);
            }
                
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SetCurrentArea(targetEntryPoint.parentTerrain.GetComponent<Level>());
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
            Cursor.lockState = CursorLockMode.Locked;
            crosshair.SetActive(true);

        }
        else if(cameraMode == 1)
        {
            LockedTo = playerTransform;
            cameraMode = 0;
            crosshair.SetActive(false);

        }
        playerTransform.GetComponent<PlayerMovement>().CameraMode = cameraMode;
    }

    public void ToEntryPoint(Transform t1, EntryPoint targetEntrypoint)
    {
        LockedTo = t1;
        targetEntryPoint = targetEntrypoint;
        translating = true;
        playerTeleported = true;
        playerTransform.LookAt(t1);
        float desiredAngle = playerTransform.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        transform.position = playerTransform.transform.position - (rotation * offset);
        transform.LookAt(playerTransform.transform.position);
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
