using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState    //Inherits from playergroundedstate which inherits from playerstate.
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animatorBoolName) : base(_player, _stateMachine, _animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.ZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput == player.facingDir && player.IsWallDetected())  //This is so walking anim doesn't play when touching wall.
            return;

        if (xInput != 0 && !player.isBusy)        //If movement input, change to move state. Also checks we're not in split second between attacks when player is busy.
            stateMachine.ChangeState(player.moveState);

    }
}
