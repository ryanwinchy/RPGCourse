using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Unique Effect/Buff Effect")]          //Creates right click menu option. Create -> Data -> Item Effect

public class BuffEffect : UniqueItemEffect        //What stat to modify, by how much, for how long. One script to make many scriptable objects.
{

    PlayerStats playerStats;
    [SerializeField] StatType buffType;
    [SerializeField] int buffAmount;
    [SerializeField] float buffDuration;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.IncreaseStatBy(buffAmount, buffDuration, playerStats.GetStatOfType(buffType));
    }



}
