using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Weapon_Inventary;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interface
{
    public abstract class InventoryItem : MonoBehaviour
    {


        public GameObject PickUpPrefab;

        public String InventaryItemName;

        public Sprite InventarSprite;

        public virtual void Start()
        {
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

        public void Drop()
        {
            Drop(transform);
        }

        public virtual void Drop(Transform transform)
        {

            DropHelper.DropItem(GetType(),transform, item =>
            {
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
            inventaryItem.InventarSprite = InventarSprite;
            inventaryItem.InventaryItemName = InventaryItemName;
            OnCreateCopy(inventaryItem);
            return inventaryItem;

        }

        protected abstract void OnCreateCopy(InventoryItem addComponent);

    }
}
