using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {


    public Vector3 player;
    private Camera main_camera;
    public int speed = 5;
    private PlayerAnimation animation;

    Vector3 previous;
    float playerVelocity;

    private Inventory inventory;

    // Use this for initialization
    void Start () {
        main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        inventory = GetComponent<Inventory>();
        animation = GetComponent<PlayerAnimation>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        SpeedLimit();  
        Aiming();//AIMING

        if(Input.GetKeyDown(KeyCode.F))
        {
            inventory.TakeUp();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            inventory.Previous();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventory.Next();
        }


        CharacterMovement();

    }
    
    


    void Aiming()
    {
        Ray cameraRay = main_camera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up,transform.position);
        float rayLength;
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength); // Where the player looks
            //Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z)); //look to directions on plane level (x,z)
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
        direction.z = Input.GetAxisRaw("Vertical");
        direction.x = Input.GetAxisRaw("Horizontal");
        //animation stuff
        animation.inputHorizontal = direction.x;
        animation.inputVertical = direction.z;

        direction.Normalize(); // normalize for the diretion -1 - 1.
        
        transform.Translate(direction.x * speed * Time.deltaTime, 0, direction.z * speed * Time.deltaTime, Space.World); // our WASD controls is related to the WORLD, not the players axis.
    }


        
}
