using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);
    }

    protected override void Start()
    {
        base.Start();
    }

    public void SetupCraftSlot(ItemData_Equipment data)
    {
        if(data == null)
            return;
        
        item.data = data;

        itemImage.sprite = data.icon;
        itemText.text = data.itemName;

        // if ( itemText.text.Length > 12)
        //     itemText.fontSize = itemText.fontSize * 0.8f;
        // else
        //     itemText.fontSize = 24;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
    }
}
