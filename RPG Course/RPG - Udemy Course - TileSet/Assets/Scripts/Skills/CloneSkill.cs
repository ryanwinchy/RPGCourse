using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{


    [Header("Clone Info")]
    [SerializeField] float attackMultiplier;
    [SerializeField] GameObject clonePrefab;
    [SerializeField] float cloneDuration;
    [Space]

    [Header("Clone Attack")]
    [SerializeField] SkillTreeSlotUI cloneAttackUnlockButton;
    [SerializeField] float cloneAttackMultiplier;
    [SerializeField] bool cloneCanAttackUnlocked;       //This will eventually be skill tree unlock. For now, inspector bool for testing.

    [Header("Aggressive Clone")]
    [SerializeField] SkillTreeSlotUI aggressiveCloneUnlockButton;      //Increases clone attack multiplier (clone damage).
    [SerializeField] float aggressiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect {  get; private set; }


    [Header("Multiple Clones")]
    [SerializeField] SkillTreeSlotUI multipleUnlockButton;
    [SerializeField] float multipleCloneAttackMultiplier;
    [SerializeField] bool canDuplicateClone;
    [SerializeField] float chanceToDuplicate;

    [Header("Crystal Instead of Clone")]
    [SerializeField] SkillTreeSlotUI crystalInsteadUnlockButton;
    public bool crystalInsteadOfClone;


    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
    }

    #region Unlock Region

    protected override void CheckIfSkillLoaded()     //If we have save data with skill unlocked, have to run this to unlock the skills. Otherwise button shows unlocked but not skill.
    {
        UnlockCloneAttack();
        UnlockAggressiveClone();
        UnlockMultiClone();
        UnlockCrystalInstead();
    }
    void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {

            cloneCanAttackUnlocked = true;
            attackMultiplier = cloneAttackMultiplier;      //Standard.
        }
    }

    void UnlockAggressiveClone()
    {
        if (aggressiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneAttackMultiplier;    //Increased damage.
        }

    }

    void UnlockMultiClone()
    {
        if (multipleUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multipleCloneAttackMultiplier;
        }
    }

    void UnlockCrystalInstead()
    {
        if (crystalInsteadUnlockButton.unlocked)
            crystalInsteadOfClone = true;
    }


    #endregion














    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();

            return;                                           //So creates clone then leaves, does not proceed below to create a clone.
        }

        

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, cloneCanAttackUnlocked, offset, FindClosestEnemy(newClone.transform),
            canDuplicateClone, chanceToDuplicate, player, attackMultiplier);
    }


    public void CreateCloneWithDelay(Transform enemy)
    {
            StartCoroutine(CloneWithDelayCoroutine(0.4f, enemy, new Vector3(2 * player.facingDir, 0)));
    }

    IEnumerator CloneWithDelayCoroutine(float time, Transform transform, Vector3 offset)
    {
        yield return new WaitForSeconds(time);
            CreateClone(transform, offset); //spawn clone behind enemy. If im on left enemy on right and I counter, will spawn clone behind enemy always.
    }


    public void MakeMirageOnParry()
    {

    }


}
