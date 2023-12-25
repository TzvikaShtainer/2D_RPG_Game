using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy.Enemy>() != null)
            {
                EnemyStats target = hit.GetComponent<EnemyStats>();
                
                player.Stats.DoDamage(target);

                Inventory.instance.GetEquippedItem(EquipmentType.Weapon)?.ExecuteItemEffect();
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.Sword.CreateSword();
    }
}
