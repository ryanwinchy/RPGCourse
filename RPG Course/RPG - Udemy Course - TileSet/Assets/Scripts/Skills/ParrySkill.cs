using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] SkillTreeSlotUI parryUnlockButton;
    public bool parryUnlocked {  get; private set; }          //Set to public for testing, then made {get; private set;} property when working. Other scripts can only get, not set.

    [Header("Healing Parry")]
    [SerializeField] SkillTreeSlotUI healingParryUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] float healthRestorePercentage;
    public bool healingParryUnlocked { get; private set; }

    [Header("Parry with Mirage")]
    [SerializeField] SkillTreeSlotUI parryMirageUnlockButton;
    public bool parryMirageUnlocked { get; private set; }
    public override void UseSkill()
    {
        base.UseSkill();

        if (healingParryUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * healthRestorePercentage);
            player.stats.IncreaseHealthyBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();
        
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);           //When buttons clicked try to unlock. Checks if unlocked in skilltree slot UI, if it is, unlock. 
        healingParryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockHealingParry);
        parryMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    protected override void CheckIfSkillLoaded()     //If we have save data with skill unlocked, have to run this to unlock the skills. Otherwise button shows unlocked but not skill.
    {
        UnlockParry();
        UnlockHealingParry();
        UnlockParryWithMirage();
    }

    void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    void UnlockHealingParry()
    {
        if (healingParryUnlockButton.unlocked)
            healingParryUnlocked = true;
    }

    void UnlockParryWithMirage()
    {
        if (parryMirageUnlockButton.unlocked)
            parryMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform spawnPos)
    {
        if (parryMirageUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(spawnPos);
    }

}
