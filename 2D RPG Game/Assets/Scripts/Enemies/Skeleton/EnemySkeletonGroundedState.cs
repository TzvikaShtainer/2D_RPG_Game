using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class EnemySkeletonGroundedState : EnemyState
{
    protected EnemySkeleton enemy;
    protected Transform player;
    
    public EnemySkeletonGroundedState(Enemy.Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemySkeleton enemy) : base(enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
            EnemyStateMachine.ChangeState(enemy.SkeletonBattleState);
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
