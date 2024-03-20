using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    //Unlockable skills.
    [Header("Dash")]
    public bool dashUnlocked;
    [SerializeField] SkillTreeSlotUI dashUnlockButton;

    [Header("Clone on Dash")]
    public bool cloneOnDashUnlocked;
    [SerializeField] SkillTreeSlotUI cloneOnDashUnlockButton;

    [Header("Clone on Arrival")]
    public bool cloneOnArrivalUnlocked;
    [SerializeField] SkillTreeSlotUI cloneOnArrivalUnlockButton;



    public override void UseSkill()
    {
        base.UseSkill();

        Debug.Log("Creates clone behind you!");
    }

    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);       //When that button clicked, run unlock dash.
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener (UnlockCloneOnArrival);
    }

    void UnlockDash()
    {
        if (dashUnlockButton.unlocked)
            dashUnlocked = true;
    }

    void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }



    public void CloneOnDash()       //Skills are actually on the skill now! Checks if unlocked, if so, run that skill.
    {
        if (cloneOnDashUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }
}
