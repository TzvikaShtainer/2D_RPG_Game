using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class EnemySkeletonAnimationsTriggers : MonoBehaviour
{
    private EnemySkeleton EnemySkeleton => GetComponentInParent<EnemySkeleton>();
    
    private void AnimationTrigger()
    {
        EnemySkeleton.AnimationFinishTrigger();
    }
    
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(EnemySkeleton.attackCheck.position, EnemySkeleton.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Player>().Damage();
                PlayerStats target = hit.GetComponent<PlayerStats>();
                EnemySkeleton.Stats.DoDamage(target);
            }
        }
    }

    private void OpenCounterAttackWindow() => EnemySkeleton.OpenCounterAttackWindow();
    
    private void CloseCounterAttackWindow() => EnemySkeleton.CloseCounterAttackWindow();
}
