using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//Interfaces are for on and off hover, and then so we can save the skills,
public class SkillTreeSlotUI : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, ISaveManager
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

    private void Awake()                          //Awake as we want the unlocking skill slot to happen instantly when clicked, THEN the skill scripts listening for unlock can actually unlock the skill in their start. This has to run first.
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());   //When button pressed, run unlock skill slot.
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();
        skillImage = GetComponent<Image>();

        skillImage.color = lockedSkillColour;       //default for locked skills.

        if (unlocked)
            skillImage.color = Color.white;
    }

    public void UnlockSkillSlot()
    {
        if (unlocked)       //So cant pay to unlock twice.
            return;

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
        ui.skillTooltip.ShowTooltip(skillDescription, skillName, skillPrice);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.HideTooltip();
    }

    public void LoadData(GameData _data)
    {
        if (_data.savedSkillTree.TryGetValue(skillName, out bool value))   //From saved skill tree, see if this skill slot name is in list, if is get value and set unlocked in this script to it .
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data)       //Overview: If skill already saved in saved skill tree, remove then add. If not in saved skill tree, just add.
    {
        if (_data.savedSkillTree.TryGetValue(skillName, out bool value))   //use skillName as key. See if its already in saved Skill tree.
        {
            _data.savedSkillTree.Remove(skillName);                 //Remove existing saved skill tree for this skill slot. Otherwise get errors.
            _data.savedSkillTree.Add(skillName, unlocked);           //Add this skill tree slot name , and the unlocked bool.
        }
        else
            _data.savedSkillTree.Add(skillName, unlocked);          //If skill name was not in saved skill tree, add it and whether or not unlocked.
    }
}
