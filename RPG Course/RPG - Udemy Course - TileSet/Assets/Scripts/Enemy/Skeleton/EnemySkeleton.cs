using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy                              //Inherits from entity and enemy, so has state machine setup (enemy) and rb, anim references, movement, collisions (entity).
{                                                                //This child script is specific to skeleton, to set up skeleton states and initialize idle state.

    #region States

    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }

    public SkeletonAttackState attackState { get; private set; }

    public SkeletonStunnedState stunnedState { get; private set; }
    public SkeletonDeadState deadState { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();

        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);    //The first this is passing enemyBase which it inherits from, the second this is passing enemyskeleton, this script.
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "Move", this);  //battle state but just moving towards player, so needs move animation. It's basically an 'agro' state.
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SkeletonDeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }



    public override bool CanBeStunned()
    {
       if (base.CanBeStunned())       //Runs base check of can be stunned, true or false.
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
       return false;
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

}
