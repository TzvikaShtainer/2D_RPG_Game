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
    public float swordReturnImpact = 7f;

    [Header("Dash Info")]
    [SerializeField] private float dashCooldown = 1;
    private float dashUsageTimer;
    public float dashSpeed = 25f;
    public float dashDuration = 0.4f;
    public float DashDir { get; private set; }

    public SkillManager Skill { get; private set; }
    public GameObject Sword { get; private set; }
    
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
    
    public PlayerCatchSwordState CatchSwordState { get; private set; }
    public PlayerAimSwordState AimSwordState { get; private set; }
    public PlayerBlackHoleState blackHoleState{ get; private set; }


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
        
        CatchSwordState = new PlayerCatchSwordState(this, StateMachine, "CatchSword");
        AimSwordState = new PlayerAimSwordState(this, StateMachine, "AimSword");

        blackHoleState = new PlayerBlackHoleState(this, StateMachine, "Jump");

    }

    protected override void Start()
    {
        base.Start();
        
        Skill = SkillManager.instance;
        
        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        
        StateMachine.CurrentState.Update();
        
        CheckForDashInput();
        
        if(Input.GetKeyDown(KeyCode.F))
            Skill.crystal.UseSkill();
    }

    public void AssignNewSword(GameObject newSword)
    {
        Sword = newSword;
    }
    
    public void CatchSword()
    {
        StateMachine.ChangeState(CatchSwordState);
        Destroy(Sword);
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
        if(IsWallDetected())
            return;
        
        if(Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.Dash.CanUseSkill())
        {
            DashDir = Input.GetAxisRaw("Horizontal");

            if (DashDir == 0)
                DashDir = FacingDir;
            
            StateMachine.ChangeState(DashState);
        }
    }
}
