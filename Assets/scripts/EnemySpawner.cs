using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject EnemyPrefab;
    public Transform[] spawnPoints;

	// Use this for initialization
	void Start ()
	{
	    spawnPoints = transform.GetComponentsInChildren<Transform>();
	    foreach (Transform t in spawnPoints)
	    {
            if(t.CompareTag("EnemySpawnPoint"))
	            Instantiate(EnemyPrefab, t.position, Quaternion.Euler(new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f)));
	    }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
