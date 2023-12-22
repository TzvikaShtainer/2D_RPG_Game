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
    
    [Header("Points From Stats Stats")]
    [SerializeField] private int magicResPointFromIntelligent = 3;
    [SerializeField] private int healthPointsFromVitality = 5;
    
    [Header("Offensive Stats")]
    public Stats damage;
    public Stats critPower;
    public Stats critChance;
    
    [Header("Defensive Stats")]
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;
    public Stats magicResistance;

    [Header("Magic Stats")] //make elements Generic

    [Header("Fire")]
    [Tooltip("damage over time")]
    public Stats fireDamage;
    public bool isIgnited; //damage over time
    [SerializeField] private float igniteDuration = 4;
    
    private float ignitedTimer;
    private float ignitedDamageCooldown = 0.3f;
    private float ignitedDamageTimer;
    private int ignitedDamage;
    [SerializeField] private float igniteDamagePercentage = 0.2f;
    
    
    [Header("Ice")]
    [Tooltip("reduce armor by 20%")]
    public Stats iceDamage;
    public bool isChilled; //reduce armor by 20%
    [SerializeField] private float chillDuration = 4;
    [SerializeField] private float slowPercentage = 0.3f;
    private float ChilledTimer;
    
    
    [Header("Lighting")]
    [Tooltip("reduce accuracy by 20%")]
    public Stats lightingDamage;
    public bool isShocked; //reduce accuracy by 20%
    [SerializeField] private float shockDuration = 4;
    private float ShockedTimer;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;
    [SerializeField] private float shockDamagePercentage = 0.1f;
    
    //make elements Generic
    
    
    public int currentHealth;

    public delegate void OnHealthChanged();
    public event OnHealthChanged onHealthChanged;

    protected bool isDead;

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

    #region Physical Damage 
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
        
        //DoMagicalDamage(targetStats);
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
    #endregion
    
    #region Magic Damage And Elements
    public void DoMagicalDamage(CharacterStats targetStats)
    {
        int _fireDamage = fireDamage.GetBaseValue();
        int _iceDamage = iceDamage.GetBaseValue();
        int _lightingDamage = lightingDamage.GetBaseValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetBaseValue();

        totalMagicDamage = CalcMagicResistance(targetStats, totalMagicDamage);
        targetStats.TakeDamage(totalMagicDamage);

        if(Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0) //so we wont go to endless while
            return;
        
        AttemptToApplyElements(targetStats, _fireDamage, _iceDamage, _lightingDamage);
    }

    private void AttemptToApplyElements(CharacterStats targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
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
            targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * igniteDamagePercentage));
        
        if(canApplyShock)
            targetStats.SetupShockDamage(Mathf.RoundToInt(_lightingDamage * shockDamagePercentage));
        
        targetStats.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private int CalcMagicResistance(CharacterStats targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= targetStats.magicResistance.GetBaseValue() + (targetStats.intelligence.GetBaseValue() * magicResPointFromIntelligent);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public void ApplyElements(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled  && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled  && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = igniteDuration;
            
            fx.IgniteFxFor(igniteDuration);
        }
        
        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            ChilledTimer = chillDuration;
            
            GetComponent<Entity>().SlowEntityBy(slowPercentage, chillDuration);
            fx.ChillFxFor(chillDuration);
            
        }
        
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if(GetComponent<Player>() != null)
                    return;

                HitClosestTargetWithShockStrike();
            }
        }   
    }

    public void ApplyShock(bool _shock)
    {
        if(isShocked)
            return;
        
        isShocked = _shock;
        ShockedTimer = shockDuration;
            
        fx.ShockFxFor(shockDuration);
    }

    private void HitClosestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        
        float closestTarget = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy.Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestTarget)
                {
                    closestTarget = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform; //thunder the same enemy we attack
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
                    
            newShockStrike.GetComponent<ShockStrikeController>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
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

        if(isIgnited)
            ApplyIgniteDamage();
    }

    private void ApplyIgniteDamage()
    {
        if (ignitedDamageTimer < 0)
        {
            ignitedDamageTimer = ignitedDamageCooldown;
            
            DecreaseHealth(ignitedDamage);
            
            if (currentHealth <= 0 && !isDead)
                Die();
        }
    }

    public void SetupIgniteDamage(int _damage) => ignitedDamage = _damage;
    public void SetupShockDamage(int _damage) => shockDamage = _damage;
    #endregion

    #region Health
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealth(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");
        
        if (currentHealth <= 0 && !isDead)
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
        isDead = true;
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetBaseValue() + vitality.GetBaseValue() * healthPointsFromVitality;
    }
    #endregion
}
