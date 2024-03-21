using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatTooltipUI : TooltipUI
{
    [SerializeField] TextMeshProUGUI description;

    public void ShowStatTooltip(string _text)
    {
        description.text = _text;
        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideStatTooltip()
    {
        description.text = "";

        gameObject.SetActive(false);
    }


}
