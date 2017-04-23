using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawn : MonoBehaviour
{

    public GameObject MinionPrefab;
    private ParticleSystem particleSystem;

	// Use this for initialization
	void Start ()
	{
	    particleSystem = GetComponent<ParticleSystem>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Spawn(GameObject prefab)
    {
        Instantiate(prefab, transform.position, Quaternion.Euler(0, 180, 0));
        particleSystem.Play();
    }
}
