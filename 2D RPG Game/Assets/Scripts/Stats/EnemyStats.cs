using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy.Enemy enemy;
    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy.Enemy>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        
        enemy.DamageEffects();
    }

    public override void Die()
    {
        base.Die();
        
        enemy.Die();
    }
}
