using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArcherBattleState : EnemyState
{

    Transform player;
    EnemyArcher enemy;
    int moveDirection;
    public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyArcher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

            if (enemy.IsPlayerDetected().distance < enemy.jumpTriggerDistance && CanJump())  //If archer can jump and close to player, jump
                stateMachine.ChangeState(enemy.jumpState);

            if ((enemy.IsPlayerDetected().distance < enemy.attackDistance) && canAttack())   //Else attack, if far distance (its an archer).
            {
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        else   //Player not detected.
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)      //if state timer is over or player far away, go back to idle, so leaves battle state, no longer pursues player.
                stateMachine.ChangeState(enemy.idleState);
        }

        FlipSprite();

    }

    private void FlipSprite()
    {
        if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1)       //If player to right.
            enemy.Flip();
        else if (player.position.x < enemy.transform.position.x && enemy.facingDir == 1)   //Player to left.
            enemy.Flip();
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
    
    bool CanJump()
    {
        if (enemy.GroundBehindCheck() == false || enemy.WallBehindCheck() == true)      //No ground behind, cannot jump.
            return false;

        if (Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
        {
            enemy.lastTimeJumped = Time.time;
            return true;
        }

        return false;
    }


}
