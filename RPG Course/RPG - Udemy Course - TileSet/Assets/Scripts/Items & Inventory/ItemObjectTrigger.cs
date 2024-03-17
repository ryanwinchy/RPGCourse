using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    ItemObject itemObject => GetComponentInParent<ItemObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)     //If PLAYER picks up this item.
        {
            if (collision.GetComponent<CharacterStats>().isDead)    //If dead, we do not pick up items.
                return;

            itemObject.PickupItem();
        }
    }

}
