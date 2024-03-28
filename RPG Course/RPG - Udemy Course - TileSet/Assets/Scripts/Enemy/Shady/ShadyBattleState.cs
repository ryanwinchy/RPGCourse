using Unity.VisualScripting;
using UnityEngine;

public class ShadyBattleState : EnemyState
{
    Transform player;
    EnemyShady enemy;
    int moveDirection;

    float defaultSpeed;
    public ShadyBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyShady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        defaultSpeed = enemy.moveSpeed;   //Store default.

        enemy.moveSpeed = enemy.battleStateMoveSpeed;   //Speed up while in this state (aggroed).

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)  //Wont attack when youre dead.
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.moveSpeed = defaultSpeed;    //Go back to normal speed.
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())   //Checks raycast to see if player detected.
        {
            stateTimer = enemy.battleTime;                //When enemy detects player, timer resets and we start counting again.

            if ((enemy.IsPlayerDetected().distance < enemy.attackDistance))
                enemy.stats.KillEntity();      //This enters dead state which triggers explosion , drops items and currency.

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

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())           //Whenever gets to edge, flips then goes to idle state for idle time.
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
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
