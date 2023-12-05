using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StateMachine.ChangeState(Player.WallJumpState);
            return;
        }
        
        if(xInput != 0 && Player.FacingDir != xInput)
            StateMachine.ChangeState(Player.IdleState);

        if (yInput < 0)
            Rb.velocity = new Vector2(0, Rb.velocity.y);
        else
            Rb.velocity = new Vector2(0, Rb.velocity.y * 0.7f);
        
        if(Player.IsGroundDetected())
            StateMachine.ChangeState(Player.IdleState);
    }
}
