using UnityEngine;

public class ShockStrikeController : MonoBehaviour
{
    [SerializeField] CharacterStats targetStats;
    [SerializeField] float speed;
    int damage;

    Animator animator;
    bool trigerred;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    void Update()
    {

        if (!targetStats)
            return;
        
        if (trigerred)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);   //Move towards pos of target.
        transform.right = transform.position - targetStats.transform.position;          //Face target.

        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {
            animator.transform.localRotation = Quaternion.identity;   //Reset rotation to 0.
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);          //make bigger, looks impactful.

            animator.transform.localPosition = new Vector3(0, 0.3f);     //Offset. When lightning animates, raise 0.3 up so strikes floor level.

            Invoke("DamageAndSelfDestroy", 0.2f);        //Delays the actual damage by 0.2f. More in line with animation of when lightning strikes.

            trigerred = true;
            animator.SetTrigger("Hit");
        }
    }

    void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, 0.4f);     //destroy after 0.4 second delay/

    }


}
