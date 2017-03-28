using System;
using Assets.Scripts.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Weapon_Inventary
{
    public class InventoryViewer : MonoBehaviour
    {
        private Inventory inventory;
        private Text pickUpText;
        private Text _inventoryText;
        private Boolean showsText = false;

        public void Start()
        {
            inventory = gameObject.GetComponent<Inventory>();

            GameObject o = GameObject.Find(Constants.InventaryText);
            if (o != null)
            {
                _inventoryText = o.GetComponent<Text>();
            }

            GameObject find = GameObject.Find(Constants.PickUpItems);
            if (find != null)
            {
                pickUpText = find.GetComponent<Text>();

            }
         
        }

        public void Update()
        {

            if (pickUpText != null)
            {
                bool notNullInventary = _inventoryText != null;
                bool selectedItem = inventory.SelectedPickUpAround != null;
                bool dataToShow = selectedItem && notNullInventary;
                if (dataToShow)
                {



                    pickUpText.text = "[" + (inventory.SelectedPickUpArroundIndex + 1) + ":" +
                                inventory.PickUpsAround.Count + "]  " + inventory.SelectedPickUpAround.InventaryItemName;



                    showsText = true;
                }
                else if (showsText)
                {
                    pickUpText.text = "";
                }


            }


            if (_inventoryText != null)
            {          
                String fullText = "";
                for (int i = 0; i < inventory.Items.Count; i++)
                {
                    fullText += getItem(i);
                }


                _inventoryText.text = fullText;

            }






        }

        protected String getItem(int index)
        {
            String text = " [";
            if (index == inventory.Index)
            {
                text += "->";
            }
            else
            {
                text += "  ";

            }
            InventoryItem inventaryItem = inventory.Items[index];
            if (inventaryItem == null)
            {
                text += "      ";
            }
            else
            {
                text += inventaryItem.InventaryItemName;
                text += "(" + inventaryItem.UseAbleAmount + ")";
            }


            if (index == inventory.Index)
            {
                text += "<-";
            }
            else
            {
                text += "  ";

            }
            text += "] ";
            return text;
        }

    }
}
