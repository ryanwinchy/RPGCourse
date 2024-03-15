using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary; //Dictionaries are a key system. Search thru items with key (itemData), get inventoryItem. Like list.

    public List<InventoryItem> stash;     //Stash is another inventory for mats. So two lists of items basically.
    public Dictionary<ItemData, InventoryItem> stashDictionary;   //Inventory item takes item data and has more info on it, like stack size. For items inside inv.

    public List<InventoryItem> equipment;                  //Equipment is list of equippable items.
    public Dictionary <ItemDataEquipment, InventoryItem> equipmentDictionary;    //Item data equipment (child) so can see the equipment type.
    
    [Header("Inventory UI")]

    [SerializeField] Transform inventorySlotParent;
    [SerializeField] Transform stashSlotParent;     //Stash is for materials and such.
    [SerializeField] Transform equipmentSlotParent;

    ItemSlotUI[] inventoryItemSlots;
    ItemSlotUI[] stashItemSlots;
    EquipmentSlotUI[] equipmentSlots;          //Equipment Slot UI is exactly the same as Item Slot, but has an Equipment type. Everything else is inherited identically.

    private void Awake()
    {
        if (instance == null)      //Singleton. If tries to create new when change scene, destroy the new one. Keep one only.
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();

        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();  //Finds all the item slot UIs in children, stores in array.
        stashItemSlots = stashSlotParent.GetComponentsInChildren<ItemSlotUI>();      //Array of stash slots.
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<EquipmentSlotUI>();   //Array of equipment slots.
    }

    public void EquipItem(ItemData _item)
    {
        ItemDataEquipment newEquipment = _item as ItemDataEquipment;      //Takes itemData and converts to itemDataEquipment (its derived class), exact same but also has equipmentType on it.
        InventoryItem newItem = new InventoryItem(newEquipment);    //Make inventory item for item.

        ItemDataEquipment oldEquipment = null;     //Null by default.

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)   //check each item in dictionary with key itemDataEquipment to find inventoryItem. Basically each item in list.
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)    //if new equipment type (like sword) is already equipped (in equipment dictionary / list).
            {
                oldEquipment = item.Key;   //Delete equipment already there, as new equip coming in is same type.
            }
        }

        if (oldEquipment != null)       //If there is an item to remove.
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);            //Old equipment was removed, so add it back to inventory.
        }

        equipment.Add(newItem);                          //Add the new item to the equipment list and dictionary.
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();            //Adds the modifiers of the new equipment equipped.

        RemoveItem(_item);   //Remove the item from the inventory, as it has been equipped.

        UpdateUISlots();

    }

    public void UnequipItem(ItemDataEquipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))   //If this item already in equipment inventory.
        {
            equipment.Remove(value);              //Remove from list and dictionary for tracking.
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();        //Remove modifiers from equipment removed.
        }
    }

    void UpdateUISlots()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)        //Go through all UI equipment slots.
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)   //Each equipment in dictionary.
            {
                if (item.Key.equipmentType == equipmentSlots[i].slotType)    //if equipment in dictionary (like sword) already has a slot (like sword slot).
                    equipmentSlots[i].UpdateSlot(item.Value);           //Update that slot with the equipment already in equipment dictionary. Value means inventoryItem, key and value for dictionary.
    
            }
        }



        for (int i = 0; i < inventoryItemSlots.Length; i++)    //Go thru all inventory item slots.
        {
            inventoryItemSlots[i].CleanupSlot();       //Cleanup, reset whole slot.
        }

        for (int i = 0; i < stashItemSlots.Length; i++)    //Go thru all stashh inventory item slots.
        {
            stashItemSlots[i].CleanupSlot();       //Cleanup, reset whole slot.
        }


        for (int i = 0; i < inventory.Count; i++)    //Go thru all inventory items.
        {
            inventoryItemSlots[i].UpdateSlot(inventory[i]);       //Update UI for all inventory items.
        }

        for (int i = 0; i < stash.Count; i++)    //Go thru all current stash items.
        {
            stashItemSlots[i].UpdateSlot(stash[i]);       //Update UI for all stash items.
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
            AddToInventory(_item);

        else if (_item.itemType == ItemType.Material)
            AddToStash(_item);



        UpdateUISlots();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value)) //If find item of this kind already in stash, add stack.
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);      //Create new stash item, as this one is first.
            stash.Add(newItem);                             //Add to list and dictionary for tracking.
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))  //If find item of this kind already in inventory, add stack.
        {
            value.AddStack();
        }
        else          //If no item of this kind in inventory currently. Create item and add to inventory.
        {
            InventoryItem newItem = new InventoryItem(_item);   //Create new inventory item, as this one is first.
            inventory.Add(newItem);                      //Add item to list of inventoryItems and Dictionary for tracking.
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))         //If find item of this kind in inventory already.
        {
            if (value.stackSize <= 1)           //If stack size of found inventory item is 1 or less.
            {
                inventory.Remove(value);            //Remove from list and dictionary from tracking. Then when UI updates, will remove that item from UI.
                inventoryDictionary.Remove(_item);
            }
            else   // > 1.
            {
                value.RemoveStack();            //If more than 1 in inventory, remove one from stack.
            }
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))   //Does this item have a stash inventory item? Is it in inv already.
        {
            if (stashValue.stackSize <= 1)     //If last one in stack.
            {
                stash.Remove(stashValue);       //Remove from list and dictionary for tracking. Then when UI updates, will remove that item from UI.
                stashDictionary.Remove(_item);
            }
            else     //If >1 in stack, remove one.
            {
                stashValue.RemoveStack();
            }
        }

        UpdateUISlots();

    }








}




