using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Interface;
using Assets.Scripts.Weapon_Inventary;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;


public class Inventary : MonoBehaviour
{
    public List<InventaryItem> Items;
    public Transform AttackSpawnPosition;
    public int index = 0;
    private int InventarySize = 4;
    private List<GameObject> takeableItemsAround = new List<GameObject>();

    public InventaryItem SelectedTakeableItemsAround;


    private int _selectedTakeableItemsAroundIndex = -1;
    private int _defaultWeaponAmount = 1;
    private float _lastIndexChangeTime = 0;
    private float _lastTakeUpTime = 0;

    private float changeTimeThreshold = 0.2f;
    public List<GameObject> TakeableItemsAround { get { return takeableItemsAround; } }


    public void Start()
    {
        InventarySize = Items.Count;
    }
    public int SelectedTakeableItemsAroundIndex
    {
        get { return _selectedTakeableItemsAroundIndex; }
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

        do
        {
            if (index == 0)
            {
                index = Items.Count - 1;
            }
            else
            {
                index--;
            }
        } while (Items[index] == null);



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

        do
        {
            if (index == Items.Count - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }

        } while (Items[index] == null);
    }

    public void SelectNextTakeableItem()
    {
        if (_selectedTakeableItemsAroundIndex == -1)
        {
            return;
        }
        _selectedTakeableItemsAroundIndex++;
        if (TakeableItemsAround.Count >= _selectedTakeableItemsAroundIndex)
        {
            _selectedTakeableItemsAroundIndex = 0;
        }
        SelectedTakeableItemsAround = TakeableItemsAround[_selectedTakeableItemsAroundIndex].GetComponent<InventaryItem>();
    }

    public void SelectPreviousTakeableItem()
    {

        if (_selectedTakeableItemsAroundIndex == -1)
        {
            return;
        }
        _selectedTakeableItemsAroundIndex--;
        if (0 > _selectedTakeableItemsAroundIndex)
        {
            _selectedTakeableItemsAroundIndex = TakeableItemsAround.Count - 1;
        }
        SelectedTakeableItemsAround = TakeableItemsAround[_selectedTakeableItemsAroundIndex].GetComponent<InventaryItem>(); ;

    }

    public void Use()
    {
        InventaryItem inventaryItem = Items[index];
        if (inventaryItem != null)
        {

            inventaryItem.Use();
            if (inventaryItem.UseAbleAmount == 0)
            {
                Items[index] = null;
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

        if (SelectedTakeableItemsAround != null)
        {
            int itemCount = CountItems();
            if (InventarySize <= itemCount)
            {

                if (index < _defaultWeaponAmount)
                {
                    return;
                }
                InventaryItem inventaryItem = SelectedTakeableItemsAround.CreateCopy(gameObject);

                InventaryItem itemToDrop = Items[index];
                Items.Remove(itemToDrop);
                OnItemAdded(inventaryItem);
                Items.Insert(index, inventaryItem);
                itemToDrop.Drop(transform);
                Destroy(itemToDrop);

            }
            else
            {

                InventaryItem inventaryItem = SelectedTakeableItemsAround.CreateCopy(gameObject);
                OnItemAdded(inventaryItem);

                AddItem(inventaryItem);

            }
            takeableItemsAround.Remove(takeableItemsAround[SelectedTakeableItemsAroundIndex]);
            SelectedTakeableItemsAround.OnBeeinTakenUp();
            if (takeableItemsAround.Count == 0)
            {
                _selectedTakeableItemsAroundIndex = -1;
                SelectedTakeableItemsAround = null;
            }
            else
            {
                _selectedTakeableItemsAroundIndex = 0;
                SelectedTakeableItemsAround = takeableItemsAround[_selectedTakeableItemsAroundIndex].GetComponent<InventaryItemHolder>().InventaryItem;
            }
        }
    }

    protected void OnItemAdded(InventaryItem inventaryItem)
    {
        if (inventaryItem is IWeapon)
        {
            IWeapon weapon = inventaryItem as IWeapon;
            weapon.SetBulletSpawnPosition(AttackSpawnPosition);
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

    private void AddItem(InventaryItem item)
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
        Boolean contains = gameObject.CompareTag(Constants.InventarItem);
        if (contains)
        {
            InventaryItem inventaryItem = other.GetComponent<InventaryItemHolder>().InventaryItem;
            if (!TakeableItemsAround.Contains(gameObject))
            {

                TakeableItemsAround.Add(gameObject);
                if (_selectedTakeableItemsAroundIndex == -1)
                {
                    _selectedTakeableItemsAroundIndex = 0;
                    SelectedTakeableItemsAround = inventaryItem;
                }
            }
        }

    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.InventarItem))
        {
            var gameObject = other.gameObject;
            InventaryItem inventaryItem = other.gameObject.GetComponent<InventaryItem>();
            bool contains = TakeableItemsAround.Contains(gameObject);
            if (contains)
            {
                int indexOfNotAnymoreAvailable = TakeableItemsAround.IndexOf(gameObject);
                TakeableItemsAround.Remove(gameObject);

                if (SelectedTakeableItemsAround == inventaryItem)
                {
                    if (TakeableItemsAround.Count == 0)
                    {
                        SelectedTakeableItemsAround = null;
                        _selectedTakeableItemsAroundIndex = -1;
                    }
                    else if (_selectedTakeableItemsAroundIndex == 0)
                    {
                        SelectedTakeableItemsAround = TakeableItemsAround[0].GetComponent<InventaryItem>();
                    }
                    else
                    {
                        _selectedTakeableItemsAroundIndex--;
                        SelectedTakeableItemsAround = TakeableItemsAround[_selectedTakeableItemsAroundIndex].GetComponent<InventaryItem>();
                    }

                }
                else
                {
                    if (indexOfNotAnymoreAvailable < _selectedTakeableItemsAroundIndex)
                    {
                        _selectedTakeableItemsAroundIndex--;
                    }

                }

            }
        }

    }


}
