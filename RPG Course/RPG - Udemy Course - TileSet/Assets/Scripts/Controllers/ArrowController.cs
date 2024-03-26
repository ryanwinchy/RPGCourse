using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] string targetLayerName = "Player";

    [SerializeField] float xVelocity;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] bool canMove = true;
    [SerializeField] bool flipped;      //if counter, it flips.

    CharacterStats stats;


    private void Update()
    {
        if (!canMove)
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);       //because y affected by gravity.
    }

    public void SetupArrow(float _speed, CharacterStats _stats)
    {
        xVelocity = _speed;
        stats = _stats;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

                                                         //if hits target layer.
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))     //Whichever layer arrow is targeting, so player. Could probs just do game object. get component player.
        {
            stats.DoDamage(collision.GetComponent<CharacterStats>());

            StuckInto(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))  //Also gets stuck in ground as well as the target layer. 
            StuckInto(collision);


    }

    private void StuckInto(Collider2D collision)
    {
        GetComponentInChildren<ParticleSystem>().Stop();  //Stop particle FX when stuck into something.
        GetComponent<CapsuleCollider2D>().enabled = false;    //Once its stuck, turn off collider so dont get continually damaged.

        canMove = false;           //When collides with something, freezes.
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;     //Put arrow as child of thing collided with.

        Destroy(gameObject, Random.Range(5, 14));      //Destroy arrow when is stuck in ground or someone, with some variation.
    }

    public void FlipArrow()      //When player counters it.
    {
        if (flipped)     //Can only flip once.
            return;

        xVelocity = xVelocity * -1;
        flipped = true;
        transform.Rotate(0, 180, 0);

        targetLayerName = "Enemy";    //Player counted, now target layer is enemy!
    }


}
