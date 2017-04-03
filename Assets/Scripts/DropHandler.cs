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

    //public double DropChance = 1.0f; // Has to be a value between 0.0 and 1.0
    private List<GameObject> differentWeapons; // List of all the weapon prefabs 
    private GameObject test;  // test value


    /**
     * Droprates for different weapons
     **/
    private readonly double droprate1 = 0.3f;
    private readonly double droprate2 = 0.3f;
    private readonly double droprate3 = 0.8f;

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

        calculateDropRates();
      



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
        if (doesItDrop(droprate1) == true)
        {
            createDropLoot(differentWeapons[0]); 
        }
        else
        {
            if (doesItDrop(droprate2) == true)
            {
                createDropLoot(differentWeapons[1]);
            }
            else
            {
                if (doesItDrop(droprate3) == true)
                {
                    createDropLoot(differentWeapons[2]);
                }
            }
        }
        

    }

    /**
     * This method validates if the item according to the given rates
     * will be dropped
     **/
    private bool doesItDrop(double dropRate)
    {
        double rateIndex = UnityEngine.Random.value;
        if (dropRate > rateIndex)
        {
            return true;
        }
        else
        {
            return false;
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
 }
