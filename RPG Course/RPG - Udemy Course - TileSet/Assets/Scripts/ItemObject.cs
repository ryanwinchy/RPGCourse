using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour       //Script to handle item pickups etc...
{
    SpriteRenderer spriteRenderer;
    [SerializeField] ItemData ItemData;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = ItemData.icon;     //Assigns the image.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)     //If PLAYER picks up this item.
        {
            Debug.Log("Picked up item " + ItemData.itemName);
            Destroy(gameObject);   //Item picked up.
        }
    }
}
