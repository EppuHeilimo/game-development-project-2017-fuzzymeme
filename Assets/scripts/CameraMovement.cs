using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerTransform;
    private Vector3 offset;

	// Use this for initialization
	void Start ()
	{
	    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	    offset = transform.position - playerTransform.position;
	    transform.position = playerTransform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.position = playerTransform.position;
	}
}
