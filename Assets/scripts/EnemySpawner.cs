using System.Collections;
using System.Collections.Generic;
using Assets.Script;
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
        int count = 0;
        foreach (Transform t in spawnPoints)
        {
            if (t.CompareTag("EnemySpawnPoint"))
            {
                GameObject go = Instantiate(EnemyPrefab, t.position, Quaternion.Euler(new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f)));
                Transform minimapObj = go.transform.FindChild("MinimapObject");
                minimapObj.parent = null;
                minimapObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                float rand = Random.Range(0.7f, 0.9f);
                
                go.transform.FindDeepChild("teddysculp").GetComponent<SkinnedMeshRenderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                go.transform.localScale = new Vector3(rand, rand, rand);

                minimapObj.parent = go.transform;
                count++;
            }  
        }
        transform.root.GetComponent<Level>().SetEnemyCount(count);
    }
}
