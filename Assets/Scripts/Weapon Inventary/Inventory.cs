using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Interface;
using Assets.Scripts.Weapon_Inventary;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public List<InventoryItem> Items;
    public int Index { get; set; } 
    private int InventarySize = 4;
    private List<GameObject> pickUpsAround = new List<GameObject>();
    public InventoryItem SelectedPickUpAround { get; set; }
    public List<GameObject> PickUpsAround { get { return pickUpsAround; } }


    private int _selectedPickUpArroundIndex = -1;
    private int _defaultWeaponAmount = 1;
    private float _lastIndexChangeTime = 0;
    private float _lastTakeUpTime = 0;

    private float changeTimeThreshold = 0.2f;


    public void Start()
    {
        Index = 0;
        InventarySize = Items.Count;
    }
    public int SelectedPickUpArroundIndex
    {
        get { return _selectedPickUpArroundIndex; }
    }

    public void Previous()
    {
        float time = Time.time;
        float diff = time - _lastIndexChangeTime;
        if (diff < changeTimeThreshold)
        {
            return;
        }
        _lastIndexChangeTime = time;
        int nextIndex = Index;
        do
        {
            if (nextIndex == 0)
            {
                nextIndex = Items.Count - 1;

            }
            else
            {
                nextIndex = nextIndex - 1;
            }
        } while (Items[nextIndex] == null);

        ChangeIndex(nextIndex);


    }

    public void Next()
    {
        float time = Time.time;
        float diff = time - _lastIndexChangeTime;
        if (diff < changeTimeThreshold)
        {
            return;
        }
        _lastIndexChangeTime = time;
        int newIndex = Index;
        do
        {
            if (newIndex == Items.Count - 1)
            {

                newIndex = 0;
            }
            else
            {

                newIndex= newIndex + 1;
            }

        } while (Items[newIndex] == null);
        ChangeIndex(newIndex);
    }

    private void ChangeIndex(int newIndex)
    {
        Index = newIndex;
        Items[Index].OnBeingSelected();
    }

    public void SelectNextTakeableItem()
    {
        if (_selectedPickUpArroundIndex == -1)
        {
            return;
        }
        _selectedPickUpArroundIndex++;
        if (PickUpsAround.Count >= _selectedPickUpArroundIndex)
        {
            _selectedPickUpArroundIndex = 0;
        }
        SelectedPickUpAround = PickUpsAround[_selectedPickUpArroundIndex].GetComponent<InventoryItem>();
    }

    public void SelectPreviousTakeableItem()
    {

        if (_selectedPickUpArroundIndex == -1)
        {
            return;
        }
        _selectedPickUpArroundIndex--;
        if (0 > _selectedPickUpArroundIndex)
        {
            _selectedPickUpArroundIndex = PickUpsAround.Count - 1;
        }
        SelectedPickUpAround = PickUpsAround[_selectedPickUpArroundIndex].GetComponent<InventoryItem>(); ;

    }

    public void Use()
    {
        InventoryItem inventaryItem = Items[Index];
        if (inventaryItem != null)
        {

            inventaryItem.Use();
            if (inventaryItem.UseAbleAmount == 0)
            {
                Items[Index] = null;
                Destroy(inventaryItem);
                Previous();
            }
        }
    }

    public void TakeUp()
    {
        float time = Time.time;
        float diff = time - _lastTakeUpTime;
        if (diff < changeTimeThreshold)
        {
            return;
        }
        _lastTakeUpTime = time;

        if (SelectedPickUpAround != null)
        {
            int itemCount = CountItems();
            if (InventarySize <= itemCount)
            {

                if (Index < _defaultWeaponAmount)
                {
                    return;
                }
                InventoryItem inventaryItem = SelectedPickUpAround.CreateCopy(gameObject);

                InventoryItem itemToDrop = Items[Index];
                Items.Remove(itemToDrop);
                OnItemAdded(inventaryItem);
                Items.Insert(Index, inventaryItem);
                itemToDrop.Drop();
                Destroy(itemToDrop);

            }
            else
            {

                InventoryItem inventaryItem = SelectedPickUpAround.CreateCopy(gameObject);
                OnItemAdded(inventaryItem);

                AddItem(inventaryItem);

            }
            pickUpsAround.Remove(pickUpsAround[SelectedPickUpArroundIndex]);
            SelectedPickUpAround.OnBeeinTakenUp();
            if (pickUpsAround.Count == 0)
            {
                _selectedPickUpArroundIndex = -1;
                SelectedPickUpAround = null;
            }
            else
            {
                _selectedPickUpArroundIndex = 0;
                SelectedPickUpAround = pickUpsAround[_selectedPickUpArroundIndex].GetComponent<InventoryItemHolder>().InventoryItem;
            }
        }
    }

    protected void OnItemAdded(InventoryItem inventaryItem)
    {
        if (inventaryItem is IWeapon)
        {
            IWeapon weapon = inventaryItem as IWeapon;
        }
    }

    private int CountItems()
    {
        int i = 0;
        foreach (var inventaryItem in Items)
        {
            if (inventaryItem != null)
            {
                i++;
            }
        }
        return i;
    }

    private void AddItem(InventoryItem item)
    {
        int i = 0;
        for (i = 0; i < Items.Count; i++)
        {
            if (Items[i] == null)
            {
                break;
            }
        }
        Items[i] = item;
    }


    void OnTriggerEnter(Collider other)
    {
        var gameObject = other.gameObject;
        Boolean contains = gameObject.CompareTag(Constants.InventoryItem);
        if (contains)
        {
            InventoryItem inventaryItem = other.GetComponent<InventoryItemHolder>().InventoryItem;
            if (!PickUpsAround.Contains(gameObject))
            {

                PickUpsAround.Add(gameObject);
                if (_selectedPickUpArroundIndex == -1)
                {
                    _selectedPickUpArroundIndex = 0;
                    SelectedPickUpAround = inventaryItem;
                }
            }
        }

    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.InventoryItem))
        {
            var gameObject = other.gameObject;
            InventoryItem inventaryItem = other.gameObject.GetComponent<InventoryItem>();
            bool contains = PickUpsAround.Contains(gameObject);
            if (contains)
            {
                int indexOfNotAnymoreAvailable = PickUpsAround.IndexOf(gameObject);
                PickUpsAround.Remove(gameObject);

                if (SelectedPickUpAround == inventaryItem)
                {
                    if (PickUpsAround.Count == 0)
                    {
                        SelectedPickUpAround = null;
                        _selectedPickUpArroundIndex = -1;
                    }
                    else if (_selectedPickUpArroundIndex == 0)
                    {
                        SelectedPickUpAround = PickUpsAround[0].GetComponent<InventoryItem>();
                    }
                    else
                    {
                        _selectedPickUpArroundIndex--;
                        SelectedPickUpAround = PickUpsAround[_selectedPickUpArroundIndex].GetComponent<InventoryItem>();
                    }

                }
                else
                {
                    if (indexOfNotAnymoreAvailable < _selectedPickUpArroundIndex)
                    {
                        _selectedPickUpArroundIndex--;
                    }

                }

            }
        }

    }


}
