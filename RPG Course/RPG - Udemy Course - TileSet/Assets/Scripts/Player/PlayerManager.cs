using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour           //This script needs to be accessible by any script so can always access player.
{
    public static PlayerManager instance;    //Singleton pattern. This is now accessibly in any script. Without a ref, can go PlayerManager.instance.player . Instead of player = gameobject.find("player") which is VERY resource heavy. As searches thru all objects every time.
    public Player player;

    private void Awake()
    {
        if (instance != null)    //Check if any instance, if is, destroy it. If none, assign it. This is because we only want one instance of player manager. When change scenes,tries make two.
            Destroy(instance.gameObject);
        else
        instance = this; // first instance assigned, all others destroyed.
    }






}
