using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrops;
    [SerializeField] private List<ItemData> dropList = new List<ItemData>();
    
    
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private ItemData item;


    public void GenerateDrop()
    {
        for (int i = 0; i < possibleDrops.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrops[i].dropChance)
                dropList.Add(possibleDrops[i]);
        }

        for (int i = 0; i < possibleItemDrop; i++)
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];
            
            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }
    public void DropItem(ItemData _item)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));
        
        newDrop.GetComponent<ItemObject>().SetupItem(_item, randomVelocity);
    }
}
