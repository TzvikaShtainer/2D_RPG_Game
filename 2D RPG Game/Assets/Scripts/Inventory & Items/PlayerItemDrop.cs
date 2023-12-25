using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's Drop")]
    [SerializeField] private float chanceToLoseItems;

    public override void GenerateDrop()
    {
        List<InventoryItem> itemToUnequip = new List<InventoryItem>();

        foreach (InventoryItem item in Inventory.instance.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= chanceToLoseItems)
            {
                DropItem(item.data);
                itemToUnequip.Add(item);
            }
        }
        
        foreach (InventoryItem item in Inventory.instance.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLoseItems)
            {
                DropItem(item.data);
                itemToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemToUnequip.Count; i++)
        {
            Inventory.instance.UnEquipItem(itemToUnequip[i].data as ItemData_Equipment);
        }
    }
}
