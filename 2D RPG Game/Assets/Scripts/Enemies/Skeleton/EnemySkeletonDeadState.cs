using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class EnemySkeletonDeadState : EnemyState
{
    private EnemySkeleton enemy;
    
    public EnemySkeletonDeadState(Enemy.Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemySkeleton enemySkeleton) : base(enemyBase, enemyStateMachine, animBoolName)
    {
        enemy = enemySkeleton;
    }

    public override void Update()
    {
        base.Update();
        
    }

    public override void Enter()
    {
        base.Enter();
        
        enemy.Anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.Cd.enabled = false;
        enemy.Anim.speed = 0;

        stateTimer = 0.15f;
    }

    public override void Exit()
    {
        base.Exit();

        if (stateTimer > 0)
            Rb.velocity = new Vector2(0, 10); //make the enemy jum on dead
    }
}
