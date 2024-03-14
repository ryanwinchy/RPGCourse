using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]          //Creates right click menu option. Create -> Data -> Item
public class ItemData : ScriptableObject        //This is a scriptable object - like a template.
{

    public string itemName;
    public Sprite icon;
}
