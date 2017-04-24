using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawn : MonoBehaviour
{

    private GameObject MinionPrefab;
    private ParticleSystem particleSystem;
    private float timer = 0f;
    private float time = 2.5f;
    private bool minionSpawning = false;

	// Use this for initialization
	void Start ()
	{
	    particleSystem = GetComponent<ParticleSystem>();

	}
	
	// Update is called once per frame
	void Update () {
	    if (minionSpawning)
	    {
	        timer += Time.deltaTime;
	        if (timer >= time)
	        {
                Instantiate(MinionPrefab, transform.position, Quaternion.Euler(0, 180, 0));
	            minionSpawning = false;
	        }
	    }
	}

    public void Spawn(GameObject prefab)
    {
        timer = 0f;
        minionSpawning = true;
        MinionPrefab = prefab;
        particleSystem.Play();
    }
}
