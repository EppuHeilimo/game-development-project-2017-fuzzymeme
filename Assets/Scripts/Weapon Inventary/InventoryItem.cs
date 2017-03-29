using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Weapon_Inventary;
using UnityEngine;

namespace Assets.Scripts.Interface
{
    public abstract class InventoryItem : MonoBehaviour
    {


        public GameObject PickUpPrefab;
        public String InventaryItemName;


        public void Start()
        {

            gameObject.GetComponent<MeshFilter>().sharedMesh = PickUpPrefab.GetComponent<MeshFilter>().sharedMesh;
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = PickUpPrefab.GetComponent<MeshRenderer>().sharedMaterial;

        }


        public virtual double ReloadPercentage
        {
            get { return 1; }
        }

        public virtual String ItemDescription
        {
            get
            {
                return InventaryItemName;
                
            }
        }

        public virtual int UseAbleAmount
        {
            get { return -1; }
        }

        public abstract void Use();


        public virtual void Drop()
        {

            //GameObject pickupItemObject = new GameObject();
            //pickupItemObject.transform.position = new Vector3(transform.position.x,0.5f,transform.position.z);
            //pickupItemObject.transform.rotation = transform.rotation;
            ////pickupItemObject.transform.localScale=new Vector3(0.5f,0.5f,0.5f);

            //InventoryItemHolder itemHolder = pickupItemObject.AddComponent<InventoryItemHolder>();
            //InventoryItem inventoryItem = CreateCopy(pickupItemObject);
            //itemHolder.SetInventoryItem(inventoryItem);


            DropHelper.DropItem(GetType(),transform, item =>
            {
                item.InventaryItemName = InventaryItemName;
                item.PickUpPrefab = PickUpPrefab;
                item.InventaryItemName = InventaryItemName;
                item.PickUpPrefab = PickUpPrefab;
                OnCreateCopy(item);
            });

           
        }


        public virtual void OnBeeinTakenUp()
        {
            Destroy(gameObject, 0);
        }

        public virtual void OnBeingSelected()
        {
            
        }

        public InventoryItem CreateCopy(GameObject newGameObject)
        {
            Type type = this.GetType();
            Component addComponent = newGameObject.AddComponent(type);
            InventoryItem inventaryItem = addComponent as InventoryItem;
            inventaryItem.PickUpPrefab = PickUpPrefab;
            inventaryItem.InventaryItemName = InventaryItemName;
            OnCreateCopy(inventaryItem);
            return inventaryItem;

        }

        protected abstract void OnCreateCopy(InventoryItem addComponent);

    }
}
