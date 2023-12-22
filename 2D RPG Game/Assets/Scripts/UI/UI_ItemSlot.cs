using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] private Image itemImage;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;
        
        itemImage.color = Color.white;
        
        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }
}
