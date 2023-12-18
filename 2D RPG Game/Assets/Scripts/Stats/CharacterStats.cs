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
    public Stats magicResistance;

    [Header("Magic Stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightingDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;
    
    

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
        //DoMagicDamage(targetStats);
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

    public virtual void DoMagicDamage(CharacterStats targetStats)
    {
        int _fireDamage = fireDamage.GetBaseValue();
        int _iceDamage = iceDamage.GetBaseValue();
        int _lightingDamage = lightingDamage.GetBaseValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetBaseValue();

        totalMagicDamage = CalcMagicResistance(targetStats, totalMagicDamage);
        targetStats.TakeDamage(totalMagicDamage);
    }

    private static int CalcMagicResistance(CharacterStats targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= targetStats.magicResistance.GetBaseValue() + (targetStats.intelligence.GetBaseValue() * 3); //get 3 on every intelligence point, need to make this varible
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public virtual void ApplyElements(bool isIgnited, bool isChilled, bool isShocked)
    {
        if(this.isIgnited || this.isChilled || this.isShocked)
            return;

        this.isIgnited = isIgnited;
        this.isChilled = isChilled;
        this.isShocked = isShocked;
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
