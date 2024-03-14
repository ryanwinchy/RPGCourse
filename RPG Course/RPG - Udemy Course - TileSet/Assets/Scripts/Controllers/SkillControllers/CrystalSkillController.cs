using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    Animator animator => GetComponent<Animator>();        //Instead of start function.
    CircleCollider2D circleCollider => GetComponent<CircleCollider2D>();
    Player player;
    
    float crystalDuration;

    bool canExplode;
    bool canMove;
    float moveSpeed;

    bool canGrow;
    float growSpeed = 5f;

    Transform closestEnemy;
    [SerializeField] LayerMask whatIsEnemy;

    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestEnemy, Player _player)
    {
        crystalDuration = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestEnemy = _closestEnemy;
        player = _player;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);   //Instead of whatIsEnemy layermask, can do for loop checking for enemy components as we did in other places.

        if (colliders.Length > 0 )
            closestEnemy = colliders[Random.Range(0, colliders.Length)].transform;  //get all colliders in radius, choose random one.
    }

    private void Update()
    {
        crystalDuration -= Time.deltaTime;

        if (crystalDuration < 0)
        {
            FinishCrystal();
        }

        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestEnemy.position) < 1f)
            {
                canMove = false;
                FinishCrystal();
            }
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);   //Lerp grows from one scale to another, at a given speed.
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            animator.SetTrigger("Explode");
        }
        else
            SelfDestruct();
    }

    void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);  //temp array of all colliders in attack circle when called.

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)      //if hit an enemy in attack circle.
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());        //access the players stats, DoMagicDamage function targeted at the hit enemy's stats.
        }
    }

    public void SelfDestruct() => Destroy(gameObject);
}
