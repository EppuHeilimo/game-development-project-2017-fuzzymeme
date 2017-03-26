using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    private Level currentArea;
    private EnemySpawner areasSpawner;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCurrentArea(Level currentArea)
    {
        this.currentArea = currentArea;
        areasSpawner = currentArea.transform.FindChild("EnemySpawnPoints").GetComponent<EnemySpawner>();
        areasSpawner.SpawnAll();
    }

    public Level GetCurrentArea()
    {
        return currentArea;
    }

    public void OpenCurrentAreasEntries()
    {
        EntryPoint[] entries = currentArea.GetComponentsInChildren<EntryPoint>();
        foreach (EntryPoint t in entries)
        { 
            if (t.CompareTag("EntryPoint"))
            {
                t.OpenPath();
            }
        }
    }

    public void CloseCurrentAreasEntries()
    {
        EntryPoint[] entries = currentArea.GetComponentsInChildren<EntryPoint>();
        foreach (EntryPoint t in entries)
        {
            if (t.CompareTag("EntryPoint"))
            {
                t.ClosePath();
            }
        }
    }

}
