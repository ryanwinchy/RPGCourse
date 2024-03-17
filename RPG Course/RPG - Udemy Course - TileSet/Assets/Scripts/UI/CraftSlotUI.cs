using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftSlotUI : ItemSlotUI
{
    void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemDataEquipment craftData = item.itemData as ItemDataEquipment;     //Take the item we have in slot (parent script) and convert to child, itemDataEquipment because it has the crafting info.

        Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);
    }




}
