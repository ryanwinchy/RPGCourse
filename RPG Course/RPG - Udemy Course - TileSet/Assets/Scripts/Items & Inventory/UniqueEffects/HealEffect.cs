using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Unique Effect/Heal Effect")]          //Creates right click menu option. Create -> Data -> Item Effect
public class HealEffect : UniqueItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] float healPercent;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);

        playerStats.IncreaseHealthyBy(healAmount);
    }
}
