using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    new public Camera camera { get; private set; }

    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttckDuration = 0.2f;


    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact;

    float defaultMoveSpeed;
    float defaultJumpForce;
    float defaultDashSpeed;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }


    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }    
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerDashState dashState { get; private set; }

    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }

    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }

    public PlayerBlackholeState blackholeState { get; private set; }
    public PlayerDeadState deadState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        camera = FindObjectOfType<Camera>();     //For use in aim state.
        
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword"); //Sending camera, as when aiming need camera to find mouse pos so can flip sprite when aiming.
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");

        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");

        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();          //Runs base start (entity), which gets ref to rb and animator.

        skill = SkillManager.instance;         //This means in other scripts, can just type player.skill instead of SkillManager.instance.
        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;

    }


    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();


        CheckForDashInput();

        if(Input.GetKeyDown(KeyCode.F))
        {
            skill.crystal.CanUseSkill();           //Check if cooldown finished, if has, use skill.
        }
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)      //Overrides from entity, as slowing player diff to slowing enemy. Base is empty so dont need it. Polymorph it instead.
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 -  _slowPercentage);

        Invoke("ReturnToDefaultSpeed", _slowDuration);
    }

    protected override void ReturnToDefaultSpeed()                           //Overrides from entity.
    {
        base.ReturnToDefaultSpeed();  //Call base as base resets animator to 1x speed.

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }


    public void AssignNewSword(GameObject newSword)      //When throw a sword, so can only have one at a time.
    {
        sword = newSword;
    }

    public void CatchSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }               

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;        

        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;




        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            
            stateMachine.ChangeState(dashState);
        }
    }


    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }


}
