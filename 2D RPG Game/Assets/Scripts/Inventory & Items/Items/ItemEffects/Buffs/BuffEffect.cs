using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critPower,
    critChance,
    maxHealth,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage,
}
[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Buff")]
public class BuffEffect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;
    
    public override void ExecuteEffect(Transform enemyPos)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());
    }

    private Stats StatToModify()
    {
        if (buffType == StatType.strength) return stats.strength;
        if (buffType == StatType.agility) return stats.agility;
        if (buffType == StatType.intelligence) return stats.intelligence;
        if (buffType == StatType.vitality) return stats.vitality;
        if (buffType == StatType.damage) return stats.damage;
        if (buffType == StatType.critPower) return stats.critPower;
        if (buffType == StatType.critChance) return stats.critChance;
        if (buffType == StatType.maxHealth) return stats.maxHealth;
        if (buffType == StatType.armor) return stats.armor;
        if (buffType == StatType.evasion) return stats.evasion;
        if (buffType == StatType.magicResistance) return stats.magicResistance;
        if (buffType == StatType.fireDamage) return stats.fireDamage;
        if (buffType == StatType.iceDamage) return stats.iceDamage;
        if (buffType == StatType.lightingDamage) return stats.lightingDamage;

        return null;
    }
}
