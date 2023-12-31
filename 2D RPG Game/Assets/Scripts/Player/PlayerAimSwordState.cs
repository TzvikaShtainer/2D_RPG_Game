using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Player.Skill.Sword.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        Player.StartCoroutine("BusyFor", 0.2f);
    }

    public override void Update()
    {
        base.Update();
        
        Player.SetZeroVelocity();
        
        if(Input.GetKeyUp(KeyCode.Mouse1))
            StateMachine.ChangeState(Player.IdleState);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if(Player.transform.position.x > mousePosition.x && Player.FacingDir == 1)
            Player.Flip();
        else if(Player.transform.position.x < mousePosition.x && Player.FacingDir == -1)
            Player.Flip();
    }
}
