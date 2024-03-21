using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    //Unlockable skills.
    [Header("Dash")]
    [SerializeField] SkillTreeSlotUI dashUnlockButton;
    public bool dashUnlocked { get; private set; }  //Set to public for testing, then made {get; private set;} property when working. Other scripts can only get, not set.

    [Header("Clone on Dash")]
    [SerializeField] SkillTreeSlotUI cloneOnDashUnlockButton;
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on Arrival")]
    [SerializeField] SkillTreeSlotUI cloneOnArrivalUnlockButton;
    public bool cloneOnArrivalUnlocked { get; private set; }



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

    protected override void CheckIfSkillLoaded()     //If we have save data with skill unlocked, have to run this to unlock the skills. Otherwise button shows unlocked but not skill.
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
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
