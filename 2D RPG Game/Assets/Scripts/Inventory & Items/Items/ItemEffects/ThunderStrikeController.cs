using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy.Enemy>() != null)
        {
            EnemyStats enemyTarget = other.GetComponent<EnemyStats>();
            
            playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}