using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interface;
using UnityEngine;

namespace Assets.Scripts.Weapon_Inventary
{
    public class DropHelper
    {




        public static void DropItem<T>(Transform spawnLocation, Action<T> fillItemAction) where T:  InventoryItem, new()
        {
            Type type = typeof(T);
            DropItem(type,spawnLocation,item => fillItemAction(item as T));

        }

        public static void DropItem(Type itemType, Transform spawnLocation, Action<InventoryItem> fillItemAction)
        {
            GameObject pickupItemObject = new GameObject();
            pickupItemObject.transform.position = new Vector3(spawnLocation.position.x, 0.5f, spawnLocation.position.z);
            pickupItemObject.transform.rotation = spawnLocation.rotation;

            InventoryItemHolder itemHolder = pickupItemObject.AddComponent<InventoryItemHolder>();
            Component inventoryItem = pickupItemObject.AddComponent(itemType);
            InventoryItem item = inventoryItem as InventoryItem;
            fillItemAction(item);
            itemHolder.SetInventoryItem(item);
        }


      


    }

}
