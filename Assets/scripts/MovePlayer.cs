using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovePlayer : MonoBehaviour
{
    private float speed = 6f;
    public bool justTeleported = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 oldpos = transform.position;
            Vector3 newpos = new Vector3(oldpos.x + speed * Time.deltaTime, oldpos.y, oldpos.z );
            transform.position = newpos;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 oldpos = transform.position;
            Vector3 newpos = new Vector3(oldpos.x - speed * Time.deltaTime, oldpos.y, oldpos.z );
            
            transform.position = newpos;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 oldpos = transform.position;
            Vector3 newpos = new Vector3(oldpos.x , oldpos.y, oldpos.z + speed * Time.deltaTime);
            transform.position = newpos;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 oldpos = transform.position;
            Vector3 newpos = new Vector3(oldpos.x, oldpos.y, oldpos.z - speed * Time.deltaTime);
            transform.position = newpos;
        }

    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
        transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
    }


    void OnTriggerEnter(Collider other)
    {
    }

    void setCountText()
    {
    }
}