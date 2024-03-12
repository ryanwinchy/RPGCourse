using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{


    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.sword.DotsActive(true);      //Switch on aim dots while in this state.

        
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.2f);      //Busy for locks movement. We want this when exiting aim so cant drift.
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();        //When you aim, you have no velocity. Without this, you can slide into the state.

        if (Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idleState);

        
        Vector2 mousePosition = player.camera.ScreenToWorldPoint(Input.mousePosition);

        if (player.transform.position.x > mousePosition.x && player.facingDir == 1)       //If aim mouse on left but facing right, flip.
            player.Flip();
        else if (player.transform.position.x < mousePosition.x && player.facingDir == -1) //If aim mouse on right but facing left, flip.
            player.Flip();
    }
}
