using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathBringer : Enemy
{
    public bool bossFightBegun;

    [Header("Spell Cast Details")]
    [SerializeField] GameObject spellPrefab;
    public int amtOfSpells;
    public float spellCooldown;

    public float lastTimeCast;
    [SerializeField] float spellCastCooldown;


    [Header("Teleport Details")]
    [SerializeField] BoxCollider2D arena;        //Arena he can teleport within.
    [SerializeField] Vector2 surroundingCheckSize;       //Small box around him so can check teleporting to an open space not inside a wall.
    public float chanceToTeleport;
    public float defaultChanceToTeleport = 25;




    #region States

    public DeathBringerIdleState idleState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }
    public DeathBringerBattleState battleState { get; private set; }

    public DeathBringerAttackState attackState { get; private set; }

    public DeathBringerSpellCastState spellCastState { get; private set; }
    public DeathBringerDeadState deadState { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);         //As this sprite was drawn other way.

        idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);    //The first this is passing enemyBase which it inherits from, the second this is passing enemyDeathBringer, this script.
        teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);
        battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);  //battle state but just moving towards player, so needs move animation. It's basically an 'agro' state.
        attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);
        spellCastState = new DeathBringerSpellCastState(this, stateMachine, "SpellCast", this);
        deadState = new DeathBringerDeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }



    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    public void CastSpell()
    {
        Player player = PlayerManager.instance.player;

        float xOffset = 0;

        if (player.rb.velocity.x != 0)        //if moving, offset the spell spawn pos by 3 as prediction.
            xOffset = player.facingDir * 3;

        Vector3 spellPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + 1.5f);  //Place spell ahead of you , looks like prediction.



        GameObject newSpell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);

        newSpell.GetComponent<DeathBringerSpellController>().SetupSpell(stats);
    }

    public void GoToRandomPosition()
    {
        float x = Random.Range(arena.bounds.min.x +  3, arena.bounds.max.x - 3);    //Random spot within arena we set, but not right at edge.
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        transform.position = new Vector3(x, y);  //Initial pos. Then line below adjusts it.
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (capsuleCollider.size.y / 2));  //Second part finds distance to ground, then puts it just a bit above the ground (half of collider).
   
        if (!GroundBelow() || SomethingIsAround())          //If pos found not suitable.
        {
            Debug.Log("Looking for new position...");
            GoToRandomPosition();
        }

    }

    //Cast ray (line) and box for collision detections.
    RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 130, whatIsGround);
    bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);

    protected override void OnDrawGizmos()   //Draw gizmos so we have a visual in the editor.
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);

    }

    public bool CanTeleport()
    {
        if (Random.Range(0, 100) <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }

        return false;
    }

    public bool CanDoSpellCast()
    {
        if (Time.time >= lastTimeCast + spellCastCooldown)
        {
            return true;
        }

        return false;
    }



}
