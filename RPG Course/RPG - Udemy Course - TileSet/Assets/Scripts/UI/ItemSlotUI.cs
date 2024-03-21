using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Interface - can have same name but diff functionality on different objects. If declare on script, MUST be used. It's like a template.
//IPointerDownHandler is an interface made by Unity, that gives us the OnPointerDown function for when clicked.
//IPointerEnter and Exit is for when mouse is on hover basically. We need so can show item details in game!
public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    public InventoryItem item;

    protected UI ui;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

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
        if (item == null)    //If slot empty.
            return;

        if (Input.GetKey(KeyCode.LeftControl))           //If click on item slot and hold left control (GetKey), remove item.
        {
            Inventory.instance.RemoveItem(item.itemData);
            return;
        }


        if (item.itemData.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.itemData);

        ui.itemTooltip.HideTooltip();   //Hide it when click.

    }

    public void OnPointerEnter(PointerEventData eventData)   //Show item info on hover.
    {
        if (item == null)      //If slot is empty.
            return;



        ui.itemTooltip.ShowTooltip(item.itemData as ItemDataEquipment);     //Is child of itemData so can just use 'as'.

    }

    public void OnPointerExit(PointerEventData eventData)     //Hide item info on hover.
    {
        if (item == null)      //If slot is empty.
            return;

        ui.itemTooltip.HideTooltip();
    }
}
