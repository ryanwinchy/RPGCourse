using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Interface - can have same name but diff functionality on different objects. If declare on script, MUST be used. It's like a template.
//IPointerDownHandler is an interface made by Unity, that gives us the OnPointerDown function for when clicked.
public class ItemSlotUI : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemText;

    public InventoryItem item;


    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;      //Slot is transparent, then goes white (no colour) when item added.

        if (item != null)
        {
            itemImage.sprite = item.itemData.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanupSlot()     //Resets everything.
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)     //Called whenever you click on the slot, so the UI slot. Overriden by equipment slot.
    {
        if (item.itemData.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.itemData);

    }




}
