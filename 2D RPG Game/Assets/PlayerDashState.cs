using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        stateTimer = Player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();
        Player.SetVelocity(0, Rb.velocity.y);
    }
    
    public override void Update()
    {
        base.Update();
        
        Player.SetVelocity(Player.dashSpeed * Player.DashDir, 0);

        if (stateTimer < 0)
            StateMachine.ChangeState(Player.IdleState);
    }
}
