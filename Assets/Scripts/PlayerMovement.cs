using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {


    public Vector3 player;
    private Camera main_camera;
    public int speed = 5;
    private PlayerAnimation animation;
    private float xrotateSpeed = 30f;
    private float yrotateSpeed = 20f;

    private GameObject crosshair;

    Vector3 previous;
    float playerVelocity;

    private Inventory inventory;
    public Transform LookPoint;
    public int CameraMode = 0;
    public float yLookPoint;
    public float maxY = 7f;
    public float minY = 5f;
    private float corsshairY = 0f;
    public bool locked = false;

    // Use this for initialization
    void Start () {
        main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        inventory = GetComponent<Inventory>();
        animation = GetComponent<PlayerAnimation>();
        LookPoint = transform.FindChild("LookPoint");
        crosshair = main_camera.GetComponent<CameraMovement>().GetCrosshair();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (!locked)
	    {
            SpeedLimit();
            Aiming();//AIMING

            if (Console.GetKeyDown(KeyCode.F))
            {
                inventory.TakeUp();
            }
            if (Console.GetKeyDown(KeyCode.E))
            {
                inventory.Previous();
            }
            if (Console.GetKeyDown(KeyCode.Q))
            {
                inventory.Next();
            }
	        if (CameraMode == 1)
	        {
                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    if (Cursor.lockState == CursorLockMode.Locked)
                        Cursor.lockState = CursorLockMode.None;
                    if (Cursor.lockState == CursorLockMode.None)
                        Cursor.lockState = CursorLockMode.Locked;
                }
            }
            CharacterMovement();
        }
    }

    void Aiming()
    {

        Vector3 point = new Vector3();

        Ray cameraRay = main_camera.ScreenPointToRay(Input.mousePosition);
        LayerMask layer = (1 << 11) | (1 << 13);
        RaycastHit hit;
        if (Physics.Raycast(cameraRay, out hit, float.PositiveInfinity, layer))
        {
            if (hit.collider.gameObject.transform.CompareTag("Enemy"))
            {
                point = new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z);
            }
            else
            {
                Plane groundPlane = new Plane(Vector3.up, transform.position);
                float rayLength;
                if (groundPlane.Raycast(cameraRay, out rayLength))
                {
                    point = cameraRay.GetPoint(rayLength); // Where the player looks                                       
                }
            }
        }
        LookPoint.position = new Vector3(point.x, transform.position.y, point.z);
        if(CameraMode == 0)
            transform.LookAt(LookPoint); //look to directions on plane level (x,z)
        else if (CameraMode == 1)
        {
            float mousex = Input.GetAxis("Mouse X") * xrotateSpeed * Time.deltaTime;
            float mousey = Input.GetAxis("Mouse Y") * yrotateSpeed * Time.deltaTime;
            yLookPoint += mousey;
            transform.Rotate(0, mousex, 0);

            if (yLookPoint >= maxY )
            {
                crosshair.transform.Translate(0, mousey * yrotateSpeed / 2, 0);  
            }
            else if (yLookPoint <= minY)
            {
                crosshair.transform.Translate(0, mousey*yrotateSpeed / 2, 0);
            }
        }
    }

    public float GetLookYPoint()
    {
        if (yLookPoint >= maxY)
        {
            return maxY;
        }
        else if (yLookPoint <= minY)
        {
            return minY;
        }
        else
        {
            return yLookPoint;
        }
    }

    void SpeedLimit()
    {
        playerVelocity = ((transform.position - previous).magnitude) / Time.deltaTime;
        previous = transform.position;
    }


    void CharacterMovement()
    {
        Vector3 direction = new Vector3(0, 0, 0);
        direction.z = Console.GetAxisRaw("Vertical");
        direction.x = Console.GetAxisRaw("Horizontal");
        //animation stuff
        animation.inputHorizontal = direction.x;
        animation.inputVertical = direction.z;
        direction.Normalize(); // normalize for the diretion -1 - 1.
        if (CameraMode == 0)
        {
            transform.Translate(direction.x * speed * Time.deltaTime, 0, direction.z * speed * Time.deltaTime, Space.World); // our WASD controls is related to the WORLD, not the players axis.
        }
        else if (CameraMode == 1)
        {
            transform.Translate(direction.x * speed * Time.deltaTime, 0, direction.z * speed * Time.deltaTime, transform); // our WASD controls is related to the WORLD, not the players axis.
        }
    }   
}
