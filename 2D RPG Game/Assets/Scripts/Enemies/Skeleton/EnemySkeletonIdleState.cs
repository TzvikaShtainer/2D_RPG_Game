using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class EnemySkeletonIdleState : EnemySkeletonGroundedState
{
    
    public EnemySkeletonIdleState(Enemy.Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemySkeleton enemy) : base(enemyBase, enemyStateMachine, animBoolName, enemy)
    {
    }
    public override void Update()
    {
        base.Update();
        
        if(stateTimer > 0)
            EnemyStateMachine.ChangeState(enemy.SkeletonMoveState);
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
