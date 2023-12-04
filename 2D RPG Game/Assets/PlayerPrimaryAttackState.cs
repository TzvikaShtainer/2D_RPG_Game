using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2;
    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.deltaTime >= lastTimeAttacked + comboWindow)
            comboCounter = 0;
        
        Player.Anim.SetInteger("ComboCounter", comboCounter);
        Debug.Log(comboCounter);
    }

    public override void Exit()
    {
        base.Exit();

        comboCounter++;
        lastTimeAttacked = Time.deltaTime;
        Debug.Log(lastTimeAttacked);
    }

    public override void Update()
    {
        base.Update();
        
        if(triggerCalled)
            StateMachine.ChangeState(Player.IdleState);
    }
}
