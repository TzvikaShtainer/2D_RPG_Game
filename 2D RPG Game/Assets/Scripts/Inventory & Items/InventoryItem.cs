using System;

[Serializable]
// like a decorator for a single item in the inventory
public class InventoryItem 
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData data)
    {
        this.data = data;
        AddStack();
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
