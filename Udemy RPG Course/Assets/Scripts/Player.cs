using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public bool isBusy {  get; private set; }

    [Header("Attack Details")]
    public Vector2[] attackMovements;

    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce = 12f;

    [Header("Dash Info")]
    [SerializeField] float dashCooldown;
    [SerializeField] float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {  get; private set; }

    [Header("Collision Info")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Transform wallCheck;
    [SerializeField] float wallCheckDistance;

    public int facingDir { get; private set; } = 1;
    bool facingRight = true;

    #region Components
    public Animator animator { get; private set; }
    public Rigidbody2D rb {  get; private set; }
    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }   //States.
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }

    #endregion




    private void Awake()
    {
        stateMachine = new PlayerStateMachine();    //Create new state machine script when this script is awoken (it should go on a game object).

        idleState = new PlayerIdleState(this, stateMachine, "Idle");     //Create states. this means it sends this player script.
        moveState = new PlayerMoveState(this, stateMachine, "Moving");
        jumpState = new PlayerJumpState(this, stateMachine, "Jumping");
        airState = new PlayerAirState(this, stateMachine, "Jumping");   //as air state same anim as jump. Looks same.
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
    }

    void Start()
    {

        animator = GetComponentInChildren<Animator>();        //Gets animator component from child (animator is on child in this case.
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState);           //Initialize first state, idle to start.

    }

    void Update()      //This is the only player class inheriting from monobehaviour, so it has update(). We then call statemachine update thru this.
    {
        stateMachine.currentState.Update();

        CheckForDashInput();

    }

    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();


    void CheckForDashInput()       //This in player instead of grounded state, as behaviour is an exception. You should be able to dash to dodge damage from any state, interupting attack / jump.
    {
        if (IsWallDetected())   //Cant dash while on wall.
            return;

        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;

            dashDir = Input.GetAxisRaw("Horizontal");   //Dash direction takes input when dash pressed.

            if (dashDir == 0)       //If no input, dash direction you were already facing.
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }

    #region Velocity
    public void ZeroVelocity() => rb.velocity = new Vector2 (0, 0);
    public void SetVelocity(float _xVelocity, float _yVelocity)   //USe this function to move character.
    {
        rb.velocity = new Vector2 (_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Collision
    private void OnDrawGizmos()    //Draws lines for collision checks. I like this way of doing it! As makes checking ground and walls and no walls easy. Like 'eyes' of a game object. Good for enemy LOS also.
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));  //Draws line from ground check pos to ground check - distance.
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));  //Draws from wallcheck pos to wallcheck + distance (right).
    }  //these lines are visual, to mirror the raycast.

    
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround); //Starting from ground check pos, cast down distance amt, looking for ground layer.
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    #endregion

    #region Flip
    void Flip()    //Must be public so all states can access.
    {
        facingDir *= -1;
        facingRight = !facingRight;   //Flips it.

        transform.Rotate(0, 180, 0);   //Flips sprite 180 deg.
    }

    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)       //_x is xvelocity. If positive (right) and facing left, flip.
            Flip();
        else if (_x < 0 && facingRight)   //If velocity - (left) and facing right, flip.
            Flip();
    }

    #endregion
}
