/**
 * The DropHandler Class calculates the drop rates of different
 * weapons and afterwards, if necessary, creates the drop itself.
 * It will place the gameobject on the field
 **/
using Assets.Scripts.Weapon_Inventary;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

public class DropHandler : MonoBehaviour {

    private List<GameObject> WeaponPrefabs; // List of all the weapon prefabs 
    private List<GameObject> BulletPrefabs; // List of all the bullet prefabs
    
    public double DropChange = 1;
    private static bool prefabsImported = false;
    private static List<InventoryItem> inventoryItems = new List<InventoryItem>();
    IZeroLifePointNotify zeroPointnotifier;


    public void OnZeroLifePoints(object sender, EventArgs e)
    {


       double randomValue = UnityEngine.Random.value;
       if(randomValue <= DropChange)
       {
           //int ProbabilitySum=0;
            //foreach(WeaponDefinition definition in definitions)
            //{
            //    ProbabilitySum += definition.ProbabilitySize;
            //}

            // randomValue = UnityEngine.Random.value;
            //int number = (int)Math.Round( ProbabilitySum* randomValue);

            //WeaponDefinition chosenDefinition=null;
            //foreach (WeaponDefinition definition in definitions)
            //{
            //  number =  number - definition.ProbabilitySize;
            //    if(number < 0)
            //    {
            //        chosenDefinition = definition;
            //        break;
            //    }
            //}
            //drop
           var count = inventoryItems.Count;

           int randomIndex = UnityEngine.Random.Range(0,count);
           InventoryItem inventoryItem = inventoryItems[randomIndex];

           inventoryItem.Drop(transform);
           //DropHelper.DropItem<Weapon>(transform, (weapon) => {

           //     weapon.InventaryItemName = chosenDefinition.InventaryItemName;
           //     weapon.Ammunition = chosenDefinition.Ammunition;
           //     weapon.PickUpPrefab = chosenDefinition.PickUpPrefab;
           //     weapon.ReloadTime = chosenDefinition.ReloadTime;
           //     weapon.BulletPrefab = chosenDefinition.BulletPrefab;
           //     weapon.holdingType = chosenDefinition.HoldingType;



           // });
       }
    }


    /**
* First function to be executed! 
**/
    private void Start()
    {
    


        getPrefabs();
        //createWeaponDefinitions();
        zeroPointnotifier = GetComponent<IZeroLifePointNotify>();
        zeroPointnotifier.ZeroLifePoints += OnZeroLifePoints;  

    }

    private void OnDestroy()
    {
        if(zeroPointnotifier != null)
        zeroPointnotifier.ZeroLifePoints -= OnZeroLifePoints;

    }



    /**
     * This method needs to be called from other scripts.
     * For example if enemy dies, you will need this method to set up the drops
     * Gets the list of all weapon prefabs used in the project.
     **/
    private  void getPrefabs()
    {
        if (prefabsImported)
        {
            return;
        }
        prefabsImported = true;

        UnityEngine.Object[] allWeapons = Resources.LoadAll("Dropables/");


        foreach (GameObject allWeapon in allWeapons)
        {
            InventoryItem weapon = allWeapon.GetComponent<InventoryItem>();
            inventoryItems.Add(weapon);
        }


        //WeaponPrefabs = new List<GameObject>();
        //BulletPrefabs = new List<GameObject>();
        

        //UnityEngine.Object[] loadedweapons = Resources.LoadAll("Weapons");
        //foreach (UnityEngine.Object weapon in loadedweapons)
        //{
        //    WeaponPrefabs.Add((GameObject)weapon);
        //}


        //UnityEngine.Object[] loadbullets = Resources.LoadAll("Bullets");
        //foreach (UnityEngine.Object bullet in loadbullets)
        //{
        //    BulletPrefabs.Add((GameObject)bullet);
        //}
        
    }


    ///**
    // * Create the definitions of the used weapons
    //**/
    //private void createWeaponDefinitions()
    //{
    //    WeaponDefinition temp = new WeaponDefinition();

    //    // GUN

    //    temp.Ammunition = 10;
    //    temp.BulletPrefab = BulletPrefabs[1];
    //    temp.HoldingType = PlayerAnimation.WeaponType.OneHanded;
    //    temp.InventaryItemName = "Gun";
    //    temp.ProbabilitySize = 30;
    //    temp.Description = "Simple one handed gun. Doesn't make that much damage, but at least its ranged.";
    //    temp.PickUpPrefab = WeaponPrefabs[0];
    //    temp.ReloadTime = 1;

    //    definitions.Add(temp);

    //    // Machinegun

    //    temp.Ammunition = 70;
    //    temp.BulletPrefab = BulletPrefabs[1];
    //    temp.HoldingType = PlayerAnimation.WeaponType.TwoHanded;
    //    temp.InventaryItemName = "Machinegun";
    //    temp.ProbabilitySize = 15;
    //    temp.Description = "Single bullets do not deal too much damage, but therefore you will fire even more with it!";
    //    temp.PickUpPrefab = WeaponPrefabs[1];
    //    temp.ReloadTime = 2;

    //    definitions.Add(temp);


    //    // Shotgun

    //    temp.Ammunition = 5;
    //    temp.BulletPrefab = BulletPrefabs[1];
    //    temp.HoldingType = PlayerAnimation.WeaponType.TwoHanded;
    //    temp.InventaryItemName = "Shotgun";
    //    temp.ProbabilitySize = 5;
    //    temp.Description = "Blow those motherfuckers up";
    //    temp.PickUpPrefab = WeaponPrefabs[2];
    //    temp.ReloadTime = 5;

    //    definitions.Add(temp);
    //}


    private class WeaponDefinition
    {
        
        public int ProbabilitySize { get; set; }

        public String InventaryItemName { get; set; }

        public String Description { get; set; }

        public int Ammunition { get; set; }

        public GameObject PickUpPrefab { get; set; }

        public int ReloadTime { get; set; }

        public GameObject BulletPrefab { get; set; }

        public PlayerAnimation.WeaponType HoldingType { get; set; }


    }
 }
