using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyExplosiveController : MonoBehaviour
{
    Animator animator;
    CharacterStats stats;
    float growSpeed = 15;
    float maxSize = 6;
    float explosionRadius;

    bool canGrow = true;

    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);    //Lerp grows from one size to max size overtime. Gets slower at end, like curve.
        }

        if (maxSize - transform.localScale.x < 0.5f)
        {
            canGrow = false;
            animator.SetTrigger("Explode");
        }


    }

    public void SetupExplosive(CharacterStats _stats, float _growSpeed, float _maxSize, float _explosionRadius)
    {
        animator = GetComponent<Animator>();
        stats = _stats;
        growSpeed = _growSpeed;
        maxSize = _maxSize;
        explosionRadius = _explosionRadius;
    }

    void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);  //temp array of all colliders in attack circle when called.

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<CharacterStats>() != null)      //if hit any character (enemy or player)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);   //So knocksback entity getting damaged the correct direction.

               stats.DoDamage(hit.GetComponent<CharacterStats>());

            }
        }
    }

    void SelfDestroy() => Destroy(gameObject);

}
