using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    private Level currentArea;
    private EnemySpawner areasSpawner;
    public List<GameObject> weapons;
    public int progression = 0;
    public int levelsToBoss = 0;
    private Minimap minimap;


    // Use this for initialization
    void Start () {
        Object[] loadedweapons = Resources.LoadAll("/Assets/Weapons");
	    
        foreach (Object weapon in loadedweapons)
        {
            weapons.Add((GameObject)weapon);
        }
        minimap = GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<Minimap>();
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
        if (currentArea.transform.CompareTag("BossArea"))
        {
            if (!currentArea.Completed)
            {
                currentArea.GetComponent<BossArea>().SpawnBoss();
            } 
        }
        minimap.SetArea(currentArea.transform);
        FogCurrentMiniMap();
    }

    public Level GetCurrentArea()
    {
        return currentArea;
    }

    public void UpdateProgressionText()
    {
        GameObject.FindGameObjectWithTag("ProgressionText").GetComponent<Text>().text = "Room: " + progression + "/" + levelsToBoss;
    }

    public void RevealCurrentMiniMap()
    {
        minimap.transform.FindDeepChild("FogOfWarPlane").GetComponent<Renderer>().material.SetColor("_Color", new Color(0,0,0,0));
    }
    public void FogCurrentMiniMap()
    {
        minimap.transform.FindDeepChild("FogOfWarPlane").GetComponent<Renderer>().material.SetColor("_Color", new Color(0.2f, 0.2f, 0.2f, 1));
    }

    public void OpenCurrentAreasEntries()
    {
        if (!currentArea.Completed)
        {
            currentArea.Completed = true;
            if(currentArea.rightway)
            {
                progression++;
                UpdateProgressionText();
            }
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
