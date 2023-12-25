using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy.Enemy>() != null)
        {
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyTarget = other.GetComponent<EnemyStats>();
            
            playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}