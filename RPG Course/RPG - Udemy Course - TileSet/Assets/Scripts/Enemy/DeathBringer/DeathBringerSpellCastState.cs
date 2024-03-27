using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpellCastState : EnemyState
{
    EnemyDeathBringer enemy;

    int amtOfSpells;
    float spellTimer;
    public DeathBringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        amtOfSpells = enemy.amtOfSpells;
        spellTimer = 0.5f;

    }


    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;

        if (CanCast())
            enemy.CastSpell();

            

        if (amtOfSpells <= 0)
            stateMachine.ChangeState(enemy.teleportState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeCast = Time.time;
    }

    bool CanCast()
    {
        if (amtOfSpells > 0 && spellTimer < 0)
        {
            amtOfSpells--;
            spellTimer = enemy.spellCooldown;
            return true;
        }

        return false;
    }


}
