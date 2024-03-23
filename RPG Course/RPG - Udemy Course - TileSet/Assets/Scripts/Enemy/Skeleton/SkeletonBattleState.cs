using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    Transform player;
    EnemySkeleton enemy;
    int moveDirection;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)  //Wont attack when youre dead.
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())   //Checks raycast to see if player detected.
        {
            stateTimer = enemy.battleTime;                //When enemy detects player, timer resets and we start counting again.

            if ((enemy.IsPlayerDetected().distance < enemy.attackDistance) && canAttack())
            {
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        else   //Player not detected.
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)      //if state timer is over or player far away, go back to idle, so leaves battle state, no longer pursues player.
                stateMachine.ChangeState(enemy.idleState);
        }



        if (player.position.x > enemy.transform.position.x)       //If player to right.
            moveDirection = 1;
        else if (player.position.x < enemy.transform.position.x)   //Player to left.
            moveDirection = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDirection, rb.velocity.y);
    }

    bool canAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);    //So slighty different interval between attacks every time.
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

}
