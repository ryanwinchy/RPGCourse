using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire Effect", menuName = "Data/Item Unique Effect/Ice and Fire")]

public class IceAndFireEffect : UniqueItemEffect
{
    [SerializeField] GameObject iceAndFirePrefab;
    [SerializeField] float xVelocity;

    public override void ExecuteEffect(Transform _spawnPos)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = player.primaryAttack.comboCounter == 2;

        if (thirdAttack)      //Trigger this effect only on third attack in sequence.
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _spawnPos.transform.position, player.transform.rotation); //As its a projectile, should match players rotation.
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);

            Destroy(newIceAndFire, 10);    //Destroy after 10 seconds.

        }

    }

}
