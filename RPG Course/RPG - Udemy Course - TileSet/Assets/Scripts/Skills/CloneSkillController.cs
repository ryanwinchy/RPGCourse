using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloneSkillController : MonoBehaviour    //This script is just settings for the clone skill. Like telling clone where to spawn, and cooldowns.
{

    SpriteRenderer spriteRenderer;
    Animator anim;
    [SerializeField] float fadeSpeed;


    float cloneTimer;
    [SerializeField] Transform attackCheck;
    [SerializeField] float attackCheckRadius = 0.8f;
    Transform closestEnemy;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, (spriteRenderer.color.a - (Time.deltaTime * fadeSpeed)));    //Fade out clone when timer ends.

            if (spriteRenderer.color.a < 0 )     //Once faded, delete game object.
                Destroy(gameObject);
        }
    }
    public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack)    //Clone duration could be setup in this script, but better to pass from clone skill so set vars all in skill manager.
    {
        if (canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));       //If can attack (clone attack ability unlocked), do a random attack out the 3.


        transform.position = newTransform.position;
        cloneTimer = cloneDuration;         //When clone setup, make timer equal to clone duration.


        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = - 1f;          //So after attack, immediately triggers end of timer so disappears.
    }

    private void AttackTrigger()     //Play on animation frame attack hits.
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);  //temp array of all colliders in attack circle when called.

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)      //if hit an enemy in attack circle.
                hit.GetComponent<Enemy>().Damage();         //call that enemies damage function.
        }
    }

    void FaceClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);   //In radius of 25 around the clone pick up all colliders.

        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in colliders)   //for each collider in the 25 radius.
        {
            if ((hit.GetComponent<Enemy>() != null))      //If its an enemy.
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);    //get distance from clone to enemy collider it found.

                if (distanceToEnemy < closestDistance)  //If distance to this enemy is less than current closest, then set closest var to the new closest's transform.
                {
                    closestDistance = distanceToEnemy;  //So loops until finds closest one.
                    closestEnemy = hit.transform;
                }   
                    

            }
        }

        if ((closestEnemy != null) && (transform.position.x > closestEnemy.position.x))
            transform.Rotate(0, 180, 0);


    }

    
}
