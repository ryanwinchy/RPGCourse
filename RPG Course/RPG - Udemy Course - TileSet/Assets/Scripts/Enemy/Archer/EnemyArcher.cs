using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : Enemy
{

    [Header("Archer Specific")]
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float arrowSpeed;
    //[SerializeField] int arrowDamage;

    [SerializeField] public Vector2 jumpVelocity;
    public float jumpCooldown;
    public float jumpTriggerDistance;
    [HideInInspector] public float lastTimeJumped;

    [Header("Additional Collision Checks")]
    [SerializeField] Transform groundBehindCheck;
    [SerializeField] Vector2 groundBehindCheckSize;


    #region States

    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherBattleState battleState { get; private set; }

    public ArcherAttackState attackState { get; private set; }

    public ArcherStunnedState stunnedState { get; private set; }
    public ArcherDeadState deadState { get; private set; }

    public ArcherJumpState jumpState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);    //The first this is passing enemyBase which it inherits from, the second this is passing enemyArcher, this script.
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        battleState = new ArcherBattleState(this, stateMachine, "Idle", this);  //battle state but just moving towards player, so needs move animation. It's basically an 'agro' state.
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        stunnedState = new ArcherStunnedState(this, stateMachine, "Stunned", this);
        deadState = new ArcherDeadState(this, stateMachine, "Idle", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);
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

    public override void AnimationSpecialAttackTrigger()
    {
        Debug.Log("Shoot arrow");
        GameObject newArrow = Instantiate(arrowPrefab, attackCheck.position, Quaternion.identity);     //Spawn arrow at attackcheck pos.

        newArrow.GetComponent<ArrowController>().SetupArrow(arrowSpeed * facingDir, stats);
    }

    //Quite clever, we make box (ground check) way behind archer, where she IS going to jump. If this box isnt touching ground, she wont jump there.
    public bool GroundBehindCheck() => Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, 0, whatIsGround);
    public bool WallBehindCheck() => Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDir, wallCheckDistance + 2, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindCheckSize);  //This is just visual to show the collider we made.
    }

}
