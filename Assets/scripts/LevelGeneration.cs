using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LevelGeneration : MonoBehaviour
{
    private List<GameObject> areas;
    private GameObject startingArea;
    public String FirstRoomName = null;


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
        FirstRoomName = FirstRoomName.Trim();
        if (FirstRoomName.Equals(""))
        {
            startingArea = areas[Random.Range(0, areas.Count)];

        }
        else
        {
            var startingAreaTemp = areas.First(o => o.gameObject.name.Equals(FirstRoomName));
            startingArea = startingAreaTemp;
        }
        areas.Remove(startingArea);
        EntryPoint startingEntryPoint = startingArea.GetComponent<Level>().init(areas, null, false);

        //set player location to start
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SetCurrentArea(startingArea.GetComponent<Level>());
        GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<Minimap>().SetArea(startingArea.transform);
        GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>().Warp(startingEntryPoint.playerTeleportPoint.position);

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().UpdateProgressionText();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Init();
    }
}
