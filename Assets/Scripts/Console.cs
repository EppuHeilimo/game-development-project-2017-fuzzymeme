using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Interface;
using Assets.Scripts.Weapon_Inventary;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Assets.Script;
using UnityEngine.AI;

public class Console : MonoBehaviour
{

    const int NormalHash = -2016234814;
    const int GodHash = 1175195924;
    const int DropSomethingHash = -262007715;
    const int HalfGodHash = -208539993;
    const int LaterHash = 2071488744;
    const int NotAvailableHash = -210791430;
    const int ShowMeTheBossHash = 649304737;
    const int OpenHash = -371437091;
    private static InputField consoleInput;
    private RectTransform rectTransform;




    private Weapon stick;
    private Weapon godsFist;

    private bool consoleVisible = false;
    private float lastPressedButtonTime = 0;
	// Use this for initialization
	void Start ()
	{

	    GameObject find = GameObject.Find("ConsoleInput");
        consoleInput=find.GetComponent<InputField>();
	    consoleInput.enabled = false;
	    rectTransform = find.GetComponent<RectTransform>();
	    rectTransform.localScale = new Vector3(0,0,0);
        consoleInput.onEndEdit.AddListener(AnalyseCommand);
	}

    private void AnalyseCommand(string arg0)
    {
        int hash = arg0.GetHashCode();

        if (GodHash.Equals(hash))
        {
            StartGodMode(true);
        }else if (NormalHash.Equals(hash))
        {
            StartGodMode(false);
        }else if (HalfGodHash.Equals(hash))
        {
            StartGodMode(false);
            StartHalfGodMode();
        }else if (DropSomethingHash.Equals(hash))
        {
            DropSomething();
        }
        consoleInput.text = "";
        if (LaterHash.Equals(hash))
        {
            consoleInput.text = "I will do it... later...somewhen...who knows...";
        }else if (NotAvailableHash.Equals(hash))
        {
            consoleInput.text = "That is not available. maybe eppu can help.";

        }
        if(OpenHash.Equals(hash))
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().OpenCurrentAreasEntries();
        }
        if(ShowMeTheBossHash.Equals(hash))
        {
            TeleportToBossRoom();
        }
    }

    private void TeleportToBossRoom()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>().Warp(GameObject.FindGameObjectWithTag("BossArea").transform.FindDeepChild("Entry 1").GetComponent<EntryPoint>().playerTeleportPoint.position);
    }

    private void DropSomething()
    {
        Object[] loadAll = Resources.LoadAll("Dropables/");

        int length = loadAll.Length;
        int index = UnityEngine.Random.Range(0, length);
        GameObject o = (GameObject) loadAll[index];

        GameObject player = GameObject.Find("Player");

        o.GetComponent<InventoryItem>().Drop(player.transform);
    }

    private void StartHalfGodMode()
    {
        GameObject player = GameObject.Find("Player");
        Stats stats = player.GetComponent<Stats>();
        
            stats.LifeEnergy = 500;
            stats.CurrentLifeEnergy = 250;
        
    }
    private void StartGodMode(bool flag)
    {
        GameObject player = GameObject.Find("Player");
        Stats stats = player.GetComponent<Stats>();
        if (flag)
        {
            stats.LifeEnergy = 99999;
            stats.CurrentLifeEnergy = 99999;
        }
        else
        {
            stats.LifeEnergy = 100;
            stats.CurrentLifeEnergy = 100;
        }
        Weapon[] weapons = player.GetComponents<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            if (weapon.InventaryItemName.Equals("Stick"))
            {
                stick = weapon;
            }else if (weapon.InventaryItemName.Equals("Gods Fist"))
            {
                this.godsFist = weapon;
            }

        }

        if (godsFist == null)
        {
            var godsFist1 = Resources.Load<Weapon>("Dropables/10_Gods Fist");
            godsFist = (Weapon)godsFist1.CreateCopy(player);
            godsFist.InitWeaponHolder();
        }
     

        InventoryItem inventoryItem;
        if (flag)
        {
            inventoryItem = godsFist;
        }
        else
        {
            inventoryItem = stick;
        }


        Inventory inventory = player.GetComponent<Inventory>();
        inventory.Items.Remove(inventory.Items[0]);
        inventory.Items.Insert(0, inventoryItem);
        inventory.ChangeIndex(0);


    }

    // Update is called once per frame
    void Update ()
	{

        bool keyUp = GetKeyDown(KeyCode.I);

        if (keyUp)
	    {
	        float diff = Time.time   - lastPressedButtonTime;
	        if (diff > 0.3)
	        {
	            if (consoleVisible)
	            {
	                consoleInput.enabled = false;
	                consoleVisible = false;
                    rectTransform.localScale=new Vector3(0,0,0);
	            }
	            else
	            {
                    consoleInput.enabled = true;
                    consoleVisible = true;
                    rectTransform.localScale = new Vector3(1,1,1);
	                consoleInput.ActivateInputField();

	            }
            }
	        lastPressedButtonTime = Time.time;

	    }
	}

    public static bool GetKeyDown(KeyCode keyCode)
    {
        if (consoleInput.isFocused)
        {
            return false;
        }
        else
        {
            return Input.GetKeyDown(keyCode);

        }
    }

    public static bool GetKeyUp(KeyCode keyCode)
    {
        if (consoleInput.isFocused)
        {
            return false;
        }
        else
        {
            return Input.GetKeyUp(keyCode);
        }
    }

    public static bool GetKey(KeyCode keyCode)
    {
        if (consoleInput.isFocused)
        {
            return false;
        }
        else
        {
            return Input.GetKey(keyCode);
        }
    }

    public static float GetAxisRaw(String axis) 
    {
        if (consoleInput.isFocused)
        {
            return 0.0f;
        }
        else
        {
            return Input.GetAxisRaw(axis);
        }
    }


}
