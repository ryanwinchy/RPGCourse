using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class DeathBringerSpellController : MonoBehaviour
{
    [SerializeField] Transform check;
    [SerializeField] Vector2 boxSize;
    [SerializeField] LayerMask whatIsPlayer;

    CharacterStats stats;

    public void SetupSpell(CharacterStats _stats) => stats = _stats;

    void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);  //temp array of all colliders in attack circle when called.

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)      //if hit player.
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);   //So knocksback entity getting damaged the correct direction.

                stats.DoDamage(hit.GetComponent<CharacterStats>());

            }
        }
    }

    private void OnDrawGizmos() => Gizmos.DrawWireCube(check.position, boxSize); //Visual so we can see boxcollider we make above.

    void SelfDestroy() => Destroy(gameObject);

}
