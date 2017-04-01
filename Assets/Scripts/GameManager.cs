using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using Assets.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    private Level currentArea;
    private EnemySpawner areasSpawner;
    public List<GameObject> weapons;

    
	// Use this for initialization
	void Start () {
        Object[] loadedweapons = Resources.LoadAll("/Assets/Weapons");
	    
        foreach (Object weapon in loadedweapons)
        {
            weapons.Add((GameObject)weapon);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCurrentArea(Level currentArea)
    {
        this.currentArea = currentArea;
        if (currentArea.transform.CompareTag("Area"))
        {
            if (!currentArea.Completed)
            {
                areasSpawner = currentArea.transform.FindDeepChild("EnemySpawnPoints").GetComponent<EnemySpawner>();
                areasSpawner.SpawnAll();
            }
        }
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
        currentArea.Completed = true;
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
