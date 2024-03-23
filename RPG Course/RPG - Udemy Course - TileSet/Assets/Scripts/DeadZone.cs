using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is like out of bounds for when player / enemy falls off map.
public class DeadZone : MonoBehaviour 
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CharacterStats>() != null)                  //If entity (like player or enemy) hits dead zone, kill entity.
        {
            Debug.Log("Killed in dead zone!");
            collision.GetComponent<CharacterStats>().KillEntity();
        }
        else
            Destroy(collision.gameObject);        //if something else falls like an item ,just destroy it.
    }
}
