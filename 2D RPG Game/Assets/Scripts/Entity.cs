using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public EntityFX Fx { get; private set; }
    public SpriteRenderer Sr{ get; private set; }
    public CharacterStats Stats{ get; private set; }

    [Header("Knock-back Info")]
    [SerializeField] private Vector2 knockBackDirection;
    [SerializeField] private float knockBackDuration = 0.07f;
    protected bool IsKnocked;
    
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
    protected bool FacingRight = true;
    
    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        Sr = GetComponentInChildren<SpriteRenderer>();
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        Fx = GetComponent<EntityFX>();
        Stats = GetComponent<CharacterStats>();
    }

    protected virtual void Update()
    {
        
    }

    public virtual void DamageEffects()
    {
        Fx.StartCoroutine("FlashFX");
        
        StartCoroutine(nameof(HitKnockBack));
    }
    
    protected virtual IEnumerator HitKnockBack()
    {
        IsKnocked = true;

        Rb.velocity = new Vector2(knockBackDirection.x * -FacingDir, knockBackDirection.y);
        
        yield return new WaitForSeconds(knockBackDuration);
        
        IsKnocked = false;
    }
    
    #region Collisions
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, wallCheckDistance, whatIsGround);
    
    protected virtual void OnDrawGizmos()
    {
        var position = groundCheck.position;
        Gizmos.DrawLine(position, new Vector3(position.x, position.y - groundCheckDistance));
        
        var position1 = wallCheck.position;
        Gizmos.DrawLine(position1, new Vector3(position1.x + wallCheckDistance, position1.y));
        
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion
    
    #region Flip
    public virtual void FlipController(float xInput)
    {
        if (xInput > 0 && !FacingRight)
            Flip();
        else if(xInput < 0 && FacingRight)
            Flip();
    }
    
    public virtual void Flip()
    {
        FacingDir = FacingDir * -1;
        FacingRight = !FacingRight;
        transform.Rotate(0, 180, 0);
    }
    #endregion
    
    #region Velocity

    public virtual void SetZeroVelocity()
    {
        if(IsKnocked)
            return;
        
        Rb.velocity = new Vector2(0, 0);
    }
    public virtual void SetVelocity(float xVelocity, float yVelocity)
    {
        if(IsKnocked)
            return;
        
        Rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    #endregion

    public void MakeTransparent(bool transparent)
    {
        Debug.Log(transparent);
        if (transparent)
        {
            Sr.color = Color.clear;
            Debug.Log("Sr.color " +Sr.color);
        }
        else
        {
            Sr.color = Color.white;
            Debug.Log("Sr.color " +Sr.color);
        }
    }

    public virtual void Die()
    {
        
    }
}
