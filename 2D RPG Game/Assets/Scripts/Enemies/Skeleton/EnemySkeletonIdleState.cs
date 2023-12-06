using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class EnemySkeletonIdleState : EnemyState
{
     EnemySkeleton enemy;
    public EnemySkeletonIdleState(Enemy.Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemySkeleton enemy) : base(enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = enemy;
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

        stateTimer = 1f;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
