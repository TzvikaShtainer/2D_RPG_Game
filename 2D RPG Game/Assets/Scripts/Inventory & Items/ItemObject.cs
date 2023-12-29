using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;
    
    private void SetupVisuals()
    {
        if(itemData == null)
            return;
        
        GetComponent<SpriteRenderer>().sprite =  itemData.icon;
        gameObject.name = "Item Object - " +itemData.name;
    }
    
    public void SetupItem(ItemData _item, Vector2 _velocity)
    {
        itemData = _item;
        rb.velocity = _velocity;
        
        SetupVisuals();
    }
    public void PickUpItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7);
            return;
        }
        
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
