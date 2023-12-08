using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 1;
    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0;

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;
        
        Player.Anim.SetInteger("ComboCounter", comboCounter);
        
        float attackDir = Player.FacingDir;
        
        if (xInput != 0)
            attackDir = xInput;
        
        //Player.SetVelocity(Player.attackMovement[comboCounter] * Player.FacingDir, Rb.velocity.y); //no hoop just movement forward
        Player.SetVelocity(Player.attackMovement[comboCounter].x * attackDir, Player.attackMovement[comboCounter].y);

        stateTimer = 0.1f; //so she will move a little bit when attacking
    }

    public override void Exit()
    {
        base.Exit();

        Player.StartCoroutine("BusyFor", 0.15f);
        
        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            Player.SetZeroVelocity();
        
        if(triggerCalled)
            StateMachine.ChangeState(Player.IdleState);
    }
}
