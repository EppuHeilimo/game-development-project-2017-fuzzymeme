/**
 * The DropHandler Class calculates the drop rates of different
 * weapons and afterwards, if necessary, creates the drop itself.
 * It will place the gameobject on the field
 **/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHandler : MonoBehaviour {

    public double DropChance = 1.0; // Has to be a value between 0.0 and 1.0
    private List<GameObject> differentWeapons; // List of all the weapon prefabs 
    private GameObject test;  // test value


    /**
     * First function to be executed!
     * Gets the list of all weapon prefabs used  in the project. 
     **/
    private void Start()
    {
        differentWeapons = new List<GameObject>();
        /*
        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        GameManager gm = obj.GetComponent<GameManager>();
        differentWeapons = gm.weapons;
        test = differentWeapons[0];     */
        
        UnityEngine.Object[] loadedweapons = Resources.LoadAll("Weapons");
        foreach (UnityEngine.Object weapon in loadedweapons)
        {
            differentWeapons.Add((GameObject)weapon);
        }  


        foreach (GameObject obj in differentWeapons)
        {
            Debug.Log(obj);
        }
        createDropLoot(differentWeapons[0]);
      



    }

    /**
     * Using the droprates this function should be possible if or what item will be dropped.
     * Call the createDropLoot - method with the right game object to be dropped
     * 
     * TODO: Needs to be extended as soon as we have prefabs
     * and an idea about the droprates
     **/
    public void calculateDropRates() 
    {
        double dropValue = UnityEngine.Random.value;

        if ( dropValue <= DropChance)
        {
            createDropLoot(test);
        }

     }

    /**
     * This method will return the current location the current enemy is standing at.
     * This will guarantee, the loot will be dropped at the same location the enemy has been killed.
     **/ 
    private Transform getSpawnLocation()
    {
        Transform spawnLocation = this.transform;
        return spawnLocation;
    }

    /**
     * Give different parameters for different prefabs to be created
     **/
    private void createDropLoot(GameObject prefab) 
    {
        Transform spawnLocation = getSpawnLocation();
        var itemToDrop = (GameObject)Instantiate(prefab, spawnLocation.position, spawnLocation.rotation);
    }

    /**
     * Create a copy of the weapon to be dropped
     **/
     private DropHandler createCopy(GameObject obj)
    {
        Type type = this.GetType();
        Component newComponent = obj.AddComponent(type);
        DropHandler drop = newComponent as DropHandler;
        drop.test = test;
        return drop;
    }
 }
