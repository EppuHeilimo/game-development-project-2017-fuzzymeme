using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject EnemyPrefab;
    public List<Transform> spawnPoints;

	// Use this for initialization
	void Start ()
	{
	    Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Init()
    {
        spawnPoints = new List<Transform>();
        foreach (Transform t in transform)
        {
            spawnPoints.Add(t);
        }
    }

    public void SpawnAll()
    {
        //make sure spawnpoints have been initiated, might not happen on first zone. 
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Init();
        }
        foreach (Transform t in spawnPoints)
        {
            if (t.CompareTag("EnemySpawnPoint"))
                Instantiate(EnemyPrefab, t.position, Quaternion.Euler(new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f)));
        }
    }
}
