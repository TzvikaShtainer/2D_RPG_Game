using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour //one inventory that contains a list of items and dict for searching 
{
    public static Inventory instance;

    [Header("Inventory")] 
    public List<InventoryItem> inventory; 
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    
    [Header("Stash")] 
    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;


    [Header("Inventory UI")] 
    [SerializeField] private Transform inventorySlotParent;
    private UI_ItemSlot[] inventoryItemSlot; 
    
    
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
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        
        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }
        
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
            AddItem(_item, inventory, inventoryDictionary);
        else if (_item.itemType == ItemType.Material)
            AddItem(_item, stash, stashDictionary);
        
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
            inventory.Add(newItem);
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
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddItem(ItemData _item, List<InventoryItem> addToList, Dictionary<ItemData, InventoryItem> addToDict)
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
    public void RemoveItem(ItemData itemToRemove)
    {
        if (inventoryDictionary.TryGetValue(itemToRemove, out InventoryItem inventoryValue))
        {
            if (inventoryValue.stackSize <= 1)
            {
                inventory.Remove(inventoryValue);
                inventoryDictionary.Remove(itemToRemove);
            }
            else
                inventoryValue.RemoveStack();
        }
        
        if (inventoryDictionary.TryGetValue(itemToRemove, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(itemToRemove);
            }
            else
                stashValue.RemoveStack();
        }
        
        UpdateSlotUI();
    }
}
