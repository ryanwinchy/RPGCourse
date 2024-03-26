using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType { big , medium, small}  //Enum for which type of slime it is.

public class EnemySlime : Enemy
{

    [Header("Slime Specific")]
    [SerializeField] SlimeType slimeType;
    [SerializeField] int amtToCreateOnDeath;
    [SerializeField] GameObject createSlimePrefab;
    [SerializeField] Vector2 minCreationVelocity;  //When created should be pushed out a bit.
    [SerializeField] Vector2 maxCreationVelocity;

    #region States

    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }

    public SlimeAttackState attackState { get; private set; }

    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeadState deadState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);   //This is because slime sprite is facing the other way to skeleton.

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);    //The first this is passing enemyBase which it inherits from, the second this is passing enemySlime, this script.
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);  //battle state but just moving towards player, so needs move animation. It's basically an 'agro' state.
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SlimeStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SlimeDeadState(this, stateMachine, "Idle", this);
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

        if (slimeType == SlimeType.small)    //Small slimes dont create more.
            return;

        CreateSlimes(amtToCreateOnDeath, createSlimePrefab);
    }

    void CreateSlimes(int _amtOfSlimes, GameObject _slimePrefab)
    {
        for (int i = 0; i < _amtOfSlimes; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);

            newSlime.GetComponent<EnemySlime>().SetupSlime(facingDir);
        }
    }

    public void SetupSlime(int _facingDir)
    {
        if (_facingDir != facingDir)
            Flip();

        float xVelocity = Random.Range(minCreationVelocity.x, maxCreationVelocity.x);
        float yVelocity = Random.Range(minCreationVelocity.y, maxCreationVelocity.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * -facingDir, yVelocity); //Apply force on creation.

        Invoke("CancelKnockback", 1.5f);

    }

    void CancelKnockback() => isKnocked = false;

}
