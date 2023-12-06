using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class EnemySkeletonMoveState : EnemyState
{
    private EnemySkeleton enemy;
    
    public EnemySkeletonMoveState(Enemy.Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemySkeleton enemy) : base(enemy, enemyStateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Update()
    {
        base.Update();
        
        enemy.SetVelocity(2 * enemy.FacingDir, enemy.Rb.velocity.y);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            EnemyStateMachine.ChangeState(enemy.SkeletonIdleState);
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
