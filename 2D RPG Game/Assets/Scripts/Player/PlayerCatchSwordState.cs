using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    public PlayerCatchSwordState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = Player.Sword.transform;
        
        if(Player.transform.position.x > sword.position.x && Player.FacingDir == 1)
            Player.Flip();
        else if(Player.transform.position.x < sword.position.x && Player.FacingDir == -1)
            Player.Flip();

        Rb.velocity = new Vector2(Player.swordReturnImpact * -Player.FacingDir, Rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        
        Player.StartCoroutine("BusyFor", 0.1f);
    }

    public override void Update()
    {
        base.Update();
        
        if(triggerCalled)
            StateMachine.ChangeState(Player.IdleState);
    }
}
