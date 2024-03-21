using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]  //So can see in inspector without monobehaviour.

public class GameData
{

    public int currency;

    public SerializableDictionary<string, int> savedInventory;               //String is item name, int is stack size. Don't worry about serializable dictionary class, its same as normal dictionary but serializable so saveable. Just copy it and use it, dont need to know ins and outs.
    public SerializableDictionary<string, bool> savedSkillTree;      //String as skill name, bool as learned or not learned.
    public List<string> savedEquipmentIDs;        //List of equipment item IDs to save.

    public GameData()        //Constructor. When new game, everything defaults to 0.
    {
        this.currency = 0;

        savedInventory = new SerializableDictionary<string, int>();
        savedSkillTree = new SerializableDictionary<string, bool>();
        savedEquipmentIDs = new List<string>();

    }




}
