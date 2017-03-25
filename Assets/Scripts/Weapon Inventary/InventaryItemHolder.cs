using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interface;
using Assets.Scripts.Weapon_Inventary;
using UnityEngine;

namespace Assets.Scripts
{
    public class InventaryItemHolder : MonoBehaviour
    {

        public InventaryItem InventaryItem;


        public void Start()
        {
            gameObject.tag = Constants.InventarItem;

        }



    }
}
