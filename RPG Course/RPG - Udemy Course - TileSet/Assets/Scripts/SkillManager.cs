using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;       //Singleton, access from any script without reference. Put refs to all skills here, so any script can easily access any skill.

    public DashSkill dash;            //Available from anywhere, as is on singleton script. get , private set so dont see in inspector.

    private void Awake()
    {
        if (instance != null)             //Only first instance gets assigned, rest deleted, like if change scenes and tries to create another.
            Destroy(instance.gameObject);
        else
            instance = this;

        dash = GetComponent<DashSkill>();
    }


}
