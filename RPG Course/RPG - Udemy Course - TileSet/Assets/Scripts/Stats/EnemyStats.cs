using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    Enemy enemy;
    ItemDrop dropSystem;

    public Stat currencyDropAmount;

    [Header("Level Details")]
    [SerializeField] int level;

    [Range(0f, 1f)]
    [SerializeField] float percentageModifier;


    protected override void Start()
    {
        currencyDropAmount.SetDefaultValue(100);

        ApplyLevelModifiers();

        base.Start();                //Before base start, because base.start updates the health, so we want it to do that based on the modified health.


        enemy = GetComponent<Enemy>();
        dropSystem = GetComponent<ItemDrop>();

    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);     //Dont really have to modify these for enemy. These stats should be 0 anyway.
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);            //Gives good control. But best for enemy, just modify stats manually to really fine tune.
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armour);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);

        Modify(currencyDropAmount);      // so higher level enemies drop more coins.

    }

    void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)   //Start at enemy level 1. Each level, enemy gets stronger with modifiers based on the given %.
        {
            float modifier = _stat.GetValue() * percentageModifier;    //Modifier is a set % of current stat.

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        PlayerManager.instance.currency += currencyDropAmount.GetValue();
        dropSystem.GenerateDrop();

        Destroy(gameObject, 5f);     //Destroy dead enemy after 5 seconds.
    }
}
