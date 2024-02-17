using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    string animatorBoolName;

    protected float xInput;  //Movement goes in player state base as all states need to know it.
    protected float yInput;

    protected float stateTimer;

    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animatorBoolName)     //Constructor when setting up specific states like move state , idle state.
    {
        this.player = _player;              //When setup state like move state, needs player so can do things like move it.
        this.stateMachine = _stateMachine;
        this.animatorBoolName = _animatorBoolName;
    }

    public virtual void Enter()        //Add object to shelf. Can perform actions when enter state, like dash.
    {
        player.animator.SetBool(animatorBoolName, true);    //Turn on animation for this state.
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void Update()      //Perform actions while in state, like movement.
    {

        stateTimer -= Time.deltaTime;   //Reduce timer every second. Player state, so all states inherited from here can access the timer.

        xInput = Input.GetAxisRaw("Horizontal");  //Raw input means no smoothing, just the raw input val.
        yInput = Input.GetAxisRaw("Vertical");

        player.animator.SetFloat("yVelocity", rb.velocity.y);   //So we pass y velocity to all states, as all states inherit from here. This animator field sets yVelocity to control fall / jump blend tree.
    }

    public virtual void Exit()     //Remove object from shelf.   Perform actions as exit state, like attack.
    {
        player.animator.SetBool(animatorBoolName, false);      //Turn off anim for this state.
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }


}
