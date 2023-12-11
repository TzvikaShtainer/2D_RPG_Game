using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Entity
{
    [Header("Attack Details")]
    //public float[] attackMovement; for only movement forword
    public Vector2[] attackMovement; // with hooping

    public float counterAttackDuration = 0.2f;
     
    public bool isBusy { get; private set; }
    
    [Header("Move Info")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Dash Info")]
    [SerializeField] private float dashCooldown = 1;
    private float dashUsageTimer;
    public float dashSpeed = 25f;
    public float dashDuration = 0.4f;
    public float DashDir { get; private set; }

    public SkillManager skill { get; private set; }
    
    //public float wallSpeed = 0.5f;
    public PlayerStateMachine StateMachine { get; private set; }
    
    #region States
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerWallSlideState WallSlideState{ get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    
    public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }
    public PlayerCounterAttackState CounterAttackState { get; private set; }


    #endregion
    
    
    protected override void Awake()
    {
        base.Awake();
        
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        AirState  = new PlayerAirState(this, StateMachine, "Jump");
        DashState = new PlayerDashState(this, StateMachine, "Dash");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, "Jump");
        
        PrimaryAttackState = new PlayerPrimaryAttackState(this, StateMachine, "Attack");
        CounterAttackState = new PlayerCounterAttackState(this, StateMachine, "CounterAttack");

    }

    protected override void Start()
    {
        base.Start();
        
        skill = SkillManager.instance;
        
        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        
        StateMachine.CurrentState.Update();
        
        CheckForDashInput();
    }

    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(seconds);

        isBusy = false;
    }

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    private void CheckForDashInput()
    {
        dashUsageTimer -= Time.deltaTime;
        
        if(IsWallDetected())
            return;
        
        if(Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            DashDir = Input.GetAxisRaw("Horizontal");

            if (DashDir == 0)
                DashDir = FacingDir;
            
            StateMachine.ChangeState(DashState);
        }
    }
}
