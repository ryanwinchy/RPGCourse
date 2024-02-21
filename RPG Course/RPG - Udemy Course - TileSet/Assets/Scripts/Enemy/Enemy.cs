using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity                //Basically sets up new state machine on enemy game object.
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stunned Info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;  //Red square when enemy attacks so know when to time counter.

    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;   //How long he should be in battle (agro) state.

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine { get; private set; }


    protected override void Awake()
    {
       base.Awake();
       stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50f, whatIsPlayer);      //Same as ground / wall check but returns raycast hit for a lot of info. Like collider, game object of what it detects.
                                                                                                                                                //Distance of 50 is enemy line of sight.

    protected override void OnDrawGizmos()    //Overriding from base method to make it yellow and draw another line for the attack.
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));   //Draw this one from enemy centre pos.


    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

}            
