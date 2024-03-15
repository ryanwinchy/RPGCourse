using System;

[Serializable]         //Can see in inspector, as this doesn't inherit from mono.
public class InventoryItem               //This class is for data on an item and stack size, similar to the 'stat' script. Items actually in inventory.
{
    public ItemData itemData;
    public int stackSize;

    public InventoryItem(ItemData _newItemData)
    {
        itemData = _newItemData;
        AddStack();     //When new item created, starts by adding one of it. So starts with 1 stack size.
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;

}
