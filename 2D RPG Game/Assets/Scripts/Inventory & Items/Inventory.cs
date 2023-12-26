using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour 
{
    public static Inventory instance;

    public List<ItemData> startingItems = new List<ItemData>();

    [Header("Equipment")]
    public List<InventoryItem> equipmentList;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;
    
    [Header("Inventory")] 
    public List<InventoryItem> inventoryList; 
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    
    [Header("Stash")] 
    public List<InventoryItem> stashList;
    public Dictionary<ItemData, InventoryItem> stashDictionary;
    
    
    
    [Header("Inventory UI")] 
    [SerializeField] private Transform inventorySlotParent;
    private UI_ItemSlot[] inventoryItemSlot; 

    [Header("Stash UI")] 
    [SerializeField] private Transform stashSlotParent;
    private UI_ItemSlot[] stashItemSlot;

    [Header("Equipment UI")]
    [SerializeField] private Transform equipmentSlotParent;
    private UI_EquipmentSlot[] equipmentItemSlot;

    [Header("Items Cooldown")]
    private float lastTimeUsedFlask;
    
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
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();
        equipmentItemSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        
        inventoryList = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        
        stashList = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentItemSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentItemSlot[i].slotType)
                    equipmentItemSlot[i].UpdateSlot(item.Value);
            }
        }
        
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }
        
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }
        
        
        
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
        {
            AddToInventory(_item);
            //AddItemTo(_item, inventoryList, inventoryDictionary);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
            //AddItemTo(_item, stashList, stashDictionary);
        }
        
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
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;
        
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;
        }

        //Debug.Log(itemToRemove);

        if (oldEquipment != null)
        {
            UnEquipItem(oldEquipment);
            AddItem(oldEquipment);
        }
        
        
        equipmentList.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        
        newEquipment.AddModifiers();
        
        RemoveItem(_item);
        
        UpdateSlotUI();
    }

    public void UnEquipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipmentList.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            
            itemToRemove.RemoveModifiers();
        }
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
        
        if (stashDictionary.TryGetValue(itemToRemove, out InventoryItem stashValue))
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

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        
        for (int i = 0; i < requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(requiredMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < requiredMaterials[i].stackSize)
                {
                    Debug.Log("not enough");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("not enough");
                return false;
            }
        }
        
        //materialsToRemove.Clear();

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        
        AddItem(_itemToCraft);
        Debug.Log("here your item " + _itemToCraft.name);
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipmentList;
    
    public List<InventoryItem> GetStashList() => stashList;

    public ItemData_Equipment GetEquippedItem(EquipmentType _type)
    {
        ItemData_Equipment equippedItem = null;
        
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
                equippedItem = item.Key;
        }
        
        return equippedItem;
    }

    public void UseFlask()
    {
        
        ItemData_Equipment currentFlask = GetEquippedItem(EquipmentType.Flask);

        if(currentFlask == null)
            return;
        
        bool canUseFlask = Time.time > lastTimeUsedFlask + currentFlask.itemCooldown;

        if (canUseFlask)
        {
            currentFlask.ExecuteItemEffect(null);
            lastTimeUsedFlask = Time.time;
            
        }
        else
        {
            Debug.Log("flask on cooldwon");
        }
    }
}
