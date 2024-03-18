using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SwordSkillController : MonoBehaviour     //Attached to sword game object, sets up skill settings.
{

    Animator anim;
    Rigidbody2D rb;
    CircleCollider2D circleCollider;
    Player player;

    bool canRotate = true;
    bool isReturning;

    float freezeTimeDuration;
    float returnSpeed = 12f;

    [Header("Pierce Info")]    //Unlockable skill.
    int pierceAmount;     //how many enemies this ability can pierce through.

    [Header("Bounce Info")]
    float bounceSpeed = 20f;
    bool isBouncing;        //Unlockable skill where thrown sword can bounce between enemies within a range.
    int numBounces;
    List<Transform> enemyTargets = new List<Transform>();
    int targetIndex = 0;

    [Header("Spin Info")]    //Unlockable skill.
    float maxTravelDistance;
    float spinDuration;
    float spinTimer;
    bool wasStopped;
    bool isSpinning;


    float hitTimer;
    float hitCooldown;

    float spinDirection;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

    }

    void DestroyMe() => Destroy(gameObject);

    public void SetupSword(Vector2 dir, float gravityScale, Player player, float freezeTimeDuration, float returnSpeed)        //Basically start function.
    {
        this.player = player;           //Instead of using _player. Probably choose one or the other and be consistent.
        this.freezeTimeDuration = freezeTimeDuration;
        this.returnSpeed = returnSpeed;

        rb.velocity = dir;
        rb.gravityScale = gravityScale;

        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);     //So spins direction it is going.

        Invoke("DestroyMe", 7f);          //This means after 7 seconds sword is destroyed. Good for bugs if sword falls off map, can get it again instead of losing it.
    }

    public void SetupBounce(bool _isBouncing, int _numBounces, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        numBounces = _numBounces;
        bounceSpeed = _bounceSpeed;
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;

    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetBool("Rotation", false);

        transform.parent = null;
        isReturning = true;     //Then in update will return to player.
    }
    private void Update()
    {
        if (canRotate)          //If can spin, spin in air. When sword is  stuck in something, canRotate is false.
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);   //Move from pos to player.

            if (Vector2.Distance(transform.position, player.transform.position) < 1)   //When gets close clear sword.
                player.CatchSword();     //Destroy sword and enter catch state.
        }

        BounceLogic();
        SpinLogic();


    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                //Instead of the spinning sword stopping, it will slowly move in direction its spinning. Cool effect. Allows to zone enemies.
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;         //Every 0.35 seconds (3 attacks a second) , do enemy check and damage enemies within range.

                if (hitTimer < 0)
                {

                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);        // create circle around sword radius of 1, array of all their colliders.

                    foreach (Collider2D hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)          //If enemy collider, damage that enemy.
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }

            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0)     //If bounce unlocked and have found targets.
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);   //Move towards first in list.

            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < 0.1f)
            {
                SwordSkillDamage(enemyTargets[targetIndex].GetComponent<Enemy>());     //Damage and freeze the enemy it hit before bouncing to next target.


                targetIndex++;
                numBounces--;

                if (numBounces <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTargets.Count)   //If index bigger than num enemies, go back to first target.
                    targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)       //Whenever anything enters collider of sword, this func called. So basically when sword sticks to something.
    {

        if (isReturning)      //If sword is returning, it won't get stuck / collide as below.
            return;

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);

        }


        SetupBounceTargets(collision);

        StuckInto(collision);   //Moved to own function and sent collision info, same as having it in here.
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        player.stats.DoDamage(enemy.GetComponent<CharacterStats>());
        enemy.FreezeTimeFor(freezeTimeDuration);

        ItemDataEquipment equippedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);   //If have amulet, do the effect also.

        if (equippedAmulet != null)
            equippedAmulet.ExecuteItemEffect(enemy.transform);
    }

    private void SetupBounceTargets(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)     //if hit enemy.
        {

            if (isBouncing && enemyTargets.Count <= 0)        //if is bouncing unlocked, and no enemies in list currently.
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);        // create circle around sword radius of 10, array of all their colliders.

                foreach (Collider2D hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTargets.Add(hit.transform);      //Adds the transform of enemy within the radius to the list of targets.
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)    //Basically pierce through enemy instead of sticking.
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)      //So wont get stuck on enemies when is spinning. calling stop when spinning means it will stop when hits an enemy. Perhaps too easy?
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        circleCollider.enabled = false;    //disable collider.

        rb.isKinematic = true;      //Change from dynamic to kinematic.
        rb.constraints = RigidbodyConstraints2D.FreezeAll;        //Freeze all constraints, same as inspector.

        if (isBouncing && enemyTargets.Count > 0)      //If bouncing return, do not end animation or make child of collided object.   > 0 means if hit ground or only one enemy, can still get stuck.
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;      //Put this game object as child as what it collided with.
    }




}
