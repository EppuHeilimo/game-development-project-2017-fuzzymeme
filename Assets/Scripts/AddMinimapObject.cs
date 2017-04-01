using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMinimapObject : MonoBehaviour
{

    public GameObject prefab;
    
	// Use this for initialization
	void Start () {
	    foreach (Transform trans in transform)
	    {
	        Instantiate(prefab, new Vector3(trans.position.x, trans.position.y + 4, trans.position.z), trans.rotation).transform.parent = trans;
	    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
