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
    

    public void SetupCrystal(float crystalDuration, bool canExplode, bool canMoveToEnemy, float moveSpeed)
    {
        crystalExistTimer = crystalDuration;
        this.canExplode = canExplode;
        this.canMoveToEnemy = canMoveToEnemy;
        this.moveSpeed = moveSpeed;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
            anim.SetTrigger("Explode");
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
