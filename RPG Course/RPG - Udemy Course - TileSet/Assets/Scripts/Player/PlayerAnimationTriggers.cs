using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()     //Play on animation frame attack hits.
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);  //temp array of all colliders in attack circle when called.

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)       //if hit an enemy in attack circle.
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();      //get stats script of enemy you hit.

                if (_target != null)
                    player.stats.DoDamage(_target);        //On player, get its stats script, run do damage using player stats on target.



                ItemDataEquipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);   //On attack, execute effect on weapon. Func it calls check if weapon has effect.

                if (weaponData != null)                 //Only if has weapon.
                    weaponData.ExecuteItemEffect(_target.transform);


            }
        }
    }


    void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
