using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour //one inventory that contains a list of items and dict for searching 
{
    public static Inventory instance;

    [Header("Equipped Items")]
    public List<InventoryItem> equipmentList;
    public Dictionary<ItemData, InventoryItem> equipmentDictionary;
    
    
    [Header("Inventory")] 
    public List<InventoryItem> inventoryList; 
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    
    [Header("Inventory UI")] 
    [SerializeField] private Transform inventorySlotParent;
    private UI_ItemSlot[] inventoryItemSlot; 
    
    [Header("Stash")] 
    public List<InventoryItem> stashList;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Stash UI")] 
    [SerializeField] private Transform stashSlotParent;
    private UI_ItemSlot[] stashItemSlot; 
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        equipmentList = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData, InventoryItem>();
        
        inventoryList = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        
        stashList = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryList.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventoryList[i]);
        }
        
        for (int i = 0; i < stashList.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stashList[i]);
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
            AddItemTo(_item, inventoryList, inventoryDictionary);
        else if (_item.itemType == ItemType.Material)
            AddItemTo(_item, stashList, stashDictionary);
        
        UpdateSlotUI();
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryList.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }
    
    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stashList.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddItemTo(ItemData _item, List<InventoryItem> addToList, Dictionary<ItemData, InventoryItem> addToDict)
    {
        if (addToDict.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            addToList.Add(newItem);
            addToDict.Add(_item, newItem);
        }
    }

    public void EquipItem(ItemData _item)
    {
        InventoryItem newItem = new InventoryItem(_item);
        
        equipmentList.Add(newItem);
        equipmentDictionary.Add(_item, newItem);
        
        RemoveItem(_item);
    }
    public void RemoveItem(ItemData itemToRemove)
    {
        if (inventoryDictionary.TryGetValue(itemToRemove, out InventoryItem inventoryValue))
        {
            if (inventoryValue.stackSize <= 1)
            {
                inventoryList.Remove(inventoryValue);
                inventoryDictionary.Remove(itemToRemove);
            }
            else
                inventoryValue.RemoveStack();
        }
        
        if (inventoryDictionary.TryGetValue(itemToRemove, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stashList.Remove(stashValue);
                stashDictionary.Remove(itemToRemove);
            }
            else
                stashValue.RemoveStack();
        }
        
        UpdateSlotUI();
    }
}
