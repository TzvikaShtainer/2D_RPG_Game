using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyStats : CharacterStats
{
    private Enemy.Enemy enemy;

    [Header("Level Details")] [SerializeField]
    private int level = 1;
    
    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = 0.4f;

    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy.Enemy>();
    }

    private void ApplyLevelModifiers()
    {
        // Modify(strength);
        // Modify(agility);
        // Modify(intelligence);
        // Modify(vitality);
        
        Modify(damage);
        Modify(critPower);
        Modify(critChance);
        
        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);
        
        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);
    }

    private void Modify(Stats _stats)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stats.GetBaseValue() * percentageModifier;
            
            _stats.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

public override void Die()
    {
        base.Die();
        
        enemy.Die();
    }
}
