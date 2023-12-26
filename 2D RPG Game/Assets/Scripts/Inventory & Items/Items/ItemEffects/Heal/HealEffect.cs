using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal")]
public class HealEffect : ItemEffect
{
    [Range(0, 1)]
    [SerializeField] private float healPercent;
    
    public override void ExecuteEffect(Transform enemyPos)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healAmount = Mathf.RoundToInt(playerStats.maxHealth.GetBaseValue() * healPercent);
        
        playerStats.IncreaseHealth(healAmount);
    }
}
