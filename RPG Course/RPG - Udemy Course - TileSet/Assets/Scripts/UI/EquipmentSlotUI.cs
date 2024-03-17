using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotUI : ItemSlotUI
{
    public EquipmentType slotType;        //This is the type of equipment slot it is . Eg one for sword, armour etc... Can unlock more?!

    private void OnValidate()          //Whenever anything changed in Unity editor.
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)    //Different logic than itemslot for when clicked, so inherits and polymorphs.
    {
        Inventory.instance.UnequipItem(item.itemData as ItemDataEquipment);         //Unequip item when click on equip slot. Pass in itemData converted to itemDataEquipment (its child).
        Inventory.instance.AddItem(item.itemData);            //Old equipment was removed, so add it back to inventory.

        CleanupSlot();      //Cleanup slot when remove the item.
    }

}
