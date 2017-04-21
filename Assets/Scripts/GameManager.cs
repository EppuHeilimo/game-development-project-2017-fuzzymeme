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
    public int DropLevel = 1;
    private bool lightColorChanging = false;
    private Light Sun;
    //where g and b colors will be at boss
    private float targetSunColor = 0.4f;
    private float colorOffsetPerLevel;
    private float lightChangeSpeed = 0.2f;

    private GameObject gameWin;


    // Use this for initialization
    void Start ()
	{
	    Sun = GameObject.FindGameObjectWithTag("Sun").GetComponent<Light>();
        Object[] loadedweapons = Resources.LoadAll("/Assets/Weapons");
	    
        foreach (Object weapon in loadedweapons)
        {
            weapons.Add((GameObject)weapon);
        }

        minimap = GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<Minimap>();
        gameWin = GameObject.FindGameObjectWithTag("GameWinCanvas");
        gameWin.SetActive(false);
	}

    public void Init()
    {
        colorOffsetPerLevel = (1f - targetSunColor) / levelsToBoss;
    }
	
	// Update is called once per frame
	void Update () {
	    if (lightColorChanging)
	    {
	        float off = Time.deltaTime*lightChangeSpeed;
            Color currColor = Sun.color;
	        Sun.color = new Color(currColor.r, currColor.g - off, currColor.b - off);
	        if (Sun.color.g <= 1f - colorOffsetPerLevel * progression)
	        {
	            lightColorChanging = false;
                Debug.Log(currColor);
	        }
	    }	
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
                lightColorChanging = true;
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

    public void GameWin()
    {
        player.GetComponent<PlayerMovement>().locked = true;
        gameWin.SetActive(true);
    }

}
