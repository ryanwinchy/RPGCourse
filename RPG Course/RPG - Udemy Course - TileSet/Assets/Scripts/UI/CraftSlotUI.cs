using UnityEngine.EventSystems;

public class CraftSlotUI : ItemSlotUI      //Inherits name and image from item slot.
{



    protected override void Start()
    {
        base.Start();    //gets access to ui if we need.
    }
    void OnEnable()
    {
        UpdateSlot(item);
    }

    public void SetupCraftSlot(ItemDataEquipment _data)
    {

        if (_data == null)
            return;

        item.itemData = _data;      //Already have item (inherited). Assigning data to it.

        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;

        if (itemText.text.Length > 12)
            itemText.fontSize *= 0.7f;
        else
            itemText.fontSize = 24;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetupCraftWindow(item.itemData as ItemDataEquipment);   //Send data of what we have in this slot. In this case send child, itemdata equipment.
    }




}
