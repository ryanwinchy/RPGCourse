using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] float crystalDuration;
    GameObject currentCrystal;

    [Header("Crystal Mirage")]
    [SerializeField] bool cloneInsteadOfCrystal;

    [Header("Explosive Crystal")]
    [SerializeField] bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] bool canMoveToEnemy;
    [SerializeField] float moveSpeed;

    [Header("Multi Crystal")]
    [SerializeField] bool canUseMultiCrystal;      //Unlockable.
    [SerializeField] int amtCrystals;
    [SerializeField] float multiCrystalCooldown;
    [SerializeField] float useTimeWindow;
    [SerializeField] List<GameObject> crystalsLeft = new List<GameObject>();

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

