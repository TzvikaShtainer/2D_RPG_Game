using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private Animator anim => GetComponent<Animator>();
    
    private float crystalExistTimer;
    
    private bool canExplode;
    
    private bool canMoveToEnemy;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;
    [SerializeField] private float maxSize = 3;
    
    private Transform closestEnemy;
    

    public void SetupCrystal(float crystalDuration, bool canExplode, bool canMoveToEnemy, float moveSpeed, Transform closestEnemy)
    {
        crystalExistTimer = crystalDuration;
        this.canExplode = canExplode;
        this.canMoveToEnemy = canMoveToEnemy;
        this.moveSpeed = moveSpeed;
        this.closestEnemy = closestEnemy;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canMoveToEnemy)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.transform.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestEnemy.transform.position) < 1)
            {
                FinishCrystal();
                canMoveToEnemy = false;
            }
        }
        
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime); //make the circle to grow until max over time
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    public void AnimationExplodeEvent()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in hits)
        {
            if (hit.GetComponent<Enemy.Enemy>() != null)
            {
                hit.GetComponent<Enemy.Enemy>().Damage();
            }
        }
    }
    
    private void SelfDestroy() => Destroy(gameObject);
}
