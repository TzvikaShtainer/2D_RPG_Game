using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime = 0.4f;
    private bool skillUsed;

    private float defaultGravity;
    public PlayerBlackHoleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skillUsed = false;
        stateTimer = flyTime;

        defaultGravity = Player.Rb.gravityScale;
        Rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        Player.Rb.gravityScale = defaultGravity;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            Rb.velocity = new Vector2(0, 15);
        }

        if (stateTimer < 0)
        {
            Rb.velocity = new Vector2(0, -0.1f);

            if (!skillUsed)
            {
                if(Player.Skill.blackHole.CanUseSkill())
                    skillUsed = true;
            }
        }
    }
    
    //we exit state in black hole skill controller when all of the attacks are over
}
