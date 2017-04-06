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
    public int progression = 0;

    
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

    public void SetCurrentArea(Level area)
    {
        this.currentArea = area;
        if (currentArea.transform.CompareTag("Area"))
        {
            if (!currentArea.Completed)
            {
                areasSpawner = currentArea.transform.FindDeepChild("EnemySpawnPoints").GetComponent<EnemySpawner>();
                areasSpawner.SpawnAll();
                CloseCurrentAreasEntries();
            }
        }
    }

    public Level GetCurrentArea()
    {
        return currentArea;
    }

    public void OpenCurrentAreasEntries()
    {
        if (!currentArea.Completed)
        {
            currentArea.Completed = true;
            progression++;
        }
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
