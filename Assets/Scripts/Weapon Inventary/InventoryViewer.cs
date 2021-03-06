﻿using System;
using System.Collections.Generic;
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


        private EqualChecker equalCheckeritem1 = new EqualChecker();
        private EqualChecker equalCheckeritem2 = new EqualChecker();
        private EqualChecker equalCheckeritem3 = new EqualChecker();
        private EqualChecker equalCheckeritem4 = new EqualChecker();


        private Image item1Image;
        private Image item2Image;

        private Image item3Image;
        private Image item4Image;

        private Image Icon1;
        private Image Icon2;
        private Image Icon3;
        private Image Icon4;



        private InventoryItem lastInventoryItem = null;
        private Sprite notSelectedSprite;
        private Sprite SelectedSprite;
        private Text selectedWeaponText;
        private Image refillImage;
        private float height;
        private Text DropItemsText;

        private Text item1Text;
        private Text item2Text;
        private Text item3Text;
        private Text item4Text;
        private int last1Amount = -2;
        private int last2Amount = -2;
        private int last3Amount = -2;
        private int last4Amount = -2;

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

        //   


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

            Icon1 = GameObject.Find("Inventory/LeftSide/Item1/Image").GetComponent<Image>();
            Icon2 = GameObject.Find("Inventory/LeftSide/Item2/Image").GetComponent<Image>();
            Icon3 = GameObject.Find("Inventory/LeftSide/Item3/Image").GetComponent<Image>();
            Icon4 = GameObject.Find("Inventory/LeftSide/Item4/Image").GetComponent<Image>();


            item1Text = GameObject.Find("Inventory/LeftSide/Item1/Text").GetComponent<Text>();

            item2Text = GameObject.Find("Inventory/LeftSide/Item2/Text").GetComponent<Text>();

            item3Text = GameObject.Find("Inventory/LeftSide/Item3/Text").GetComponent<Text>();

            item4Text = GameObject.Find("Inventory/LeftSide/Item4/Text").GetComponent<Text>();

            item1Text.text = "";
            item2Text.text = "";
            item3Text.text = "";
            item4Text.text = "";

            Resources.Load<Sprite>("awesome");
            notSelectedSprite = Resources.Load<Sprite>("item_border");
            SelectedSprite = Resources.Load<Sprite>("selected_item_border");

            refillImage = GameObject.Find("Inventory/LeftSide/Refill").GetComponent<Image>();
            height = refillImage.rectTransform.rect.height;

            DropItemsText = GameObject.Find("PickupAbleItemsBar/DropItemsAround").GetComponent<Text>();
        }


        public void Update()
        {

            UpdateSelectedItem();
            UpdateReloadBar();
            UpdateUseAmount();
            UpdateSelectableItems();
            UpdateInventoryIcon();
            if (pickUpText != null)
            {
                bool notNullInventary = pickUpText != null;
                bool selectedItem = inventory.SelectedPickUpAround != null;
                bool dataToShow = selectedItem && notNullInventary;
                if (dataToShow)
                {



                    pickUpText.text = "[" + (inventory.SelectedPickUpArroundIndex + 1) + ":" +
                                      inventory.PickUpsAround.Count + "]  " +
                                      inventory.SelectedPickUpAround.InventaryItemName;



                    showsText = true;
                }
                else if (showsText)
                {
                    pickUpText.text = "";
                }


            }


        }



        private bool is100 = false;

        private void UpdateReloadBar()
        {

            InventoryItem inventoryItem = inventory.Items[inventory.Index];
            double percentage = inventoryItem.ReloadPercentage;
            if (percentage == 1)
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
            float heightint = (float) (height*percentage);
            refillImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, heightint);
        }

        private int lastInventoryItem0Id = -1;
        private int lastInventoryItem1Id = -1;
        //private InventoryItem lastInventoryItem2 = null;
        private int lastInventoryItem2Id = -1;
        private InventoryItem lastInventoryItem3 = null;


        private void UpdateInventoryIcon()
        {
            InventoryItem item = inventory.Items[0];
            var isEqual = equalCheckeritem1.IsEqual(item  );
            if (!isEqual)
            {
                equalCheckeritem1.Current = item;
                ChangeIcon(item, Icon1);
            }

             item = inventory.Items[1];
            isEqual = equalCheckeritem2.IsEqual(item);
            if (!isEqual)
            {
                equalCheckeritem2.Current = item;
                ChangeIcon(item, Icon2);
            }


            item = inventory.Items[2];
            isEqual = equalCheckeritem3.IsEqual(item);
            if (!isEqual)
            {
                equalCheckeritem3.Current = item;
                ChangeIcon(item, Icon3);
            }

            item = inventory.Items[3];
            isEqual = equalCheckeritem4.IsEqual(item);
            if (!isEqual)
            {
                equalCheckeritem4.Current = item;
                ChangeIcon(item, Icon4);
            }
           
        }

        void ChangeIcon 
            (InventoryItem item, Image icon)
            {
                if (item == null)
                {
                    icon.sprite = null;
                }
                else
                {
                    icon.sprite = item.InventarSprite;
                    icon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 46.4f);
                    icon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 61.4f);
                }

            }

        private
            void UpdateUseAmount 
            ()
            {


                InventoryItem item = inventory.Items[0];
                int lastTemp = last1Amount;
                UpdateUseAmount(item, ref lastTemp);
                if (lastTemp != last1Amount)
                {
                    last1Amount = lastTemp;

                    ChangeText(item1Text, lastTemp);
                }



                item = inventory.Items[1];
                lastTemp = last2Amount;
                UpdateUseAmount(item, ref lastTemp);
                if (lastTemp != last2Amount)
                {
                    last2Amount = lastTemp;
                    ChangeText(item2Text, lastTemp);


                }

                item = inventory.Items[2];
                lastTemp = last3Amount;
                UpdateUseAmount(item, ref lastTemp);
                if (lastTemp != last3Amount)
                {
                    last3Amount = lastTemp;
                    ChangeText(item3Text, lastTemp);

                }

                item = inventory.Items[3];
                lastTemp = last4Amount;
                UpdateUseAmount(item, ref lastTemp);
                if (lastTemp != last4Amount)
                {
                    last4Amount = lastTemp;
                    ChangeText(item4Text, lastTemp);

                }

            }

        private
            void ChangeText 
            (Text textComponent, int i)
            {

                String text;
                if (i == -2)
                {
                    text = "";
                }
                else if (i == -1)
                {
                    text = "∞";
                    textComponent.fontSize = 30;
                }
                else
                {
                    text = i + "";
                    textComponent.fontSize = 15;

                }
                textComponent.text = text;
            }



        private
            void UpdateUseAmount 
            (InventoryItem item, ref int lastAmount)
            {
                if (item == null)
                {
                    if (lastAmount != -2)
                    {

                        lastAmount = -2;
                    }

                }
                else if (item.UseAbleAmount != lastAmount)
                {

                    lastAmount = item.UseAbleAmount;
                }
            }


        private
            void UpdateSelectedItem 
            ()
            {
                InventoryItem item = inventory.Items[inventory.Index];
                if (item != lastInventoryItem)
                {
                    item1Image.sprite = notSelectedSprite;
                    item2Image.sprite = notSelectedSprite;
                    item3Image.sprite = notSelectedSprite;
                    item4Image.sprite = notSelectedSprite;
                    int index = inventory.Index;
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

                    selectedWeaponText.text = item.ItemDescription;

                    lastInventoryItem = item;
                }
            }


        protected
            String getItem 
            (int
            index)
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

        protected void UpdateSelectableItems ()
            {
                List<GameObject> pickUpsAround = inventory.PickUpsAround;
                if (pickUpsAround.Count == 0)
                {
                    DropItemsText.text = "";
                }
                else
                {
                    int indexer = inventory.SelectedPickUpArroundIndex + 1;
                    string name = inventory.SelectedPickUpAround.InventaryItemName;
                    String str = name + "  [" + indexer + "|" + pickUpsAround.Count + "]";
                    DropItemsText.text = str;

                }


            }

        class EqualChecker
        {


            private InventoryItem _current;
            private bool isSetToNotNull = false;

            public InventoryItem Current
            {
                get { return _current; }
                set
                {
                    isSetToNotNull = value != null;
                    _current = value;
                }
            }


            public bool IsEqual(InventoryItem item)
            {

                int itemId = -1;
                if (item != null)
                {
                    itemId = item.GetInstanceID();
                }

                if (isSetToNotNull)
                {
                    int currentId = Current.GetInstanceID();


                    return currentId == itemId;
                }
                else
                {
                    return itemId == -1;
                }

            }
        }


    }



}
