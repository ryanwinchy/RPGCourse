using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.dash.CloneOnDash();                //Whenever we enter dash, a clown will spawn (if unlocked). 

        stateTimer = player.dashDuration;


        player.stats.MakeInvincible(true);    //i frames while in this state.
        
        
    }

    public override void Exit()
    {
        base.Exit();

        player.skill.dash.CloneOnArrival();                  //Create clone on exit if unlocked.   
        player.SetVelocity(0, rb.velocity.y);

        player.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
        
        player.fx.CreateDashTrail();  //Creates trail when dashing. Function checks for cooldown itself.

    }
}
