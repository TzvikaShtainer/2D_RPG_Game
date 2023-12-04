using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine StateMachine;
    protected Player Player;

    protected Rigidbody2D Rb;

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        this.Player = player;
        this.StateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        Player.Anim.SetBool(animBoolName, true);
        Rb = Player.Rb;
    }

    public virtual void Exit()
    {
        Player.Anim.SetBool(animBoolName, false);
    }
    
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        Player.Anim.SetFloat("yVelocity", Rb.velocity.y);
    }
}
