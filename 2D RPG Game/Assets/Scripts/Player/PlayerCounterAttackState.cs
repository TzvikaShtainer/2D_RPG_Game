using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    public PlayerCounterAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = Player.counterAttackDuration;
        Player.Anim.SetBool("SuccessfulCounterAttack", false);

        canCreateClone = true;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        Player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(Player.attackCheck.position, Player.attackCheckRadius);

        foreach (var collider in colliders)
        {
            EnemySkeleton currEnemy = collider.GetComponent<EnemySkeleton>();
            
            if(currEnemy != null)
            {
                if (currEnemy.CanBeStunned())
                {
                    stateTimer = 10; //anything that bigger then 1
                    Player.Anim.SetBool("SuccessfulCounterAttack", true);
                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        Player.Skill.Clone.CanCreateCloneOnCounterAttack(collider.transform);
                    }
                }
            }
        }
        
        if (stateTimer < 0 || triggerCalled)
            StateMachine.ChangeState(Player.IdleState);
    }
}
