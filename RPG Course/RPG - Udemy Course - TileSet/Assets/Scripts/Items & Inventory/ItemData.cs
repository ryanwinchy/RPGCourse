using System.Text;
using UnityEngine;

#if UNITY_EDITOR         //gets rid of build errors, due to saving part below OnValidate.
using UnityEditor;
#endif


public enum ItemType { Material, Equipment }

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]          //Creates right click menu option. Create -> Data -> Item
public class ItemData : ScriptableObject        //This is a scriptable object - like a template.
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public string itemId;         //This is so we can save.

    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder stringBuilder = new StringBuilder();   //This is a way to show multiple data, in this case item stats.

    private void OnValidate()
    {
#if UNITY_EDITOR                                                  //Weird syntax, but this is how we save itemData, it assigns an ID to each iemData in Unity. Can then save the id, and use it to find items.
        string path = AssetDatabase.GetAssetPath(this);            //Was added at end, purely for saving data. it basically gives it an ID, isnt that scary.
        itemId = AssetDatabase.AssetPathToGUID(path);             //This is for unity editor only.
#endif

    }

    public virtual string GetDescription()
    {
        return "";
    }
}
