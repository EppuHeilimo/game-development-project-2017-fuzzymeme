using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Interface
{
    public abstract class InventoryItem : MonoBehaviour
    {


        public GameObject takeablePrefab;
        public String InventaryItemName;




        public virtual int UseAbleAmount
        {
            get { return -1; }
        }

        public abstract void Use();


        public virtual void Drop(Transform transform)
        {
            var takeableItem = (GameObject)Instantiate(
            takeablePrefab,
            transform.position,
            transform.rotation);
            InventoryItemHolder inventaryItemHolder = takeableItem.GetComponent<InventoryItemHolder>();
            inventaryItemHolder.InventaryItem = CreateCopy(takeableItem);
        }


        public virtual void OnBeeinTakenUp()
        {
            Destroy(gameObject, 0);
        }


        public InventoryItem CreateCopy(GameObject newGameObject)
        {
            Type type = this.GetType();
            Component addComponent = newGameObject.AddComponent(type);
            InventoryItem inventaryItem = addComponent as InventoryItem;
            inventaryItem.takeablePrefab = takeablePrefab;
            OnCreateCopy(inventaryItem);
            return inventaryItem;

        }

        protected abstract void OnCreateCopy(InventoryItem addComponent);

    }
}
