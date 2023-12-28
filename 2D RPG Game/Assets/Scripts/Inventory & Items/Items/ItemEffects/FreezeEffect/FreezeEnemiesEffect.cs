using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Enemies Effect", menuName = "Data/Item Effect/Freeze Enemies")]
public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float duration;
    [SerializeField] private float freezeEffectRadius = 2;
    
    public override void ExecuteEffect(Transform playerPos)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * 0.1f)
            return;
        
        if(!Inventory.instance.CanUseArmor())
            return;
        
        FreezeEnemiesThatAttacksYou(playerPos);
    }

    private void FreezeEnemiesThatAttacksYou(Transform playerPos)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(playerPos.position, freezeEffectRadius);

        foreach (var hit in hits)
        {
            hit.GetComponent<Enemy.Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
