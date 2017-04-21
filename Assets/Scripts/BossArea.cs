using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour
{

    public GameObject BossPrefab;
    public Transform bossSpawnPoint;
    private GameObject boss;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnBoss()
    {
        boss = Instantiate(BossPrefab, bossSpawnPoint.position, BossPrefab.transform.rotation);
        boss.GetComponent<BossAI>().CenterOfZone = bossSpawnPoint;
    }
}
