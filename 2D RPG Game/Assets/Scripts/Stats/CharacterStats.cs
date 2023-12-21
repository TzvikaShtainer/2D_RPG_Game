using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;
    
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

    public bool isIgnited; //damage over time
    [SerializeField] private float igniteDuration = 4;
    
    public bool isChilled; //reduce armor by 20%
    [SerializeField] private float chillDuration = 4;
    
    public bool isShocked; //reduce accuracy by 20%
    [SerializeField] private float shockDuration = 4;

    private float ChilledTimer;
    private float ShockedTimer;
    private float ignitedTimer;
    
    
    private float ignitedDamageCooldown = 0.3f;
    private float ignitedDamageTimer;
    private int ignitedDamage;
    
    
    //make elements Generic
    
    
    public int currentHealth;

    public delegate void OnHealthChanged();
    public event OnHealthChanged onHealthChanged;

    private void Awake()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        
    }

    protected virtual void Start()
    {
        fx = GetComponent<EntityFX>();
        //critPower.SetDefaultValue(150);
        //currentHealth = GetMaxHealthValue();
    }

    protected virtual void Update()
    {
        ElementsHandler();
    }

    private void ElementsHandler()
    {
        ignitedTimer -= Time.deltaTime;
        ChilledTimer -= Time.deltaTime;
        ShockedTimer -= Time.deltaTime;
        
        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;
        
        if (ChilledTimer < 0)
            isChilled = false;
        
        if (ShockedTimer < 0)
            isShocked = false;

        if (ignitedDamageTimer < 0 && isIgnited)
        {
            ignitedDamageTimer = ignitedDamageCooldown;
            DecreaseHealth(ignitedDamage);
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void SetupIgniteDamage(int _damage) => ignitedDamage = _damage; 
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
        
        //targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(targetStats);
    }

    private bool TargetCanAvoidAttack(CharacterStats targetStats)
    {
        int totalEvasion = targetStats.evasion.GetBaseValue() + targetStats.agility.GetBaseValue();

        if (isShocked)
            totalEvasion += 20;
        
        if (totalEvasion > Random.Range(0, 100))
        {
            return true;
        }

        return false;
    }
    
    private int CalcTargetArmor(CharacterStats targetStats, int totalDamage)
    {
        if (isChilled)
            totalDamage -= Mathf.RoundToInt(targetStats.armor.GetBaseValue() * 0.8f);
        else
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

    public virtual void DoMagicalDamage(CharacterStats targetStats)
    {
        int _fireDamage = fireDamage.GetBaseValue();
        int _iceDamage = iceDamage.GetBaseValue();
        int _lightingDamage = lightingDamage.GetBaseValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetBaseValue();

        totalMagicDamage = CalcMagicResistance(targetStats, totalMagicDamage);
        targetStats.TakeDamage(totalMagicDamage);

        if(Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0) //so we wont go to endless while
            return;
        
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < 0.3f && _fireDamage > 0)
            {
                canApplyIgnite= true;
                targetStats.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            
            if (Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChill = true;
                targetStats.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            
            if (Random.value < 0.5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                targetStats.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }
        
        if(canApplyIgnite)
            targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
        
        targetStats.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private static int CalcMagicResistance(CharacterStats targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= targetStats.magicResistance.GetBaseValue() + (targetStats.intelligence.GetBaseValue() * 3); //get 3 on every intelligence point, need to make this varible
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public virtual void ApplyElements(bool _isIgnited, bool _isChilled, bool _isShocked)
    {
        if(isIgnited || isChilled || isShocked)
            return;

        if (_isIgnited)
        {
            isIgnited = _isIgnited;
            ignitedTimer = igniteDuration;
            
            fx.IgniteFxFor(igniteDuration);
        }
        
        if (_isChilled)
        {
            isChilled = _isChilled;
            ChilledTimer = chillDuration;
            
            fx.ChillFxFor(chillDuration);
            
        }
        
        if (_isShocked)
        {
            isShocked = _isShocked;
            ShockedTimer = shockDuration;
            
            fx.ShockFxFor(shockDuration);
        }   
    }
    
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealth(_damage);

        if (currentHealth < 0)
        {
            Die();
        }
    }

    public virtual void DecreaseHealth(int _damage)
    {
        Debug.Log(_damage);
        currentHealth -= _damage;
        onHealthChanged?.Invoke();
    }

    public virtual void Die()
    {
        
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetBaseValue() + vitality.GetBaseValue() * 5;
    }
}
