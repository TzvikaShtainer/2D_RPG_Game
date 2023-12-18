using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private int currentHealth;

    [Space] 
    public Stats strength;
    public Stats damage;
    public Stats maxHealth;


    protected virtual void Start()
    {
        currentHealth = maxHealth.GetBaseValue();
    }

    public virtual void DoDamage(CharacterStats targetStats)
    {
        int totalDamage = damage.GetBaseValue() + strength.GetBaseValue();
        targetStats.TakeDamage(totalDamage);
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
        Destroy(gameObject);
    }
}
