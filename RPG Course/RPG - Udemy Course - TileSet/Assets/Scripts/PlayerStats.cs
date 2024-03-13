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

        player.DamageEffect();      //Damage visual effect.
    }

    protected override void Die()
    {
        base.Die();

        player.Die();        //accesses player, on player calls die func which just changes state to dead.
    }


}
