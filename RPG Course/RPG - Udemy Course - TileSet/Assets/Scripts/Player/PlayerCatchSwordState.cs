using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{

    Transform sword;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;       //Get sword transform from player.

        if (player.transform.position.x > sword.position.x && player.facingDir == 1)       //If catch sword on left but facing right, flip.
            player.Flip();
        else if (player.transform.position.x < sword.position.x && player.facingDir == -1) //If catch sword on right but facing left, flip.
            player.Flip();

        rb.velocity = new Vector2(player.swordReturnImpact * - player.facingDir, rb.velocity.y);  //When we enter sword catch, apply a bit of negative knockback.
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.1f);      //Busy for locks movement. We want this when exiting catch state so forced to feel above knockback.
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
