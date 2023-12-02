using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Components
    public PlayerStateMachine StateMachine { get; private set; }
    public Animator Anim { get; private set; }
    
    #endregion
    
    #region States
    public PlayerIdleState IdleState;
    public PlayerMoveState MoveState;
    
    #endregion
    
    
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
    }

    private void Start()
    {
        Anim = GetComponentInChildren<Animator>(); //cuz its a child of player
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.Update();
    }
}
