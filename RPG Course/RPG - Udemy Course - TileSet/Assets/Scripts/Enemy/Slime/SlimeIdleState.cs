using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inherits from grounded, as needs to do everything grounded does - move and search for player.
public class SlimeIdleState : SlimeGroundedState
{
    public SlimeIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
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


    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)                                   //At end of idle timer (eg 1.5 seconds), changes to move state.
            stateMachine.ChangeState(enemy.moveState);
    }


}
