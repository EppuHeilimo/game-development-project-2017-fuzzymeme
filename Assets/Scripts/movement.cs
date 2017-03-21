using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movement : MonoBehaviour {


    public Vector3 player;
    public Camera Main_camera;
    public int speed;


    Vector3 previous;
    float playerVelocity;
    public Text velocityText;

    // Use this for initialization
    void Start () {
        velocityText.text = "velocity:" + playerVelocity.ToString();
        
        
    }
	
	// Update is called once per frame
	void Update () {

        speedLimit();
        
        aiming();//AIMING
      
    }


void FixedUpdate()
    {
        //CHARACTER MOVEMENT
        characterMovement();

    }



void aiming()
    {
        
        Ray cameraRay = Main_camera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up,transform.position);
        float rayLength;
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength); // Where the player looks
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z)); //look to directions on plane level (x,z)
            
        }
    }

void speedLimit()
    {
        playerVelocity = ((transform.position - previous).magnitude) / Time.deltaTime;
        previous = transform.position;
        velocityText.text = "velocity:" + playerVelocity.ToString();

    }


void characterMovement()
    {
        Vector3 direction = new Vector3(0, 0, 0);
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.z = Input.GetAxisRaw("Vertical");
        direction.Normalize(); // normalize for the diretion -1 - 1.
        transform.Translate(direction.x * speed * Time.deltaTime, 0, direction.z * speed * Time.deltaTime, Space.World); // our WASD controls is related to the WORLD, not the players axis.
         
    }


        
        }
