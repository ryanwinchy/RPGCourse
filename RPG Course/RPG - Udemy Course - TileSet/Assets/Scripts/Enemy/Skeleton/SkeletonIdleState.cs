using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;     //Gets timer from one of its bases.
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.PlaySFX(24, enemy.transform);  //Play sound effect from enemy.
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)                                   //At end of idle timer (eg 1.5 seconds), changes to move state.
            stateMachine.ChangeState(enemy.moveState);
    }


}
