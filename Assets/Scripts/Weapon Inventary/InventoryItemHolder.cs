using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interface;
using Assets.Scripts.Weapon_Inventary;
using UnityEngine;

namespace Assets.Scripts
{
    public class InventoryItemHolder : MonoBehaviour
    {

        public InventoryItem InventaryItem;


        public void Start()
        {
            gameObject.tag = Constants.InventoryItem;

        }



    }
}
