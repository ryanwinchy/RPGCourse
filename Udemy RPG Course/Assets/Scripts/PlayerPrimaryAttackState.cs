using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    int comboCounter;

    float lastTimeAttacked;
    float comboWindow = 1f;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animatorBoolName) : base(_player, _stateMachine, _animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time > lastTimeAttacked + comboWindow)      
            comboCounter = 0;

        player.animator.SetInteger("ComboCounter", comboCounter);


        float attackDir = player.facingDir;

        if (xInput!= 0)                     //This lets you change attack dir between attacks, more control.
            attackDir = xInput;


        player.SetVelocity(player.attackMovements[comboCounter].x * attackDir, player.attackMovements[comboCounter].y);   //Set movement in inspector for each attack in combo.

        stateTimer = 0.1f;    //Tiny window, means player can move for 0.1f seconds after attacking so tiny bit of feedback before you stop.
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.15f);       //On exit of attack state, change busy to true for 0.15 seconds. Then can lock movement in this split second between attacks (as goes to idle for split second between).
                                                          //Basically can not move for 0.15 secs after attack.
        comboCounter++;
        lastTimeAttacked = Time.time;   //Sets it to the time of the attack.
    }

    public override void Update()
    {
        base.Update();
         
        if (stateTimer < 0)                     //After 0.15f timer (just after press attack), stop moving the player.
            player.ZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }



}
