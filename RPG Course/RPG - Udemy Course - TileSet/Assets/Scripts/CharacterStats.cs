using UnityEngine;

public class CharacterStats : MonoBehaviour     //base stat class. These stats, all chars have. Player can upgrade / invest in them to improve.
{
    [Header("Major Stats")]
    public Stat strength;          //1 pt increase damage by 1 and crit.power by 1.
    public Stat agility;           //1 pt increase evasion by 1 and crit.chance by 1.
    public Stat intelligence;          //1pt increase magic damage by 1 and magic resistance by 3.
    public Stat vitality;           //1 point increase healthy by 3.

    [Header("Offensive Stats")]
    public Stat damage;              //Stat is basically an int, it's a class I made to store info on each stat. Can set in inspector.
    public Stat critChance;
    public Stat critPower;            //default 150% damage.

    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armour;
    public Stat evasion;




 

    [SerializeField] int currentHealth;
    protected virtual void Start()           //Found bug. If this was private (not protected virtual), charStats and EnemyStats can't override (even if just keep base), so had no current health. Important to understand.
    {
        critPower.SetDefaultValue(150);       //All entities in game with stats, have 150% crit by default.
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamage(CharacterStats _targetStats)         //Nice easy method to combine stats. targetStats is the target, like enemy. this script is the damager.
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue() - _targetStats.armour.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);          //Ensure damage is 0 minimum. Otherwise, if armour more than damage, theyd get healed.

        if (CanCrit())
        {
            Debug.Log("Crit hit!");
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        _targetStats.TakeDamage(totalDamage);
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)     
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)               //Choose random 1 to 100, if lower than total evasion, dodge. So more evasion, the more likely dodge.
        {
            Debug.Log("Attack avoided!");
            return true;
        }

        return false;
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth < 0)
            Die();
    }

    protected virtual void Die()      //Just a template, this is overidden in playerStats and enemyStats to perform their death sequence.
    {

    }

    bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;   //So works as float.

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);      //As stats are ints, dont want messy floats.
    }

}
