using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType { Weapon, Armour, Amulet, Flask }


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]          //Creates right click menu option. Create -> Data -> Item

public class ItemDataEquipment : ItemData       //Exactly the same as itemData scriptable object, but has an equipment type, so can work out which slot is which, and equip only one sword for eg.
{                                                //Equipment is also equipped, so modifies ints.

    public EquipmentType equipmentType;

    [Header("Major Stats")]
    public int strength;          //1 pt increase damage by 1 and crit.power by 1.
    public int agility;           //1 pt increase evasion by 1 and crit.chance by 1.
    public int intelligence;          //1pt increase magic damage by 1 and magic resistance by 3.
    public int vitality;           //1 point increase healthy by 3.

    [Header("Offensive Stats")]
    public int damage;              //int is basically an int, it's a class I made to store info on each int. Can set in inspector.
    public int critChance;
    public int critPower;            //default 150% damage.

    [Header("Defensive Stats")]
    public int maxHealth;
    public int armour;
    public int evasion;
    public int magicResistance;

    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;

    [Header("Craft Requirements")]                  //If equipment is craftable, here we cna input list of required materials.
    public List<InventoryItem> craftingMaterials;
    
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armour.AddModifier(armour);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);

    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armour.RemoveModifier(armour);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }



}
