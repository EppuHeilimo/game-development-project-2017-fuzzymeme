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
            GameObject look;
            Transform find = gameObject.transform.Find("Look");
            if (find == null)
            {
                look = new GameObject("Look");
                look.transform.parent = transform;
            }
            else
            {
                look = find.gameObject;
            }
            if (look.GetComponent<MeshFilter>() == null)
            {
                look.AddComponent<MeshFilter>();

            }
            if (look.GetComponent<MeshRenderer>() == null)
            {
                look.AddComponent<MeshRenderer>();
            }

            this.inventaryItem = inventoryItem;

            if (inventaryItem != null)
            {
                
                    GameObject pickUpPrefab = inventoryItem.PickUpPrefab;


                    Mesh sharedMesh = pickUpPrefab.GetComponent<MeshFilter>().sharedMesh;

                look.GetComponent<MeshFilter>().sharedMesh = sharedMesh;

                look.GetComponent<MeshRenderer>().material =
                        pickUpPrefab.GetComponent<MeshRenderer>().sharedMaterial;

                look.transform.localPosition = new Vector3(0f,1,0f);
                look.transform.localScale = pickUpPrefab.transform.localScale;
                look.transform.rotation = pickUpPrefab.transform.rotation;

            }
        }
    }
}
