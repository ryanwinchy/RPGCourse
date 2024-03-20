using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTooltipUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemTypeText;
    [SerializeField] TextMeshProUGUI itemDescription;

    [SerializeField] int defaultFontSize = 32;
    void Start()
    {
        
    }

    public void ShowTooltip(ItemDataEquipment _item)
    {
        if (_item == null)
            return;

        itemNameText.text = _item.itemName;
        itemTypeText.text = _item.equipmentType.ToString();
        itemDescription.text = _item.GetDescription();
        
        gameObject.SetActive(true);    //Show this tooltip.


        if (itemNameText.text.Length > 13)    //If title of tooltip is quite long, reduce the size so fits on one line.
        {
            itemNameText.fontSize *= 0.7f;
        }
        else
            itemNameText.fontSize = defaultFontSize;

    }

    public void HideTooltip()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }

}
