using System.Collections;
using System.Collections.Generic;
using Assets.Script;
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
    private GameObject apertureMask1;
    private GameObject apertureMask2;
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
        
        orthoSizeDefault = Camera.main.orthographicSize;
        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        crosshair.SetActive(false);
        apertureMask1 = playerTransform.FindDeepChild("ApertureMask").gameObject;
        apertureMask2 = playerTransform.FindDeepChild("ApertureMask2").gameObject;
        apertureMask2.SetActive(false);
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
	        if (cameraMode == 0)
	        {
                if (Vector3.Distance(transform.position, LockedTo.position) < 0.3f)
                {
                    translating = false;
                    player.locked = false;
                }
            }
            else if (cameraMode == 1)
            {
                if (Vector3.Distance(transform.position, LockedTo.position) < 1f)
                {
                    translating = false;
                    player.locked = false;
                    playerTransform.LookAt(targetEntryPoint.transform);
                    float desiredAngle = playerTransform.transform.eulerAngles.y;
                    Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
                    transform.position = playerTransform.transform.position - (rotation * offset);
                    transform.LookAt(playerTransform.transform.position);
                }
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

            }
                
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SetCurrentArea(targetEntryPoint.parentTerrain.GetComponent<Level>());
            
        }
        if(Console.GetKeyDown(KeyCode.P))
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
            apertureMask2.SetActive(true);
            apertureMask1.SetActive(false);

        }
        else if(cameraMode == 1)
        {
            LockedTo = playerTransform;
            cameraMode = 0;
            crosshair.SetActive(false);
            apertureMask2.SetActive(true);
            apertureMask1.SetActive(false);

        }
        playerTransform.GetComponent<PlayerMovement>().CameraMode = cameraMode;
    }

    public void ToEntryPoint(Transform t1, EntryPoint targetEntrypoint)
    {
        LockedTo = t1;
        targetEntryPoint = targetEntrypoint;
        translating = true;
        playerTeleported = true;

        if (cameraMode == 1)
        {
            playerTransform.LookAt(t1);
            float desiredAngle = playerTransform.transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
            transform.position = playerTransform.transform.position - (rotation * offset);
            transform.LookAt(playerTransform.transform.position);
        }

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
