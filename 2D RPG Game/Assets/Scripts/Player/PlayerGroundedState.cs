using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if(Input.GetKeyDown(KeyCode.Q))
            StateMachine.ChangeState(Player.CounterAttackState);
            
        if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Z))
            StateMachine.ChangeState(Player.PrimaryAttackState);
        
        if(!Player.IsGroundDetected())
            StateMachine.ChangeState(Player.AirState);
        
        if (Input.GetKeyDown(KeyCode.Space) && Player.IsGroundDetected())
            StateMachine.ChangeState(Player.JumpState);
    }
}
