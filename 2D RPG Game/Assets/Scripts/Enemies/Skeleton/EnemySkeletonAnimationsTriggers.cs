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
}
