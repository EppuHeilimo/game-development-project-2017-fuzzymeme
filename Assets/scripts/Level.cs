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
    public RenderTexture minimapTexture;
    public RenderTexture fogOfWarTexture;
    public bool Completed = false;
    public int enemyCount = 0;
    public bool rightway = false;
    GameManager gameManager;
    public bool initiated = false;

    //public void Awake()
    //{
    //    bool compareTo = CompareTag("Area");
    //    if (!compareTo)
    //    {
    //        tag = "Area";
    //    }
    //}

    // Use this for initialization
    void Start ()
	{
        minimapTexture = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32);
        minimapTexture.antiAliasing = 2;
        fogOfWarTexture = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32);
        fogOfWarTexture.antiAliasing = 2;
        //get all children and find entrypoints from them
        entrypoints = new List<Transform>();
	    Transform ep = transform.Find("EntryPoints");
        foreach (Transform t in ep)
        {
            if (t.CompareTag("EntryPoint"))
                entrypoints.Add(t);
        }
        roomCount = Random.Range(2, entrypoints.Count + 1);
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

	
	// Update is called once per frame
	void Update () {

	    if (initiated && enemyCount <= 0 && !transform.root.CompareTag("BossArea"))
	    {
	        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().OpenCurrentAreasEntries();
	    }
	}

    //inits the room and returns the entrypoint used to enter the room (RECURSIVE)
    public EntryPoint init(List<GameObject> availableAreas, EntryPoint prevArea, bool deadend)
    {
        //select rnd assigned entrypoint
        int rnd = Random.Range(0, entrypoints.Count);
        EntryPoint entry = entrypoints[rnd].GetComponent<EntryPoint>();

        //ensure that the next area entrance isn't in the same direction as the one you just left
        if (prevArea != null && entry.compassDirection == prevArea.compassDirection && entrypoints.Count > 1)
        {
            entrypoints.RemoveAt(rnd);
            rnd = Random.Range(0, entrypoints.Count);
            EntryPoint temp = entry;
            entry = entrypoints[rnd].GetComponent<EntryPoint>();
            entrypoints.Add(temp.transform);
        }
        //set tree inactive to make a way for walking
        entrypoints[rnd].GetComponent<EntryPoint>().Init(true);
        //set previous area for the entrance
        entrypoints[rnd].GetComponent<EntryPoint>().otherSidePoint = prevArea;

            
        //remove used entrypoint
        entrypoints.RemoveAt(rnd);
        roomCount--;

#if UNITY_EDITOR
        if(prevArea != null)
            Debug.DrawLine(entry.transform.position, prevArea.transform.position, Color.blue, float.PositiveInfinity);
#endif
        //check if there's enough rooms to continue recursive initiation
        bool last = availableAreas.Count <= roomCount;
        if (!deadend)
        {
            rightway = true;
            gameManager.levelsToBoss++;
            if (last)
            {
                //find bossarea and assign random entrypoint for bossroom
                Level area = GameObject.FindGameObjectWithTag("BossArea").GetComponent<Level>();
                rnd = Random.Range(0, entrypoints.Count);
                entrypoints[rnd].GetComponent<EntryPoint>().Init(true);
                entrypoints[rnd].GetComponent<EntryPoint>().otherSidePoint = area.init(availableAreas, entrypoints[rnd].GetComponent<EntryPoint>(), true);
                entrypoints.RemoveAt(rnd);

                Dictionary<EntryPoint, Level> usedareas = new Dictionary<EntryPoint, Level>();
                //use the rest of the available rooms if there's any
                if(entrypoints.Count != 0)
                {
                    for (int i = 0; i < availableAreas.Count; i++)
                    {
                        Level area2 = availableAreas[Random.Range(0, availableAreas.Count)].GetComponent<Level>();
                        rnd = Random.Range(0, entrypoints.Count);
                        entrypoints[rnd].GetComponent<EntryPoint>().Init(true);
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
                    entrypoints[rnd].GetComponent<EntryPoint>().Init(true);
                    //pair the entrypoints to levels
                    usedareas.Add(entrypoints[rnd].GetComponent<EntryPoint>(), area);
                    availableAreas.Remove(area.gameObject);
                    entrypoints.RemoveAt(rnd);
                }
                int count = 0;
                //Init recursively
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
        foreach (Transform e in entrypoints)
        {
            e.GetComponent<EntryPoint>().Init(false);
        }
        return entry;
    }
}
