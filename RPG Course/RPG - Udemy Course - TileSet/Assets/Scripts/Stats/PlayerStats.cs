using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{

    Player player;
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        player.Die();        //accesses player, on player calls die func which just changes state to dead.

        GetComponent<PlayerItemDrop>()?.GenerateDrop();    //checks player item drop isnt null, if not, runs generate drop.
    }


}
