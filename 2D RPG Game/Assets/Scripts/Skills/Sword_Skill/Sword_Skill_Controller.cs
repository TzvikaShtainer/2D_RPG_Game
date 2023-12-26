using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    private float freezeTimeDuration;
    private float returnSpeed = 12f;
    
    [Header("Pierce Info")]
    private float pierceAmount;
    
    [Header("bounce Info")]
    private float swordBouncingSpeed = 20;
    private bool isBouncing = false;
    private int bounceAmount;
    private List<Transform> enemyTarget = new List<Transform>();
    public int targetIndex;
    private float swordRadius = 10;
    
    [Header("Spin Info")]
    private float maxDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

    private float spinDirection;
    
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
                player.CatchSword();
        }

        BounceLogic();

        SpinLogic();
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position,
                swordBouncingSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy.Enemy>());
                
                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }
    
    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > maxDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime); 
                //makes the sword go forward when spinning

                if (spinTimer <= 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer <= 0) 
                {
                    hitTimer = hitCooldown;
                    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in hits)
                    {
                        if (hit.GetComponent<Enemy.Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy.Enemy>());
                    }
                }
            }
        }
    }
    
    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        spinTimer = spinDuration;
    }

    public void SetupSword(Vector2 dir, float gravityScale, Player player, float freezeTimeDuration, float swordTimeDuration, float returnSpeed)
    {
        this.player = player;
        this.freezeTimeDuration = freezeTimeDuration;
        this.returnSpeed = returnSpeed;
        
        rb.velocity = dir;
        rb.gravityScale = gravityScale;  
        
        if(pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1); //makes the sword go forward when spinning
        
        Invoke(nameof(DestroyMe), swordTimeDuration);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounce, float swordBouncingSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;
        this.swordBouncingSpeed = swordBouncingSpeed;
    }

    public void SetupPierce(int peirceAmount)
    {
        this.pierceAmount = peirceAmount;
    }

    public void SetupSpin(bool isSpinning, float maxDis, float spinDur, float hitCooldown)
    {
        this.isSpinning = isSpinning;
        maxDistance = maxDis;
        spinDuration = spinDur;
        this.hitCooldown = hitCooldown;
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
            Enemy.Enemy enemy = other.GetComponent<Enemy.Enemy>();

            SwordSkillDamage(enemy);
        }
        
        SetupTargetForBounce(other);
        
        StuckInto(other);
    }

    private void SwordSkillDamage(Enemy.Enemy enemy)
    {
        //enemy.DamageEffects();
        player.Stats.DoDamage(enemy.GetComponent<CharacterStats>());
        
        if(isSpinning)
            return;
        
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
        
        ItemData_Equipment equippedAmulet = Inventory.instance.GetEquippedItem(EquipmentType.Amulet);
                
        if(equippedAmulet != null)
            equippedAmulet.ExecuteItemEffect(enemy.transform);
    }

    private void SetupTargetForBounce(Collider2D other)
    {
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
    }

    private void StuckInto(Collider2D other)
    {
        if(pierceAmount > 0 && other.GetComponent<Enemy.Enemy>() != null)
        {
            pierceAmount--;
            return;
        }
        
        if(isSpinning)
        {
            StopWhenSpinning();
            return;
        }
        
        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if(isBouncing && enemyTarget.Count > 0)
            return;
        
        anim.SetBool("Rotation", false);
        transform.parent = other.transform;
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
