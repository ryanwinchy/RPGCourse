using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill
{
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] float crystalDuration;
    GameObject currentCrystal;


    [Header("Simple Crystal")]
    [SerializeField] SkillTreeSlotUI unlockCrystalButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Crystal Mirage")]
    [SerializeField] SkillTreeSlotUI unlockCrystalMirageButton;
    [SerializeField] bool cloneInsteadOfCrystal;

    [Header("Explosive Crystal")]
    [SerializeField] SkillTreeSlotUI unlockExplosiveButton;
    [SerializeField] bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] SkillTreeSlotUI unlockMovingCrystalButton;
    [SerializeField] bool canMoveToEnemy;
    [SerializeField] float moveSpeed;

    [Header("Multi Crystal")]
    [SerializeField] SkillTreeSlotUI unlockMultiCrystalButton;
    [SerializeField] bool canUseMultiCrystal;      //Unlockable.
    [SerializeField] int amtCrystals;
    [SerializeField] float multiCrystalCooldown;
    [SerializeField] float useTimeWindow;
    [SerializeField] List<GameObject> crystalsLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);       //On button click, check if skill unlocked in UI, if is, unlock here.
        unlockCrystalMirageButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener (UnlockMovingCrystal);
        unlockMultiCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMultiCrystal);
    }

    #region Unlock Skills

    protected override void CheckIfSkillLoaded()     //If we have save data with skill unlocked, have to run this to unlock the skills. Otherwise button shows unlocked but not skill.
    {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockExplosiveCrystal();
        UnlockMovingCrystal();
        UnlockMultiCrystal();
    }

    void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
            crystalUnlocked = true;
    }

    void UnlockCrystalMirage()
    {
        if (unlockCrystalMirageButton.unlocked)
            cloneInsteadOfCrystal = true;
    }

    void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveButton.unlocked)
            canExplode = true;
    }

    void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
            canMoveToEnemy = true;
    }

    void UnlockMultiCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
            canUseMultiCrystal = true;
    }

    #endregion


    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())       //if statement will run the whole function. If can do multi crystal, dont do rest of the using skill below.
            return;

        if (currentCrystal == null) //If no crystal, instantiate one
        {
            CreateCrystal();
        }
        else
        {

            if (canMoveToEnemy)        //If move towards unlocked, dont do the swapping functionality, have to give something up, cant have it all.
                return;

            Vector2 playerPos = player.transform.position;          //These 3 lines happen no matter what type of crystal they have.
            player.transform.position = currentCrystal.transform.position;          //If crystal already, teleport to it.
            currentCrystal.transform.position = playerPos;          //after placed player on crystal pos, put crystal where player was.

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);  //Spawn clone at current crystal pos.
                Destroy(currentCrystal);
            }
            else        //if cloneinsteadofcrystal not unlocked, finish crystal normally.
            {
                currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();          //If crystal not null, finish crystal.
            }


            
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        CrystalSkillController currentCrystalScript = currentCrystal.GetComponent<CrystalSkillController>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);

    }

    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();
    bool CanUseMultiCrystal()
    {
        if (canUseMultiCrystal)
        {
            if (crystalsLeft.Count > 0)    //Still have crystals left.
            {

                if (crystalsLeft.Count == amtCrystals)    //If you use a couple of crystals and still have one left, it will reset after interval.
                    Invoke("RefillCrystal", useTimeWindow);

                cooldown = 0;
                GameObject crystalToSpawn = crystalsLeft[crystalsLeft.Count - 1];    //Choose last one in list. As count starts from 0.
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);    //Spawn in crystal.

                crystalsLeft.Remove(crystalToSpawn);   //Remove from list.
                                                       //Setup object we just created.
                newCrystal.GetComponent<CrystalSkillController>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);

                if (crystalsLeft.Count <= 0)
                {
                    cooldown = multiCrystalCooldown;   //So only goes on cooldown once all crystals spent.
                    RefillCrystal();
                }

                return true;

            }

        }

        return false;
    }

    void RefillCrystal()          //Refills list full of crystals.
    {
        int amtToAdd = amtCrystals - crystalsLeft.Count;

        for (int i = 0; i < amtToAdd; i++)          //Add only amount of crystals missing.
        {
            crystalsLeft.Add(crystalPrefab);
        }
    }

    void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiCrystalCooldown;
        RefillCrystal();
    }

}

