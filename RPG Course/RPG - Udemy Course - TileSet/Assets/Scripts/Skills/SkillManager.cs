using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;       //Singleton, access from any script without reference. Put refs to all skills here, so any script can easily access any skill.
                                                  //Central hub for scripts to access all skills  with SkillManager.instance.dash for eg.
    public DashSkill dash { get; private set; }           //Available from anywhere, as is on singleton script. get , private set so dont see in inspector.
    public CloneSkill clone {  get; private set; }
    public SwordSkill sword {  get; private set; }
    public BlackholeSkill blackhole {  get; private set; } 
    public CrystalSkill crystal { get; private set; }
    public ParrySkill parry { get; private set; }

    public DodgeSkill dodge { get; private set; }

    private void Awake()
    {
        if (instance != null)             //Only first instance gets assigned, rest deleted, like if change scenes and tries to create another.
            Destroy(instance.gameObject);
        else
            instance = this;


    }

    private void Start()
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        sword = GetComponent<SwordSkill>();
        blackhole = GetComponent<BlackholeSkill>();
        crystal = GetComponent<CrystalSkill>();
        parry = GetComponent<ParrySkill>();
        dodge = GetComponent<DodgeSkill>();
    }


}
