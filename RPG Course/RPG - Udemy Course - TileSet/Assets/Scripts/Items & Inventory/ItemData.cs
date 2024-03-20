using System.Text;
using UnityEngine;

public enum ItemType { Material, Equipment}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]          //Creates right click menu option. Create -> Data -> Item
public class ItemData : ScriptableObject        //This is a scriptable object - like a template.
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;

    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder stringBuilder = new StringBuilder();   //This is a way to show multiple data, in this case item stats.

    public virtual string GetDescription()
    {
        return "";
    }
}
