using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatTooltipUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI description;

    public void ShowStatTooltip(string _text)
    {
        description.text = _text;

        gameObject.SetActive(true);
    }

    public void HideStatTooltip()
    {
        description.text = "";

        gameObject.SetActive(false);
    }


}
