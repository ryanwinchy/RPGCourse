using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroundedState : EnemyState
{

    protected Transform player;
    protected EnemySlime enemy;
    public SlimeGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //If sees player, or if distance between player and him <2. So if I sneak behind.
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.aggroDistance)            //We made this grounded state, so idle and move can inherit from it and not repeat code. We want skele to go to battle state when sees
            stateMachine.ChangeState(enemy.battleState);          //player both in move or idle state, so we made this parent so can inherit instead of copy pasting code.
    }
}
