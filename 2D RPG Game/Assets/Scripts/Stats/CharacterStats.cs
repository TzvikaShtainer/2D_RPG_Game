using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stats strength;
    public Stats agility;
    public Stats intelligence;
    public Stats vitality;
    
    [Header("Offensice Stats")]
    public Stats damage;
    public Stats critPower;
    public Stats critChance;
    
    [Header("Defensive Stats")]
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;

    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = maxHealth.GetBaseValue();
    }

    public virtual void DoDamage(CharacterStats targetStats)
    {
        if (TargetCanAvoidAttack(targetStats))
            return;

        int totalDamage = damage.GetBaseValue() + strength.GetBaseValue();
        
        if (CanCrit())
        {
            totalDamage = CalcCritDamage(totalDamage);
        }
        
        totalDamage = CalcTargetArmor(targetStats, totalDamage);
        
        targetStats.TakeDamage(totalDamage);
    }

    private bool TargetCanAvoidAttack(CharacterStats targetStats)
    {
        int totalEvasion = targetStats.evasion.GetBaseValue() + targetStats.agility.GetBaseValue();
        if (totalEvasion > Random.Range(0, 100))
        {
            return true;
        }

        return false;
    }
    
    private int CalcTargetArmor(CharacterStats targetStats, int totalDamage)
    {
        totalDamage -= targetStats.armor.GetBaseValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue); //if -damage makes it 0
        return totalDamage;
    }

    public bool CanCrit()
    {
        int totalCrit = critChance.GetBaseValue() + agility.GetBaseValue();
        if (totalCrit >= Random.Range(0, 100))
        {
            return true;
        }

        return false;
    }

    public int CalcCritDamage(int damage)
    {
        float totalCritPower = (critPower.GetBaseValue() + strength.GetBaseValue()) * 0.1f;

        float critDamage = totalCritPower * damage;

        return Mathf.RoundToInt(critDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth < 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        
    }
}
