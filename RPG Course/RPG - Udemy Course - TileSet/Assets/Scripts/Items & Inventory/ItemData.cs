using UnityEngine;

public enum ItemType { Material, Equipment}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]          //Creates right click menu option. Create -> Data -> Item
public class ItemData : ScriptableObject        //This is a scriptable object - like a template.
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;


}
