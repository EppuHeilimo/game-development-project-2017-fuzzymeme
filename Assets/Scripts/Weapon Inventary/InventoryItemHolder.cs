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

        private InventoryItem inventaryItem;

        public InventoryItem InventoryItem
        {
            get
            {
                return inventaryItem;
            }
        }

        public void Start()
        {
            InventoryItem inventoryItem = GetComponent<InventoryItem>();

         

            SetInventoryItem(inventoryItem);
            BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                 boxCollider = gameObject.AddComponent<BoxCollider>();
            }
            
            boxCollider.center = new Vector3(0,1,0);
            boxCollider.size = new Vector3(3,3,3);
            boxCollider.isTrigger = true;

            

            tag = Constants.InventoryItem;

        }

        

        public void SetInventoryItem(InventoryItem inventoryItem )
        {

            if (gameObject.GetComponent<MeshFilter>() == null)
            {
                gameObject.AddComponent<MeshFilter>();

            }
            if (gameObject.GetComponent<MeshRenderer>() == null)
            {
                gameObject.AddComponent<MeshRenderer>();
            }

            this.inventaryItem = inventoryItem;

            if (inventaryItem != null)
            {
                
                    GameObject pickUpPrefab = inventoryItem.PickUpPrefab;

                    

                    Mesh sharedMesh = pickUpPrefab.GetComponent<MeshFilter>().sharedMesh;
                    gameObject.GetComponent<MeshFilter>().sharedMesh = sharedMesh;

                    gameObject.GetComponent<MeshRenderer>().material =
                        pickUpPrefab.GetComponent<MeshRenderer>().sharedMaterial;
                    transform.localScale = pickUpPrefab.transform.localScale;
                    transform.rotation = pickUpPrefab.transform.rotation;

            }
        }
    }
}
