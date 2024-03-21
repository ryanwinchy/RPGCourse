using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill        //This script is to trigger dodge. But versatile, could be used to trigger black hole, item effect, healing, buff , anything.
{

    [Header("Dodge")]
    [SerializeField] SkillTreeSlotUI unlockDodgeButton;
    [SerializeField] int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Mirage Dodge")]
    [SerializeField] SkillTreeSlotUI unlockDodgeMirageButton;
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockDodgeMirageButton.GetComponent<Button>().onClick.AddListener(UnlockDodgeMirage);
    }

    void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)   //Second check so cant keep pressing button to gain evasion.
        {
            player.stats.evasion.AddModifier(evasionAmount);    //Dodge skill basically gives evasion which means can start dodging. Assumes evasion starts at 0.
            Inventory.instance.UpdateStatsUI();      //So modifier is instantly added to UI.

            dodgeUnlocked = true;
        }
    }

    void UnlockDodgeMirage()
    {
        if (unlockDodgeMirageButton.unlocked)
            dodgeMirageUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, new Vector2 (2 * player.facingDir, 0));
    }

}
