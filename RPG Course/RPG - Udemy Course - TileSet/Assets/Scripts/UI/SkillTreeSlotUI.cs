using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlotUI : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    UI ui;
    Image skillImage;

    [SerializeField] int skillPrice;
    [SerializeField] string skillName;
    [TextArea]
    [SerializeField] string skillDescription;

    [SerializeField] Color lockedSkillColour;

    public bool unlocked;

    [SerializeField] SkillTreeSlotUI[] PrerequisiteSkills;     //Put into here the skills that are prerequisite.
    [SerializeField] SkillTreeSlotUI[] alternatePathSkills;   //Put here skills that cannot be unlocked at same time. Assumes one at a time.


    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlotUI - " + skillName;    //makes life easier in inspector.
    }

    private void Awake()                          //Awake as we want to listen for button press straight away, before skill scripts run their checks if unlocked or not.
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());   //When button pressed, run unlock skill slot.
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();
        skillImage = GetComponent<Image>();

        skillImage.color = lockedSkillColour;       //default for locked skills.

    }

    public void UnlockSkillSlot()
    {

        if (PlayerManager.instance.HaveEnoughCurrency(skillPrice) == false)     //Check if we have enough money for skill. If no, return, do not unlock.
            return;

        for (int i = 0; i < PrerequisiteSkills.Length; i++)
        {
            if (PrerequisiteSkills[i].unlocked == false)      //If any required skills not unlocked, exit.
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        for (int i = 0; i < alternatePathSkills.Length; i++) //If any alternate path skills are unlocked, exit. One at a time.
        {
            if (alternatePathSkills[i].unlocked == true)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        unlocked = true;      
        skillImage.color = Color.white;   //So can see fully when unlocked.
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTooltip.ShowTooltip(skillDescription, skillName);

        Vector2 mousePosition = Input.mousePosition;
         
        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 600)                //Nicely moves tooltip towards where the skill is.
            xOffset = -150;
        else
            xOffset = 150;

        if (mousePosition.y > 320)                //Nicely moves tooltip towards where the skill is.
            yOffset = -150;
        else
            yOffset = 150;

        ui.skillTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.HideTooltip();
    }
}
