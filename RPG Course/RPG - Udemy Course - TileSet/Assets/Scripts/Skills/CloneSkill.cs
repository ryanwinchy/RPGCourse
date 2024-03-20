using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{


    [Header("Clone Info")]
    [SerializeField] GameObject clonePrefab;
    [SerializeField] float cloneDuration;
    [Space]
    [SerializeField] bool canAttack;       //This will eventually be skill tree unlock. For now, inspector bool for testing.


    [SerializeField] bool canCreateCloneOnCounter;

    [Header("Clone Can Duplicate")]
    [SerializeField] bool canDuplicateClone;
    [SerializeField] float chanceToDuplicate;

    [Header("Crystal Instead of Clone")]
    public bool crystalInsteadOfClone;

    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();

            return;                                           //So creates clone then leaves, does not proceed below to create a clone.
        }

        

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, canAttack, offset, FindClosestEnemy(newClone.transform), 
            canDuplicateClone, chanceToDuplicate, player);
    }


    public void CreateCloneOnCounter(Transform enemy)
    {
        if (canCreateCloneOnCounter)
            StartCoroutine(CreateCloneWithDelay(0.4f, enemy, new Vector3(2 * player.facingDir, 0)));
    }

    IEnumerator CreateCloneWithDelay(float time, Transform transform, Vector3 offset)
    {
        yield return new WaitForSeconds(time);
            CreateClone(transform, offset); //spawn clone behind enemy. If im on left enemy on right and I counter, will spawn clone behind enemy always.
    }




}
