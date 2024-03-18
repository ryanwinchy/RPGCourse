using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum StatType { strength, agility, intelligence, vitality, damage, critChance, critPower, health, armour, evasion, magicResistance, fireDamage, iceDamage, lightningDamage }

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Unique Effect/Buff Effect")]          //Creates right click menu option. Create -> Data -> Item Effect

public class BuffEffect : UniqueItemEffect        //What stat to modify, by how much, for how long. One script to make many scriptable objects.
{

    PlayerStats playerStats;
    [SerializeField] StatType buffType;
    [SerializeField] int buffAmount;
    [SerializeField] float buffDuration;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());
    }

    Stat StatToModify()
    {
        if (buffType == StatType.strength) return playerStats.strength;       //Soooo much quicker doing this than script for each stat buff.
        else if (buffType == StatType.agility) return playerStats.agility;
        else if (buffType == StatType.intelligence) return playerStats.intelligence;
        else if (buffType == StatType.vitality) return playerStats.vitality;
        else if (buffType == StatType.damage) return playerStats.damage;
        else if (buffType == StatType.critChance) return playerStats.critChance;
        else if (buffType == StatType.critPower) return playerStats.critPower;
        else if (buffType == StatType.health) return playerStats.maxHealth;
        else if (buffType == StatType.armour) return playerStats.armour;
        else if (buffType == StatType.evasion) return playerStats.evasion;
        else if (buffType == StatType.magicResistance) return playerStats.magicResistance;
        else if (buffType == StatType.fireDamage) return playerStats.fireDamage;
        else if (buffType == StatType.iceDamage) return playerStats.iceDamage;
        else if (buffType == StatType.lightningDamage) return playerStats.lightningDamage;

        return null;   //If none found.


    }

}
