using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12f;
    
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    [Header("bounce Info")]
    [SerializeField] private float swordBouncingSpeed = 20;
    private bool isBouncing = false;
    private int amountOfBounce;
    private List<Transform> enemyTarget = new List<Transform>();
    public int targetIndex;
    private float swordRadius = 10;
    
    
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if(canRotate)
            transform.right = rb.velocity; //make the sword to go the dir of throw  

        if (isReturning)
        { 
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 2)
                player.CatchhSword();
        }

        BounceLogic();
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position,
                swordBouncingSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                targetIndex++;
                amountOfBounce--;

                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    public void SetupSword(Vector2 dir, float gravityScale, Player player)
    {
        this.player = player;
        
        rb.velocity = dir;
        rb.gravityScale = gravityScale;  
        
        anim.SetBool("Rotation", true);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounce)
    {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounce;
    }
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning)
            return;

        if (other.GetComponent<Enemy.Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, swordRadius);

                foreach (var hit in hits)
                {
                    if (hit.GetComponent<Enemy.Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
        
        StuckInto(other);
    }

    private void StuckInto(Collider2D other)
    {

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if(isBouncing && enemyTarget.Count > 0)
            return;
        
        anim.SetBool("Rotation", false);
        transform.parent = other.transform;
    }
}
