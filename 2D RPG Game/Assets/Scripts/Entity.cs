using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("collision Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    
    [Header("Attack Info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    
    public int FacingDir { get; private set; } = 1;
    protected bool facingRight = true;
    
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public EntityFX fx { get; private set; }
    
    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        
    }

    public virtual void Damage()
    {
        fx.StartCoroutine("FlashFX");
        Debug.Log("I Hit" + gameObject.name);
    }
    
    #region Collisions
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, wallCheckDistance, whatIsGround);
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion
    
    #region Flip
    public virtual void FlipController(float xInput)
    {
        if (xInput > 0 && !facingRight)
            Flip();
        else if(xInput < 0 && facingRight)
            Flip();
    }
    
    public virtual void Flip()
    {
        FacingDir = FacingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    #endregion
    
    #region Velocity
    public virtual void SetZeroVelocity() => Rb.velocity = new Vector2(0, 0);
    public virtual void SetVelocity(float xVelocity, float yVelocity)
    {
        Rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    #endregion
}
