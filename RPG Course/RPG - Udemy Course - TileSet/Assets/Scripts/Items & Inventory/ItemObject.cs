using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour       //Script goes on in game item objects to handle item pickups etc...
{
    [SerializeField] ItemData ItemData;


    private void OnValidate()          //This is called whenever you change anything on the object in Unity.
    {
        GetComponent<SpriteRenderer>().sprite = ItemData.icon;    
        gameObject.name = "Item object - " + ItemData.name;       //Gives game object name from item data.
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)     //If PLAYER picks up this item.
        {
            Debug.Log("Picked up " + ItemData.name);
            Inventory.instance.AddItem(ItemData);
            Destroy(gameObject);   //Item picked up.
        }
    }
}
