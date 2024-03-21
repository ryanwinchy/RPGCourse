using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackholeSkill : Skill
{

    [SerializeField] GameObject blackholePrefab;

    [SerializeField] SkillTreeSlotUI blackholeUnlockButton;
    public bool blackholeUnlocked {  get; private set; }

    [SerializeField] int numAttacks;
    [SerializeField] float cloneCooldown;
    [SerializeField] float blackholeDuration;
    [Space]
    [SerializeField] float maxSize;
    [SerializeField] float growSpeed;
    [SerializeField] float shrinkSpeed;

    BlackholeSkillController currentBlackhole;


    void UnlockBlackhole()
    {
        if (blackholeUnlockButton.unlocked)
            blackholeUnlocked = true;
    }
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        currentBlackhole = newBlackhole.GetComponent<BlackholeSkillController>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, numAttacks, cloneCooldown, blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        blackholeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }

    public bool BlackholeFinished()
    {
        if (!currentBlackhole)
            return false;

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;        //Radius of collider is set to 0.5, so we take half the scale to get the correct rad.
    }


    protected override void CheckIfSkillLoaded()     //If we have save data with skill unlocked, have to run this to unlock the skills. Otherwise button shows unlocked but not skill.
    {
        UnlockBlackhole();
    }

}
