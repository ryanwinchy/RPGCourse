using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyDeadState : EnemyState
{
    EnemyShady enemy;
    public ShadyDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyShady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }


    public override void Enter()
    {
        base.Enter();


        stateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)                //Called from animation event.
            enemy.SelfDestroy();

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 10);          //Goes up initially, then falls.
        }
    }



}
