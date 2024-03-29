using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager           //This script needs to be accessible by any script so can always access player.
{
    public static PlayerManager instance;    //Singleton pattern. This is now accessibly in any script. Without a ref, can go PlayerManager.instance.player . Instead of player = gameobject.find("player") which is VERY resource heavy. As searches thru all objects every time.
    public Player player;

    public int currency;

    private void Awake()
    {
        if (instance != null)    //Check if any instance, if is, destroy it. If none, assign it. This is because we only want one instance of player manager. When change scenes,tries make two.
            Destroy(instance.gameObject);
        else
        instance = this; // first instance assigned, all others destroyed.
    }

    public bool HaveEnoughCurrency(int _price)
    {
        if (_price > currency)
        {
            Debug.Log("Not enough money!");
            return false;
        }

        currency -= _price;

        return true;
    }

    public int GetCurrency() => currency;

    public void LoadData(GameData _data)
    {
        currency = _data.savedCurrency;
    }

    public void SaveData(ref GameData _data)     //Reference sends the actual ref to the original variable, not a copy like normal arguments. So changing the parameter it receives also changes the original.
    {
        _data.savedCurrency = currency;
    }
}
