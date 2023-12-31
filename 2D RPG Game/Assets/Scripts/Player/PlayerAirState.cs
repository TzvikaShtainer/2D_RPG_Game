using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    public override void Update()
    {
        base.Update();
        
        if(Player.IsWallDetected())
            StateMachine.ChangeState(Player.WallSlideState);
        
        if (Player.IsGroundDetected())
            StateMachine.ChangeState(Player.IdleState);

        if (xInput != 0)
            Player.SetVelocity(Player.moveSpeed * .8f * xInput, Rb.velocity.y);
    }
}
