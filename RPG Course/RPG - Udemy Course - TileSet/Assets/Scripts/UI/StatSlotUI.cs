using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

//IPointerEnter and Exit is for when mouse is on hover basically. We need so can show item details in game! Then can override the interface to get those methods.
public class StatSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UI ui;

    [SerializeField] string statName;

    [SerializeField] StatType statType;                    //Interesting, can use enums across Unity, even if no script ref.
    [SerializeField] TextMeshProUGUI statValueText;
    [SerializeField] TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] string statDescription;


    private void OnValidate()              //Whenever anything in Unity menu changes.
    {
        gameObject.name = "Stat - " + statName;

        if (statNameText != null )
            statNameText.text = statName;
    }

    // Start is called before the first frame update
    void Start()
    {
        ui = GetComponentInParent<UI>();

        UpdateStatValueUI();
    }


    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStatOfType(statType).GetValue().ToString();       //We send stat type, it returns value and converts to string.


            if (statType == StatType.health)                 //all stats that are modified by major stats, have to be calcuated in UI same way as char stats script.
                statValueText.text = playerStats.GetMaxHealthValue().ToString();  //Health is health plus vitality.

            if (statType == StatType.damage)                 
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();     //Damage is damage + strength , calculated only within methods so have to calc here.

            if (statType == StatType.critPower)                //CALC SAME AS IN CHARACTER STAT SCRIPT. HAS TO MATCH OR UI WRONG!
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();

            if (statType == StatType.critChance)                
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();

            if (statType == StatType.evasion)                
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();

            if (statType == StatType.magicResistance)                
                statValueText.text = (playerStats.magicResistance.GetValue() + (playerStats.intelligence.GetValue() * 3)).ToString();

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statTooltip.ShowStatTooltip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTooltip.HideStatTooltip();
    }
}
