using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's Drop")]
    [SerializeField] float chanceToLoseItems;
    [SerializeField] float chanceToLoseMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;   //So don't have to keep typing it out.

        List<InventoryItem> currentStash = inventory.GetStashList();
        List<InventoryItem> currentEquipment = inventory.GetEquipmentList();

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();     //The reason we use a temp list list this, is because otherwise foreach loop breaks, its going for each a list of changing length.
        List<InventoryItem> materialsToLose = new List<InventoryItem>();

        foreach (InventoryItem item in currentEquipment)   //Go thru current equipment.
        {
            if (Random.Range(0, 100) <= chanceToLoseItems)        //RNG to see if dropped or not.
            {
                DropItem(item.itemData);          //Drop item and unequip equipment from inventory.
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].itemData as ItemDataEquipment);
        }

        foreach (InventoryItem item in currentStash)
        {
            if (Random.Range(0, 100) <= chanceToLoseMaterials)
            {
                DropItem(item.itemData);
                materialsToLose.Add(item);
            }
        } 

        for(int i = 0;i < materialsToLose.Count;i++)
        {
            inventory.RemoveItem(materialsToLose[i].itemData);
        }
    }

}
