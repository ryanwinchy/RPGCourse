using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathBringerTriggers : EnemyAnimationTriggers
{
    EnemyDeathBringer enemyDeathBringer => GetComponentInParent<EnemyDeathBringer>();

    void Relocate() => enemyDeathBringer.GoToRandomPosition();

    void MakeInvisible() => enemyDeathBringer.fx.MakeTransparent(true);
    void MakeVisible() => enemyDeathBringer.fx.MakeTransparent(false);
}
