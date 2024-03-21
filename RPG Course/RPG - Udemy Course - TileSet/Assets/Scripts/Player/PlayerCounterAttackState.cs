using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{

    bool canCreateClone;
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;
        stateTimer = player.counterAttckDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);     //When gets into counter attack state, set successful to false for now.
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);   //Get all colliders in attack sphere.

        foreach (Collider2D hit in colliders)             //for each collider in the radius.
        {
            if ((hit.GetComponent<Enemy>() != null) && (hit.GetComponent<Enemy>().CanBeStunned()))      //if hit enemy, check if enemy can be stunned (has to be right timing window). Can be stunned changes enemy to stunned state.
            {
                stateTimer = 100f;     //Just an arbitrary big num, so doesnt exit state because of timer if hits.
                player.anim.SetBool("SuccessfulCounterAttack", true);     //Make anim go to successful counter. On this anim, we have an event to make triggerCalled = true, so we exit to idle below.

                player.skill.parry.UseSkill();    //Uses skill to restore health on parry.

                if (canCreateClone)
                {
                    canCreateClone = false;       //So if you counter two at once, only creates one clone.
                    player.skill.parry.MakeMirageOnParry(hit.transform);
                }

            }
        }

        if (stateTimer < 0 || triggerCalled)        //If counter was not successful, exit state based on timer of counter attempt state (this).
            stateMachine.ChangeState(player.idleState);

    }




}
