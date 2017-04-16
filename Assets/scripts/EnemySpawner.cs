using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using Assets.Scripts.Weapon_Inventary;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] EnemyPrefabs;
    public List<Transform> spawnPoints;
    public List<GameObject> enemies;

	// Use this for initialization
	void Start ()
	{

	    Init();
        enemies = new List<GameObject>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void KillAllEnemies()
    {
        foreach (GameObject go in enemies)
        {
            go.GetComponent<Stats>().CurrentLifeEnergy = 0;
        }
    }

    void Init()
    {
        spawnPoints = new List<Transform>();
        foreach (Transform t in transform)
        {
            spawnPoints.Add(t);
        }

        if(EnemyPrefabs == null || EnemyPrefabs.Length == 0)
        {
            EnemyPrefabs = Resources.LoadAll<GameObject>("Enemies/");
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
                GameObject go = Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)], t.position, Quaternion.Euler(new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f)));
                Transform minimapObj = go.transform.FindChild("MinimapObject");
                minimapObj.parent = null;
                minimapObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                float rand = Random.Range(0.7f, 0.9f);
                //Do randomization only for teddies
                if (go.transform.FindDeepChild("teddysculp") != null)
                {
                    go.transform.FindDeepChild("teddysculp").GetComponent<SkinnedMeshRenderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                    go.transform.localScale = new Vector3(rand, rand, rand);
                }
                minimapObj.parent = go.transform;
                enemies.Add(go);
                count++;
            }  
        }
        transform.root.GetComponent<Level>().initiated = true;
        transform.root.GetComponent<Level>().enemyCount = count;
        Debug.Log(count);
    }
}
