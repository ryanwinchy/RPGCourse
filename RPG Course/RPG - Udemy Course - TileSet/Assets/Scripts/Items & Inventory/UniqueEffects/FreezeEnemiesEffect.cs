using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Enemies Effect", menuName = "Data/Item Unique Effect/Freeze Enemies")]          //Creates right click menu option. Create -> Data -> Item Effect

public class FreezeEnemiesEffect : UniqueItemEffect
{

    [SerializeField] float duration;

    public override void ExecuteEffect(Transform _transform)  //We send in player transform, so any enemy within 2 of player.
    {

        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * 0.1f)    //Only freeze when health below 10%.
            return;

        if (!Inventory.instance.CanUseArmour())         //if cant use armour (its on cooldown), return.
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 1);  //temp array of all colliders in circle when called.

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)      //if hit an enemy in attack circle.
            {
                hit.GetComponent<Enemy>().FreezeTimeFor(duration);   //Could simply the null check with hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);

            }
        }
    }
}
