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

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;   //When die, your currency goes to your body for pickup.
        PlayerManager.instance.currency = 0;                //Current currency on you goes to 0.

        GetComponent<PlayerItemDrop>()?.GenerateDrop();    //checks player item drop isnt null, if not, runs generate drop.
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        if (_damage > GetMaxHealthValue() * 0.3f)      //if enemy does huge damage, 30% of our max. Do big knockback and special audio.
        {
            player.SetKnockbackPower(new Vector2 (10, 6));
            player.fx.ScreenShake(player.fx.shakeHighDamage);

            int randomSound = Random.Range(34, 35);             //As there are two girl screaming effects, this will vary it.
            AudioManager.instance.PlaySFX(randomSound, null);
        }

        ItemDataEquipment currentArmour = Inventory.instance.GetEquipment(EquipmentType.Armour);

        if (currentArmour != null)     //if your armour has an effect, execute it when take damage. This is applying the modifier loads of times, needs cleanup.
        {
            currentArmour.ExecuteItemEffect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        Debug.Log("Player avoided attack");
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats, float _cloneDamageMultiplier)
    {
        if (TargetCanAvoidAttack(_targetStats))   //Try evasion. If works, exit.
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (_cloneDamageMultiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * _cloneDamageMultiplier);

        totalDamage = CheckTargetArmour(_targetStats, totalDamage);

        if (CanCrit())
        {
            Debug.Log("Crit hit!");
            totalDamage = CalculateCriticalDamage(totalDamage);
        }


        _targetStats.TakeDamage(totalDamage);            // Do physical damage.

        DoMagicalDamage(_targetStats);            //Do magic damage. Remove this if dont want to apply magic damage on primary atk.
    }

}
