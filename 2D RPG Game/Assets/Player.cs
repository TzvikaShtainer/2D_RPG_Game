using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Dash Info")]
    [SerializeField] private float dashCooldown = 1;
    private float dashUsageTimer;
    public float dashSpeed = 25f;
    public float dashDuration = 0.4f;
    public float DashDir { get; private set; }
    
    //public float wallSpeed = 0.5f;

    [Header("collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;

    public int FacingDir { get; private set; } = 1;
    private bool facingRight = true;
    
    #region Components
    public PlayerStateMachine StateMachine { get; private set; }
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }

    #endregion
    
    #region States
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerWallSlideState WallSlideState{ get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    

    #endregion
    
    
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        AirState  = new PlayerAirState(this, StateMachine, "Jump");
        DashState = new PlayerDashState(this, StateMachine, "Dash");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, "Jump");
    }

    private void Start()
    {
        Anim = GetComponentInChildren<Animator>(); //cuz its a child of player
        Rb = GetComponent<Rigidbody2D>();
        
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.Update();
        
        CheckForDashInput();
        Debug.Log(IsWallDetected());
    }
    
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

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        Rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, wallCheckDistance, whatIsGround);
    
    public void FlipController(float xInput)
    {
        if (xInput > 0 && !facingRight)
            Flip();
        else if(xInput < 0 && facingRight)
            Flip();
    }
    
    public void Flip()
    {
        FacingDir = FacingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
}
