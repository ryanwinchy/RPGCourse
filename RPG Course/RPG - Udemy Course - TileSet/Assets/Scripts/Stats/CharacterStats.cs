using System.Collections;
using UnityEngine;
//All stats controlled here, other scripts can just get vals from here by referencing the script on the game object.

public enum StatType { strength, agility, intelligence, vitality, damage, critChance, critPower, health, armour, evasion, magicResistance, fireDamage, iceDamage, lightningDamage }

public class CharacterStats : MonoBehaviour     //base stat class. These stats, all chars have. Player can upgrade / invest in them to improve.
{

    EntityFX entityFx;

    [Header("Major Stats")]
    public Stat strength;          //1 pt increase damage by 1 and crit.power by 1.
    public Stat agility;           //1 pt increase evasion by 1 and crit.chance by 1.
    public Stat intelligence;          //1pt increase magic damage by 1 and magic resistance by 3.
    public Stat vitality;           //1 point increase healthy by 5.

    [Header("Offensive Stats")]
    public Stat damage;              //Stat is basically an int, it's a class I made to store info on each stat. Can set in inspector.
    public Stat critChance;
    public Stat critPower;            //default 150% damage.

    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armour;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited;       //does damage over time.
    public bool isChilled;        // reduce armour by 20%.
    public bool isShocked;       // reduce accuracy by 20%.

    [SerializeField] float ailmentsDuration = 4f;
    float ignitedTimer;
    float chilledTimer;
    float shockedTimer;

    float igniteDamageCooldown = 0.3f;      //Gets damage every 0.3 seconds while burning.
    float igniteDamageTimer;
    int igniteDamage;
    [SerializeField] GameObject shockStrikePrefab;
    int shockDamage;     //Damage of the actual ailment, the lightning emitted.


    public int currentHealth;

    public bool isDead { get; private set; }
    public bool isInvincible { get; private set; }
    bool isVulnerable;

    public System.Action OnHealthChanged;     //New void event.



    protected virtual void Start()           //Found bug. If this was private (not protected virtual), charStats and EnemyStats can't override (even if just keep base), so had no current health. Important to understand.
    {
        critPower.SetDefaultValue(200);         //All entities in game with stats, have 150% crit by default.
        currentHealth = GetMaxHealthValue();

        entityFx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)       //How long ignited lasts.
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        ApplyIgniteDamage();
    }

    public void MakeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerableCoroutine(_duration));
    }

    IEnumerator VulnerableCoroutine(float _duration)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duration);

        isVulnerable = false;
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModifierCoroutine(_modifier, _duration, _statToModify));
    }

    IEnumerator StatModifierCoroutine(int _modifier, float _duration, Stat _statToModify) //Temporarily add modifier, like from buff potion.
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }



    public virtual void DoDamage(CharacterStats _targetStats)         //Nice easy method to combine stats. targetStats is the target, like enemy. this script is the damager.
    {

        bool criticalHit = false;

        if (_targetStats.isInvincible)    //Dont run damage script , no damage or fx.
            return;

        if (TargetCanAvoidAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);   //So knocksback entity getting damaged the correct direction.

        int totalDamage = damage.GetValue() + strength.GetValue();

        totalDamage = CheckTargetArmour(_targetStats, totalDamage);

        if (CanCrit())
        {
            Debug.Log("Crit hit!");
            totalDamage = CalculateCriticalDamage(totalDamage);
            criticalHit = true;
        }

        entityFx.CreateHitFx(_targetStats.transform, criticalHit);    //When do damage, create hit FX on target (normal or crit different).  Could do it for each damage type, magic, crystal, sword throw but long.

        _targetStats.TakeDamage(totalDamage);            // Do physical damage.

        DoMagicalDamage(_targetStats);            //Do magic damage. Remove this if dont want to apply magic damage on primary atk.
    }


    #region Magical Damage and Ailments

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0 && isIgnited)
        {
            DecreaseHealthBy(igniteDamage);    //take burn damage every 0,3 secs. Dont want DoDamage() func, as triggers knockback and flash visual.

            if (currentHealth < 0 && !isDead)      //Without is dead check, will keep killing the enemy over and over (so will bounce) when killed with fire.
                Die();

            //take burn damage.
            igniteDamageTimer = igniteDamageCooldown;   //So applies every 0.3 seconds.
        }
    }
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();    //Just saves us typing get value all the time in this func.
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();  //total magic damage.
        totalMagicalDamage -= (_targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3));   //maag damage reduce by targets resistance and intelligence.
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);      //So damage can't go negative if they have high resistance.

        _targetStats.TakeDamage(totalMagicalDamage);

        //APPLY AILMENTS. IF EQUAL, RANDOM ONE.

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)       //If all 0, return. Otherwise while loop below could be infinite.
            return;

        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);

    }

    private void AttemptToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;   //Whichever of the 3 is biggest gets applied.
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyShock && !canApplyIgnite && !canApplyChill)     //If all equal, can't apply any, so choose random to apply.
        {
            if (Random.value < 0.5f && _fireDamage > 0)          //random.value is between 0 and 1, so < 0.5 is coin flip. Coin flip, go down until met. If fails this, tries ice.
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < 0.5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));   //Apply 20% of fire dam stat every 0.3 seconds while ignited.

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * 0.1f));   //the lightning bolt is 10% of lighting damage stat.

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isShocked && !isChilled;     //For now, can't stack ailments.
        bool canApplyChill = !isIgnited && !isShocked && !isChilled;
        bool canApplyShock = !isIgnited && !isChilled;


        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;        //Burn for 4 seconds.

            entityFx.IgniteFxFor(ailmentsDuration);
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;

            float slowPercentage = 0.2f;         //Slow by 20%
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);     //Get entity component will also get player if its a player as it inherits from entity. So it will call the correct version of slow entity by which player inherits from entity.
            entityFx.ChillFxFor(ailmentsDuration);
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else        //if already shocked.
            {      //GET CLOSEST ENEMY.

                if (GetComponent<Player>() != null) //If player, do not do this. Enemies only.
                    return;

                HitNearestTargetWithShockStrike();

            }



        }

    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = ailmentsDuration;

        entityFx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);   //In radius of 25 around the clone pick up all colliders.

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;        //Null by default, then will add the closest enemy transform to it later in the func.

        foreach (Collider2D hit in colliders)   //for each collider in the 25 radius.
        {
            if ((hit.GetComponent<Enemy>() != null) && Vector2.Distance(transform.position, hit.transform.position) > 1)      //second check is to make sure there is some distance, as lightning triggered on enemy you hit. Dont want to target itself, its supposed to bounce to nearest enemy FROM enemy you hit.
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);    //get distance from clone to enemy collider it found.

                if (distanceToEnemy < closestDistance)  //If distance to this enemy is less than current closest, then set closest var to the new closest's transform.
                {
                    closestDistance = distanceToEnemy;  //So loops until finds closest one.
                    closestEnemy = hit.transform;
                }


            }

            if (closestEnemy == null)    //If no other closest enemy, hit the enemy you hit.
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrikeController>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());

        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockStrikeDamage(int damage) => shockDamage = damage;

    #endregion
    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible)
            return;

        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        entityFx.StartCoroutine("FlashFX");

        if (currentHealth < 0 && !isDead)
            Die();
    }

    public virtual void IncreaseHealthyBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > GetMaxHealthValue())         //Cannot heal more than max health.
            currentHealth = GetMaxHealthValue();

        if (OnHealthChanged != null)
            OnHealthChanged();             //Fire event, so healthbarUI updates.
    }

    protected virtual void DecreaseHealthBy(int _damage)   //Script whenever we need to update health stat, without other effects.
    {

        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);

        currentHealth -= _damage;

        if (_damage > 0)
            entityFx.CreatePopUpText(_damage.ToString());

        if (OnHealthChanged != null)
            OnHealthChanged();             //Fire event, so healthbarUI updates.
    }

    protected virtual void Die()      //Just a template, this is overidden in playerStats and enemyStats to perform their death sequence.
    {
        isDead = true;
    }

    public void KillEntity()
    {
        if (!isDead)                    //Prevents receiving souls twice , once when you kill, once when he falls off map, just to be safe.
            Die();    //Calls die like if char falls off map. It's public where die is protected.
    }

    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;

    #region Stat Calculcations
    protected int CheckTargetArmour(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armour.GetValue() * 0.7f);      //If chilled, targets armour 70% effective.
        else
            totalDamage -= _targetStats.armour.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);          //Ensure damage is 0 minimum. Otherwise, if armour more than damage, theyd get healed.
        return totalDamage;
    }

    public virtual void OnEvasion()
    {

    }
    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;       //If I am shocked, my target more likely to avoid attack.

        if (Random.Range(0, 100) < totalEvasion)      //Choose random 1 to 100, if lower than total evasion, dodge. So more evasion, the more likely dodge.
        {
            _targetStats.OnEvasion();          //If dodge, run on evasion. Player inherits it, and creates mirage on dodge if unlocked.
            return true;
        }

        return false;
    }


    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;   //So works as float.

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);      //As stats are ints, dont want messy floats.
    }

    public int GetMaxHealthValue() => maxHealth.GetValue() + (vitality.GetValue() * 5);

    #endregion

    public Stat GetStatOfType(StatType statType)
    {
        if (statType == StatType.strength) return strength;       //Soooo much quicker doing this than script for each stat buff.
        else if (statType == StatType.agility) return agility;
        else if (statType == StatType.intelligence) return intelligence;
        else if (statType == StatType.vitality) return vitality;
        else if (statType == StatType.damage) return damage;
        else if (statType == StatType.critChance) return critChance;
        else if (statType == StatType.critPower) return critPower;
        else if (statType == StatType.health) return maxHealth;
        else if (statType == StatType.armour) return armour;
        else if (statType == StatType.evasion) return evasion;
        else if (statType == StatType.magicResistance) return magicResistance;
        else if (statType == StatType.fireDamage) return fireDamage;
        else if (statType == StatType.iceDamage) return iceDamage;
        else if (statType == StatType.lightningDamage) return lightningDamage;

        return null;   //If none found.


    }


}
