using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;
    [SerializeField] private Vector2 velocity;
    
    private void OnValidate()
    {
        if(itemData == null)
            return;
        
        GetComponent<SpriteRenderer>().sprite =  itemData.icon;
        gameObject.name = "Item Object - " +itemData.name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            rb.velocity = velocity;
        }
    }

    public void SetupItem(ItemData _item, Vector2 _velocity)
    {
        itemData = _item;
        rb.velocity = _velocity;
    }
    public void PickUpItem()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
