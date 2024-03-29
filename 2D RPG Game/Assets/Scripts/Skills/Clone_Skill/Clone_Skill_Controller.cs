using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;
    
    private float cloneTimer;
    private float attackMultiplier;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.8f;
    private Transform closestEnemy;

    [SerializeField] private bool canDuplicateClone;
    private float chanceToDuplicate;
    private int facingDir = 1;

    private Player player;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1,1,1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
            
            if(sr.color.a <= 0)
                Destroy(gameObject);
        }
    }

    public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack, Vector3 offset, Transform closestEnemy, bool canDuplicateClone, float changeToDuplicate, Player player, float attackMultiplier)
    {
        if (canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        
        transform.position = newTransform.position + offset;
        cloneTimer = cloneDuration;

        this.closestEnemy = closestEnemy;
        FaceClosestTarget();

        this.canDuplicateClone = canDuplicateClone;
        this.chanceToDuplicate = changeToDuplicate;
        this.player = player;
        this.attackMultiplier = attackMultiplier;
    }
    
    private void AnimationTrigger()
    {
        cloneTimer = -0.1f; //just to destroy the clone bec of the time, dont like this way
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy.Enemy>() != null)
            {
                //player.Stats.DoDamage( hit.GetComponent<CharacterStats>());
                //hit.GetComponent<Enemy.Enemy>().DamageEffects();
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
                
                playerStats.CloneDoDamage(enemyStats, attackMultiplier);

                if (player.Skill.Clone.canApplyHitEffect)
                {
                    ItemData_Equipment weaponData = Inventory.instance.GetEquippedItem(EquipmentType.Weapon);
                    if (weaponData != null)
                        weaponData.ExecuteItemEffect(hit.transform);
                }
                
                if (canDuplicateClone)
                {
                    if (Random.Range(1, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.Clone.CreateClone(hit.transform, new Vector3(0.5f * facingDir, 0));
                    }
                }
            }
        }
    }
    
    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                transform.Rotate(0, 180, 0);
                facingDir = -1;
            }
        }
    }
}
