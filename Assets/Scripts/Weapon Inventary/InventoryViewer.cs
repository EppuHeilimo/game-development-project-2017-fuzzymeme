using System;
using Assets.Scripts.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Weapon_Inventary
{
    public class InventoryViewer : MonoBehaviour
    {
        public Inventory Inventary;
        public Text Text;
        public Text InventaryText;
        private Boolean showsText = false;

        public void Update()
        {
            bool notNullInventary = Inventary != null;
            bool selectedItem = Inventary.SelectedTakeableItemsAround != null;
            bool dataToShow = selectedItem && notNullInventary;
            if (dataToShow)
            {



                Text.text = "[" + (Inventary.SelectedTakeableItemsAroundIndex + 1) + ":" +
                            Inventary.TakeableItemsAround.Count + "]  " + Inventary.SelectedTakeableItemsAround.InventaryItemName;



                showsText = true;
            }
            else if (showsText)
            {
                Text.text = "";
            }


            String fullText = "";
            for (int i = 0; i < Inventary.Items.Count; i++)
            {
                fullText += getItem(i);
            }


            InventaryText.text = fullText;









        }

        protected String getItem(int index)
        {
            String text = " [";
            if (index == Inventary.index)
            {
                text += "->";
            }
            else
            {
                text += "  ";

            }
            InventoryItem inventaryItem = Inventary.Items[index];
            if (inventaryItem == null)
            {
                text += "      ";
            }
            else
            {
                text += inventaryItem.InventaryItemName;
                text += "(" + inventaryItem.UseAbleAmount + ")";
            }


            if (index == Inventary.index)
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
