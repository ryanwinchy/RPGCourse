using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerBlackholeState : PlayerState
{
    float flyTime = 0.4f;
    bool skillUsed;

    float defaultGravity;
    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravity = rb.gravityScale;

        skillUsed = false;
        stateTimer = flyTime;

        rb.gravityScale = 0;        //Because in this state, we want player to fly up slowly.
    }

    public override void Exit()   
    {
        base.Exit();

        player.rb.gravityScale = defaultGravity;
        player.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 15);        //Flys up when enters state.

        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -0.1f);     //Stays where flew to, comes down marginally.

            if (!skillUsed)
            {
                if(player.skill.blackhole.CanUseSkill())
                    skillUsed = true;
            }

        }

        if (player.skill.blackhole.BlackholeFinished())
            stateMachine.ChangeState(player.airState);


    }
}
