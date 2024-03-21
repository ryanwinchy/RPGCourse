using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTooltipUI : TooltipUI
{
    [SerializeField] TextMeshProUGUI skillText;
    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] TextMeshProUGUI skillCost;
    [SerializeField] float defaultNameFontSize;


    public void ShowTooltip(string _skillDescription, string _skillName, int _price)
    {
        skillText.text = _skillDescription;
        skillName.text = _skillName;
        skillCost.text = "Cost: " + _price;

        AdjustPosition();      //Gets from parent script.
        AdjustFontSize(skillName);

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        skillName.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }


}
