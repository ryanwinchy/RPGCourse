using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostCurrencyController : MonoBehaviour      //This is script to loot your body and regain currency you lost.
{

    public int currency;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            PlayerManager.instance.currency += currency;
            Destroy(gameObject);
        }
    }

}
