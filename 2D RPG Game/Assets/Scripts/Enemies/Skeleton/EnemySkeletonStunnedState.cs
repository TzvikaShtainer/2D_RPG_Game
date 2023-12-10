using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class EnemySkeletonStunnedState : EnemyState
{
    private EnemySkeleton enemy;
    
    public EnemySkeletonStunnedState(Enemy.Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemySkeleton enemy) : base(enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Update()
    {
        base.Update();
        
        if(stateTimer < 0)
            EnemyStateMachine.ChangeState(enemy.SkeletonIdleState);
    }

    public override void Enter()
    {
        base.Enter();
        
        enemy.fx.InvokeRepeating("RedColorBlink", 0, 0.1f);
        
        stateTimer = enemy.stunDuration;
        
       Rb.velocity = new Vector2(enemy.StunDirection.X * -enemy.FacingDir, enemy.StunDirection.Y);
    }

    public override void Exit()
    {
        base.Exit();
        
        enemy.fx.Invoke("CancelRedBlink", 0);
    }
}
