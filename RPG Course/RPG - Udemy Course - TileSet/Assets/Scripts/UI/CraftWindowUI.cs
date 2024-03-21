using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftWindowUI : MonoBehaviour       //This script is for the craft window that pops up when you select an item to craft.
{

    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDescription;
    [SerializeField] Image itemIcon;

    [SerializeField] Button craftButton;

    [SerializeField] Image[] materialImages;      //Array of the material images, can get child to get stack amt text.

    public void SetupCraftWindow(ItemDataEquipment _data)
    {

        craftButton.onClick.RemoveAllListeners();      //Removes all listeners from button. so nothing happens on click.

        for (int i = 0; i < materialImages.Length; i++)    //Cycle thru material images and its text, whipe it out upon setup.
        {
            materialImages[i].color = Color.clear;      //So can't see it.
            materialImages[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;  //text of that, set to invisible.
        }

        for (int i = 0; i < _data.craftingMaterials.Count; i++)   //On the item data equipment , we have the required crafting mats. Cycle thru them.
        {
            if (_data.craftingMaterials.Count > materialImages.Length)      //Trying to setup an item with more crafting mats than there are slots.
                Debug.LogWarning("You have more materials required than you have material slots in craft window. 4 max.");

            materialImages[i].sprite = _data.craftingMaterials[i].itemData.icon;     //Turn on image, get sprite from icon for that mat.
            materialImages[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImages[i].GetComponentInChildren<TextMeshProUGUI>();   //The stack amt display.

            materialSlotText.text = _data.craftingMaterials[i].stackSize.ToString();   //Turn on text, set to stack amt of material.
            materialSlotText.color = Color.white;

        }

        itemIcon.sprite = _data.icon;
        itemName.text = _data.name;
        itemDescription.text = _data.GetDescription();

        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data, _data.craftingMaterials));      //On click will run that function.


    }
}
