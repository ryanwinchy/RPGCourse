using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{


    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }


    public SpriteRenderer spriteRenderer { get; private set; }

    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D capsuleCollider { get; private set; }

    #endregion

    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackPower = new Vector2 (7,12);
    [SerializeField] protected Vector2 knockbackOffset = new Vector2 (0.5f,2);
    [SerializeField] protected float knockbackDuration = 0.07f;
    protected bool isKnocked;

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius = 1.2f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 1;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = 0.8f;
    [SerializeField] protected LayerMask whatIsGround;

    public int knockbackDir { get; private set; }
    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public System.Action OnFlipped;        //Flip event (called an action, a delegate that's void). Can send other delegates if want to send info about event.

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)   //Override this, different for player and enemies with their own speed stats.
    {

    }

    protected virtual void ReturnToDefaultSpeed() => anim.speed = 1;

    public virtual void DamageImpact() => StartCoroutine("HitKnockback");

    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)   //Damage coming from right, so knockback to left.
            knockbackDir = -1;
        else if (_damageDirection.position.x < transform.position.x)   //Damage from left, knockback to right.
            knockbackDir = 1;
    }


    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        
        float xOffset = Random.Range(knockbackOffset.x, knockbackOffset.y);      //Random between first and second value in the var (thats why we used vector 2). doesnt mean x and y axis.


        rb.velocity = new Vector2((knockbackPower.x + xOffset) * knockbackDir, knockbackPower.y);    //the offset is to add some randomness to the knockback. Can set to 0 on enemies we dont want that for.

        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;       //This bool blocks movement in set velocity function.
        SetupZeroKnockbackPower();      //only for the player, thats why its on player script not enemy. Unless heavy enemy like a troll, maybe only knockback on crit.
    }

    public void SetKnockbackPower(Vector2 _knockbackPower) => knockbackPower = _knockbackPower;

    protected virtual void SetupZeroKnockbackPower()      //Should be abstract? But then means children HAVE to override.
    {

    }


    #region Velocity
    public void SetZeroVelocity()
    {
        if (isKnocked)    //If knocked currently, do not move.
            return;

        rb.velocity = new Vector2(0, 0);
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {

        if (isKnocked)    //If knocked currently, do not move.
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    #endregion

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);    //Checks ground layer, casts line matching gizmo line.
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()      //Draws line down to visualize the ground check, deawns line right to visualize wall check.
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);    //Visualize attack radius.
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (OnFlipped != null)
            OnFlipped();      //Fires OnFlipped event.
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }

    public virtual void SetupDefaultFacingDir(int _direction)
    {
        facingDir = _direction;

        if (facingDir == -1)
            facingRight = false;
    }

    #endregion




    public virtual void Die()      //Only for override.
    {

    }

}
