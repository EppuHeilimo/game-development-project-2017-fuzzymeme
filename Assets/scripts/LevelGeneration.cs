using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        startingArea.GetComponent<Level>().init(areas, null, false);
    }
}
