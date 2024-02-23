using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour     //All skills will inherit from this. So this script needs the stuff every skill has.
{
    [SerializeField] protected float cooldown;  //All skills will have cooldown, if not can set to 0.
    protected float cooldownTimer;

    protected virtual void Update()    //inheritable.
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()     //All skills have this check to see if can use it.
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        Debug.Log("Skill is on cooldown");
        return false;
    }

    public virtual void UseSkill()       //Skills inheriting from this will each have their own use skill functionality overriding this. POLYMORPHISM :D
    {
        //do some skill specific thing.
    }
     
}
