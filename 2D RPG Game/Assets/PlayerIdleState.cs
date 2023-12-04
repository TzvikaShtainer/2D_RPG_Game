using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        Player.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    public override void Update()
    {
        base.Update();

        if(xInput == Player.FacingDir && Player.IsWallDetected())
            return;
        
        if (xInput != 0)
            StateMachine.ChangeState(Player.MoveState);
    }
}
