using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 0.4f;
        Player.SetVelocity(5 * -Player.FacingDir, Player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        if(stateTimer < 0)
            StateMachine.ChangeState(Player.AirState);
        
        if(Player.IsGroundDetected())
            StateMachine.ChangeState(Player.IdleState);
    }
}
