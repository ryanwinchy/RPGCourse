using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTooltipUI : TooltipUI
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


        AdjustFontSize(itemNameText);      //If title quite long, reduce font size.
        AdjustPosition();                 //Offset tooltip dependent on mouse pos on hover.
        
    }

    public void HideTooltip()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }

}
