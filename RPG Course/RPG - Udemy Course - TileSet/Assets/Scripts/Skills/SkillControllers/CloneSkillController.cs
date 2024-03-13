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
    int facingDir = 1;

    bool canDuplicateClone;
    float chanceToDuplicate;


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

            if (spriteRenderer.color.a < 0)     //Once faded, delete game object.
                Destroy(gameObject);
        }
    }
    public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack, Vector3 offset, Transform _closestEnemy, bool _canDuplicateClone, float _chanceToDuplicate)    //Clone duration could be setup in this script, but better to pass from clone skill so set vars all in skill manager.
    {
        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;

        if (canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));       //If can attack (clone attack ability unlocked), do a random attack out the 3.


        transform.position = newTransform.position + offset;
        cloneTimer = cloneDuration;         //When clone setup, make timer equal to clone duration.


        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -1f;          //So after attack, immediately triggers end of timer so disappears.
    }

    private void AttackTrigger()     //Play on animation frame attack hits.
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);  //temp array of all colliders in attack circle when called.

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)       //if hit an enemy in attack circle.
            {
                hit.GetComponent<Enemy>().DamageEffect();         //call that enemies damage function.

                if (canDuplicateClone)         //When clone's attack hits, chance to make another clone.
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)          //This will be a stat. But this currently is a 35% chance to spawn clone.
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(1.5f * facingDir, 0));   //New clone facing opposite original clone. Can sandwich enemies.
                    }
                }
            }
        }
    }

    void FaceClosestTarget()
    {

        if (closestEnemy != null)
        {

            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;          //If flip, track facing dir for other functions.
                transform.Rotate(0, 180, 0);
            }
        }


    }


}
