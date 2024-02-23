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

    public void CreateClone(Transform clonePosition)
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, canAttack);
    }
}
