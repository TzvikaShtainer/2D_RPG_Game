using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        Player.SetVelocity(Player.moveSpeed * xInput, Rb.velocity.y);
        
        if(xInput == Player.FacingDir && Player.IsWallDetected()) //my add
            StateMachine.ChangeState(Player.IdleState);
        
        if (xInput == 0)
            StateMachine.ChangeState(Player.IdleState);
    }
}
