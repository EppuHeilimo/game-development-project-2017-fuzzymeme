using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class LevelGeneration : MonoBehaviour
{
    private List<GameObject> areas;
    private GameObject startingArea;

	// Use this for initialization
	void Start ()
	{
        areas = GameObject.FindGameObjectsWithTag("Area").ToList();
	    RecursivelyInitLevels();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //uses all areas recursively
    void RecursivelyInitLevels()
    {
        startingArea = areas[Random.Range(0, areas.Count)];
        areas.Remove(startingArea);
        EntryPoint startingEntryPoint = startingArea.GetComponent<Level>().init(areas, null, false);

        //set player location to start
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SetCurrentArea(startingArea.GetComponent<Level>());
        GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<Minimap>().SetArea(startingArea.transform);
        GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>().Warp(startingEntryPoint.playerTeleportPoint.position);
    }
}
