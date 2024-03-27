using UnityEngine;

public class EnemyShady : Enemy
{
    [Header("Shady Specific")]
    public float battleStateMoveSpeed;

    [SerializeField] GameObject explosivePrefab;
    [SerializeField] float growSpeed;
    [SerializeField] float maxSize;


    #region States

    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyBattleState battleState { get; private set; }
    public ShadyStunnedState stunnedState { get; private set; }
    public ShadyDeadState deadState { get; private set; }


    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new ShadyIdleState(this, stateMachine, "Idle", this);    //The first this is passing enemyBase which it inherits from, the second this is passing enemyShady, this script.
        moveState = new ShadyMoveState(this, stateMachine, "Move", this);
        battleState = new ShadyBattleState(this, stateMachine, "MoveFast", this);  //battle state but just moving towards player, so needs move animation. It's basically an 'agro' state.
        stunnedState = new ShadyStunnedState(this, stateMachine, "Stunned", this);
        deadState = new ShadyDeadState(this, stateMachine, "Dead", this);
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

    protected override void Update()
    {
        base.Update();
    }

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newExplosive = Instantiate(explosivePrefab, attackCheck.position, Quaternion.identity);

        newExplosive.GetComponent<ShadyExplosiveController>().SetupExplosive(stats, growSpeed, maxSize, attackCheckRadius);

        capsuleCollider.enabled = false;  //So doesnt fall thru ground.
        rb.gravityScale = 0;
    }

    public void SelfDestroy() => Destroy(gameObject);

}
