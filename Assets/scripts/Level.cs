using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    private List<Transform> entrypoints;
    public int roomCount;
	// Use this for initialization
	void Start ()
	{
        //get all children and find entrypoints from them
        entrypoints = new List<Transform>();
	    Transform ep = transform.Find("EntryPoints");
        foreach (Transform t in ep)
        {
            if (t.CompareTag("EntryPoint"))
                entrypoints.Add(t);
        }
        roomCount = Random.Range(2, entrypoints.Count + 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}



    //inits the room and returns the entrypoint used to enter the room (RECURSIVE)
    public EntryPoint init(List<GameObject> availableAreas, EntryPoint prevArea, bool deadend)
    {
        //select rnd assigned entrypoint
        int rnd = Random.Range(0, entrypoints.Count);
        EntryPoint entry = entrypoints[rnd].GetComponent<EntryPoint>();

        //ensure that the next area entrance isn't in the same direction as the one you just left
        if (prevArea != null && entry.compassDirection == prevArea.compassDirection)
        {
            entrypoints.RemoveAt(rnd);
            rnd = Random.Range(0, entrypoints.Count);
            EntryPoint temp = entry;
            entry = entrypoints[rnd].GetComponent<EntryPoint>();
            entrypoints.Add(temp.transform);
        }
        //set tree inactive to make a way for walking
        entrypoints[rnd].GetChild(0).gameObject.SetActive(false);
        //set previous area for the entrance
        entrypoints[rnd].GetComponent<EntryPoint>().otherSidePoint = prevArea;
        //set player location to start
        if (prevArea == null)
            GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>().Warp(entrypoints[rnd].transform.position);
        //remove used entrypoint
        
        roomCount--;

#if UNITY_EDITOR
        if(prevArea != null)
            Debug.DrawLine(entry.transform.position, prevArea.transform.position, Color.blue, float.PositiveInfinity);
#endif
        //check if there's enough rooms to continue recursive initiation
        bool last = availableAreas.Count <= roomCount;
        if (!deadend)
        {
            if (last)
            {
                //find bossarea and assign random entrypoint for bossroom
                Level area = GameObject.FindGameObjectWithTag("BossArea").GetComponent<Level>();
                rnd = Random.Range(0, entrypoints.Count);
                entrypoints[rnd].GetChild(0).gameObject.SetActive(false);
                entrypoints[rnd].GetComponent<EntryPoint>().otherSidePoint = area.init(availableAreas, entrypoints[rnd].GetComponent<EntryPoint>(), true);
                entrypoints.RemoveAt(rnd); 
                Dictionary<EntryPoint, Level> usedareas = new Dictionary<EntryPoint, Level>();
                //use the rest of the available rooms if there's any
                for (int i = 0; i < availableAreas.Count; i++)
                {
                    Level area2 = availableAreas[Random.Range(0, availableAreas.Count)].GetComponent<Level>();
                    rnd = Random.Range(0, entrypoints.Count);
                    entrypoints[rnd].GetChild(0).gameObject.SetActive(false);
                    usedareas.Add(entrypoints[rnd].GetComponent<EntryPoint>(), area2);
                    availableAreas.Remove(area2.gameObject);
                    entrypoints.RemoveAt(rnd);
                    
                }
                int count = 0;
                foreach (KeyValuePair<EntryPoint, Level> pair in usedareas)
                {
                    // make last rooms deadends
                    pair.Key.otherSidePoint = pair.Value.init(availableAreas, pair.Key, true);
                    count++;
                }
            }
            else
            {
                //if there's enough rooms to continue
                Dictionary<EntryPoint, Level> usedareas = new Dictionary<EntryPoint, Level>();
                for (int i = 0; i < roomCount; i++)
                {
                    //Take random room
                    Level area = availableAreas[Random.Range(0, availableAreas.Count)].GetComponent<Level>();
                    //take random entrypoint
                    rnd = Random.Range(0, entrypoints.Count);
                    //set blocking tree mesh inactive
                    entrypoints[rnd].GetChild(0).gameObject.SetActive(false);
                    //pair the entrypoints to levels
                    usedareas.Add(entrypoints[rnd].GetComponent<EntryPoint>(), area);
                    availableAreas.Remove(area.gameObject);
                    entrypoints.RemoveAt(rnd);
                }
                int count = 0;
                //init recursively
                foreach(KeyValuePair<EntryPoint, Level> pair in usedareas)
                {
                    //make the first one always the "right way", others should be deadends
                    if (count == 0)
                        pair.Key.otherSidePoint = pair.Value.init(availableAreas, pair.Key, false);
                    else
                        pair.Key.otherSidePoint = pair.Value.init(availableAreas, pair.Key, true);
                    count++;
                }
            }
        }
        return entry;
    }
}
