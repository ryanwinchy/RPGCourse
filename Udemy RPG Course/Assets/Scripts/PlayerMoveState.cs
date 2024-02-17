using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState        //Inherits from playergroundedstate which inherits from playerstate.
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animatorBoolName) : base(_player, _stateMachine, _animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();         //This simply means run the base enter method it has inherited. So runs exactly the same.
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();


        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);   //Sets movement.

        if (xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);        //Stop moving if touch wall.
    }

}
