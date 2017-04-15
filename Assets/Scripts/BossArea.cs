using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour
{

    public GameObject BossPrefab;
    public Transform bossSpawnPoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnBoss()
    {
        Instantiate(BossPrefab, bossSpawnPoint);
    }
}
