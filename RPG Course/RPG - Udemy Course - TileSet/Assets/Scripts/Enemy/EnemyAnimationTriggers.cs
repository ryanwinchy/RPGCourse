using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    void AnimationTrigger() => enemy.AnimationFinishTrigger();   //This is when we want to make an event in the anim that triggers some functionality in the code. But generic, can be used for anything.

    void AttackTrigger()   //Play on animation frame attack hits.
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);   //Get all colliders in attack sphere.

        foreach (Collider2D hit in colliders)             //for each collider in the radius.
        {
            if (hit.GetComponent<Player>() != null)       //if hit player, damage player.
            {

                PlayerStats _target = hit.GetComponent<PlayerStats>();

                enemy.stats.DoDamage(_target);        //Can call as entity script has ref to 'characterstats' script (stats) which has DoDamage func.
            }
        }
    }

    void SpecialAttackTrigger()   //like archer ranged attack.
    {
        enemy.AnimationSpecialAttackTrigger();
    }

    void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    void CloseCounterWindow() => enemy.CloseCounterAttackWindow();

}
