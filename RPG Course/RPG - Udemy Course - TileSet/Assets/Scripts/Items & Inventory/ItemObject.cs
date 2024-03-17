using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour       //Script goes on in game item objects to handle item pickups etc...
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] ItemData ItemData;



    private void SetupVisuals()
    {
        if (ItemData == null)    //Good to do null checks otherwise we get errors.
            return;

        GetComponent<SpriteRenderer>().sprite = ItemData.icon;
        gameObject.name = "Item object - " + ItemData.name;       //Gives game object name from item data.
    }


    public void SetupItem(ItemData _itemData, Vector2 _velocity)      //Sets up the item with the data (what it is), and the velocity it comes out at.
    {

        ItemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }

    public void PickupItem()
    {
        Inventory.instance.AddItem(ItemData);
        Destroy(gameObject);   //Item picked up.
    }
}
