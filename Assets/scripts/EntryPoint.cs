using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntryPoint : MonoBehaviour
{
    public EntryPoint OtherSidePoint;
    private GameObject player;
    private bool locked = false;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && OtherSidePoint != null && !locked)
        {
            Debug.Log("Teleported to " + transform.parent.parent.name);
            player.GetComponent<NavMeshAgent>().Warp(OtherSidePoint.transform.position);
            OtherSidePoint.locked = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            locked = false;
        }
            
    }
}
