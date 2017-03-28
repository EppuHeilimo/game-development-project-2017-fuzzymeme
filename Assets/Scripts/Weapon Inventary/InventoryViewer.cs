using System;
using Assets.Scripts.Interface;
using Boo.Lang.Runtime;
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



        private Image item1Image;
        private Image item2Image;

        private Image item3Image;
        private Image item4Image;
        int lastIndex = -1;
        private Sprite notSelectedSprite;
        private Sprite SelectedSprite;
        private Text selectedWeaponText;
        private Image refillImage;
        private float height;


        public void Start()
        {
            inventory = gameObject.GetComponent<Inventory>();

            LoadComponents();

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

        //public void Update()
        //{

        //    if (pickUpText != null)
        //    {
        //        bool notNullInventary = _inventoryText != null;
        //        bool selectedItem = inventory.SelectedPickUpAround != null;
        //        bool dataToShow = selectedItem && notNullInventary;
        //        if (dataToShow)
        //        {



        //            pickUpText.text = "[" + (inventory.SelectedPickUpArroundIndex + 1) + ":" +
        //                        inventory.PickUpsAround.Count + "]  " + inventory.SelectedPickUpAround.InventaryItemName;



        //            showsText = true;
        //        }
        //        else if (showsText)
        //        {
        //            pickUpText.text = "";
        //        }


        //    }


        //    if (_inventoryText != null)
        //    {          
        //        String fullText = "";
        //        for (int i = 0; i < inventory.Items.Count; i++)
        //        {
        //            fullText += getItem(i);
        //        }


        //        _inventoryText.text = fullText;

        //    }


        //}


      

        private void LoadComponents()
        {
            selectedWeaponText = GameObject.Find("Inventory/Bottom/Text").GetComponent<Text>();
            item1Image = GameObject.Find("Inventory/LeftSide/Item1").GetComponent<Image>();
            item2Image = GameObject.Find("Inventory/LeftSide/Item2").GetComponent<Image>();
            item3Image = GameObject.Find("Inventory/LeftSide/Item3").GetComponent<Image>();
            item4Image = GameObject.Find("Inventory/LeftSide/Item4").GetComponent<Image>();
            Resources.Load<Sprite>("awesome");
            notSelectedSprite = Resources.Load<Sprite>("item_border");
            SelectedSprite = Resources.Load<Sprite>("selected_item_border");

            refillImage = GameObject.Find("Inventory/LeftSide/Refill").GetComponent<Image>();
            height = refillImage.rectTransform.rect.height;
        }


        public void Update()
        {

            UpdateSelectedItem();





        }

        public void FixedUpdate()
        {
            UpdateReloadBar();
        }

        private bool is100 = false;
        private void UpdateReloadBar()
        {
          
            InventoryItem inventoryItem = inventory.Items[inventory.Index];
            double percentage = inventoryItem.ReloadPercentage ;
            if (percentage == 1 )
            {
                if (is100)
                {
                    return;
                }
                else
                {
                    is100 = true;
                }
               
            }
            else
            {
                is100 = false;
            }
            float heightint =(float)( height* percentage);
            refillImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, heightint);
        }

        private void UpdateSelectedItem()
        {
            int index = inventory.Index;
            if (index != lastIndex)
            {
                item1Image.sprite = notSelectedSprite;
                item2Image.sprite = notSelectedSprite;
                item3Image.sprite = notSelectedSprite;
                item4Image.sprite = notSelectedSprite;

                if (index == 0)
                {
                    item1Image.sprite = SelectedSprite;

                }
                else if (index == 1)
                {
                    item2Image.sprite = SelectedSprite;

                }
                else if (index == 2)
                {
                    item3Image.sprite = SelectedSprite;

                }
                else if (index == 3)
                {
                    item4Image.sprite = SelectedSprite;

                }
                else
                {
                    throw new RuntimeException("Unsupported index");
                }

                InventoryItem inventoryItem = inventory.Items[index];
                selectedWeaponText.text = inventoryItem.InventaryItemName + " | A:" + " | " + " | D:";

                lastIndex = index;
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
