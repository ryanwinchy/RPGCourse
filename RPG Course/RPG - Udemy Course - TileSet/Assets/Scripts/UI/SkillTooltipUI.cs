using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTooltipUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI skillText;
    [SerializeField] TextMeshProUGUI skillName;

    public void ShowTooltip(string _skillDescription, string _skillName)
    {
        skillText.text = _skillDescription;
        skillName.text = _skillName;
        gameObject.SetActive(true);
    }

    public void HideTooltip() => gameObject.SetActive(false);

}
