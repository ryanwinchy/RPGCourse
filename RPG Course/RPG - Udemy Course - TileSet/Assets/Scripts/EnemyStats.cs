using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    Enemy enemy;
    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        enemy.DamageEffect();   //Damage visual effect.
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
    }
}
