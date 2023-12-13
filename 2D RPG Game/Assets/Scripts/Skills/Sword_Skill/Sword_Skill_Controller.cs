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

    }

    public void SetupSword(Vector2 dir, float gravityScale, Player player)
    {
        this.player = player;
        
        rb.velocity = dir;
        rb.gravityScale = gravityScale;  
        
        anim.SetBool("Rotation", true);
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
        
        anim.SetBool("Rotation", false);
        
        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        transform.parent = other.transform;
    }
}
