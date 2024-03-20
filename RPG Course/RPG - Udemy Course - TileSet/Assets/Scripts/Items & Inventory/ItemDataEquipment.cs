using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public enum EquipmentType { Weapon, Armour, Amulet, Flask }


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]          //Creates right click menu option. Create -> Data -> Item

public class ItemDataEquipment : ItemData       //Exactly the same as itemData scriptable object, but has an equipment type, so can work out which slot is which, and equip only one sword for eg.
{                                                //Equipment is also equipped, so modifies stats.

    public EquipmentType equipmentType;

    public float itemCooldown;

    public UniqueItemEffect[] itemEffects;

    [TextArea]
    public string uniqueItemEffectDescription;

    [Header("Major Stats")]
    public int strength;          //1 pt increase damage by 1 and crit.power by 1.
    public int agility;           //1 pt increase evasion by 1 and crit.chance by 1.
    public int intelligence;          //1pt increase magic damage by 1 and magic resistance by 3.
    public int vitality;           //1 point increase healthy by 3.

    [Header("Offensive Stats")]
    public int damage;              
    public int critChance;
    public int critPower;            

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

    int descriptionLength;         //This is so toolTip box will always have a min size, to avoid too much resizing.
    

    public void ExecuteItemEffect(Transform _enemyPosition)
    {
        foreach (UniqueItemEffect effect in itemEffects)
        {
            effect.ExecuteEffect(_enemyPosition);
        }
    }

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


    public override string GetDescription()
    {
        stringBuilder.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit. Chance");
        AddItemDescription(critPower, "Crit. Power");

        AddItemDescription(maxHealth, "Health");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(armour, "Armour");
        AddItemDescription(magicResistance, "Magic Resist.");
        AddItemDescription(fireDamage, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightningDamage, "Thunder Damage");

        if (descriptionLength < 5)
        {
            for (int i = 0; i < 5 - descriptionLength; i++)     //This means if the desc is less than 5 lines, we will add 5 lines. Then, most the time the tooltip is same size, but resizes if big. Looks great!
            {
                stringBuilder.AppendLine();
                stringBuilder.Append("");        //Empty space after stats.
            }
        }

        if (uniqueItemEffectDescription.Length > 0)       //If this item has a unique effect.
        {
            stringBuilder.AppendLine();
            stringBuilder.Append(uniqueItemEffectDescription);
        }

        return stringBuilder.ToString();
    }

    void AddItemDescription(int _statValue, string _name)    //This is to display all the item stats. Builds description, adds to string each time called.
    {
        if (_statValue != 0)        //Only if have a stat value (dont display the 0s).
        {
            if (stringBuilder.Length > 0)
            {
                stringBuilder.AppendLine();       //Add new line to description between stats.
            }

            if (_statValue > 0)
                stringBuilder.Append("+ " + _statValue + " " + _name);

            descriptionLength++;

        }
    }


}
