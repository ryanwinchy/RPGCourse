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
            if (hit.GetComponent<Enemy>() != null)      //if hit an enemy in attack circle.
                hit.GetComponent<Enemy>().Damage();         //call that enemies damage function.
        }
    }
}
