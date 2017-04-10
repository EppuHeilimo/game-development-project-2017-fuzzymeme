using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Interface;
using Assets.Scripts.Weapon_Inventary;
using UnityEngine;

public class HealingPotion : InventoryItem
{

    public double HealingPoints;
    private bool used = false;
    private Stats stats;

    public override int UseAbleAmount
    {
        get
        {
            
            return used?0:1;
        }
    }

    // Use this for initialization
    void Start ()
    {
        stats = GetComponent<Stats>();
    }

    public override void Use()
    {

        if (used)
        {
            return;
        }
        else
        {
            stats.Heal(HealingPoints);
        }
        used = true;


    }

    protected override void OnCreateCopy(InventoryItem addComponent)
    {
        HealingPotion healingPotion = (HealingPotion) addComponent;
        healingPotion.used = used;
    }

    // Update is called once per frame
	void Update () {
		
	}
}
