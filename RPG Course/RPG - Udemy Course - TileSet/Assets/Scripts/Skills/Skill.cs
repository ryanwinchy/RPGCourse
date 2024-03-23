using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour     //All skills will inherit from this. So this script needs the stuff every skill has.
{
    [SerializeField] public float cooldown;  //All skills will have cooldown, if not can set to 0.
    public float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;          //This simply gives all skills inheriting from this a reference to player info so can get its transform etc... Eg, sword skill uses it.

        CheckIfSkillLoaded();
    }

    protected virtual void Update()    //inheritable.
    {
        cooldownTimer -= Time.deltaTime;
    }

    protected virtual void CheckIfSkillLoaded()
    {

    }

    public virtual bool CanUseSkill()     //All skills have this check to see if can use it.
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        player.fx.CreatePopUpText("Skill on cooldown");
        return false;
    }

    public virtual void UseSkill()       //Skills inheriting from this will each have their own use skill functionality overriding this. POLYMORPHISM :D
    {
        //do some skill specific thing.
    }

    protected virtual Transform FindClosestEnemy(Transform _checkTransform)      //Use whenever need to find closest enemy to something. Put on skill so all skills have it.
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);   //In radius of 25 around the clone pick up all colliders.

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;        //Null by default, then will add the closest enemy transform to it later in the func.

        foreach (Collider2D hit in colliders)   //for each collider in the 25 radius.
        {
            if ((hit.GetComponent<Enemy>() != null))      //If its an enemy.
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);    //get distance from clone to enemy collider it found.

                if (distanceToEnemy < closestDistance)  //If distance to this enemy is less than current closest, then set closest var to the new closest's transform.
                {
                    closestDistance = distanceToEnemy;  //So loops until finds closest one.
                    closestEnemy = hit.transform;
                }


            }
        }

        return closestEnemy;
    }
     
}
