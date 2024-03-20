using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour       //Script goes on in game item objects to handle item pickups etc...
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] ItemData itemData;



    private void SetupVisuals()
    {
        if (itemData == null)    //Good to do null checks otherwise we get errors.
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.name;       //Gives game object name from item data.
    }


    public void SetupItem(ItemData _itemData, Vector2 _velocity)      //Sets up the item with the data (what it is), and the velocity it comes out at.
    {

        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }

    public void PickupItem()
    {                                                                                         
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)     //If inventory slots full and is equipment, dont pick up.
        {                                                                                    //Inventory is for equip only, mats go different inventory.
            rb.velocity = new Vector2(0, 7);   //Bounces a bit when can't pickup.
            return;
        }

        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);   //Item picked up.
    }
}
