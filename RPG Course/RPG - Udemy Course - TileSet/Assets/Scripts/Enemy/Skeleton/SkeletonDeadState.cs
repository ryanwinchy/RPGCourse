using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{

    EnemySkeleton enemy;
    public SkeletonDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);    //Play last anim it played before death.
        enemy.anim.speed = 0;           //Stop animation.
        enemy.capsuleCollider.enabled = false;         //Falls thru level.

        stateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2 (0, 10);          //Goes up initially, then falls.
        }
    }
}
