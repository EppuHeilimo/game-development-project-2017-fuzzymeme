using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour
{

    public GameObject BossPrefab;
    public Transform bossSpawnPoint;
    private GameObject boss;
    private bool bossSpawning = false;
    private float timer = 0f;
    private float bossSpawnDelay = 3f;
    private bool playerArrived = false;

    private Transform player;

	// Use this for initialization
	void Start ()
	{
	    player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
	    if (playerArrived)
	    {
	        if (Vector3.Distance(player.position, bossSpawnPoint.position) < 15f)
	        {
                bossSpawnPoint.GetComponent<ParticleSystem>().Play();
                bossSpawning = true;
	            playerArrived = false;
	        }
        }
        else if (bossSpawning)
        {
            timer += Time.deltaTime;
            if (timer > bossSpawnDelay)
            {
                boss = Instantiate(BossPrefab, bossSpawnPoint.position, BossPrefab.transform.rotation);
                boss.GetComponent<BossAI>().CenterOfZone = bossSpawnPoint;
                bossSpawning = false;
            }
        }

    }

    public void SpawnBoss()
    {
        playerArrived = true;

    }
}
