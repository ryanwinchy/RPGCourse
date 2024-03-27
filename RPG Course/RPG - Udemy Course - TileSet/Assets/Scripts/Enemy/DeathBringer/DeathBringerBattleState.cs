using UnityEngine;

public class DeathBringerBattleState : EnemyState
{

    EnemyDeathBringer enemy;
    Transform player;
    int moveDirection;
    public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        //if (player.GetComponent<PlayerStats>().isDead)   //Wont attack when youre dead.
            //stateMachine.ChangeState(enemy.moveState);
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

            if ((enemy.IsPlayerDetected().distance < enemy.attackDistance))
            {
                if (canAttack())
                    stateMachine.ChangeState(enemy.attackState);
                else
                    stateMachine.ChangeState(enemy.idleState);
            }
        }




        if (player.position.x > enemy.transform.position.x)       //If player to right.
            moveDirection = 1;
        else if (player.position.x < enemy.transform.position.x)   //Player to left.
            moveDirection = -1;

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance - 0.2f)   //When close enough to attack, stop moving.  IsPlayerDetected returns a raycast (line) , so can access distance.
            return;           //Exit update loop for this frame.

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
