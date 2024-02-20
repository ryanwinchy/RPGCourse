using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
    EnemySkeleton enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemySkeleton>();
    }

    void AnimationTrigger() => enemy.AnimationFinishTrigger();


}
