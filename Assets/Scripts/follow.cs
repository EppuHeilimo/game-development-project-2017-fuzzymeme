using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour {

    //public GameObject player; // assign the player
    
    private Vector3 offset;
    GameObject player;


	void Start () {
      player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;

	}
	
	// Update is called once per frame
	void LateUpdate () {
        
        transform.position = player.transform.position + offset;
	}
}
