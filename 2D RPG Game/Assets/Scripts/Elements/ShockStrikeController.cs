using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    [SerializeField] private int damage = 1;
    [SerializeField] private Animator anim;
    private bool triggered;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        ThunderAttack();
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        targetStats = _targetStats;
        damage = _damage;
        
    }

    private void ThunderAttack()
    {
        if(!targetStats)
            return;
        
        if(triggered)
            return;
        
        transform.position =
            Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        
        transform.right = transform.position - targetStats.transform.position;
        
        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {
            FixLightingPosAndRot();

            Invoke("DamageAndSelfDestroy", 0.2f);
            triggered = true; 
            anim.SetTrigger("Hit");
        }
    }

    private void FixLightingPosAndRot()
    {
        anim.transform.localPosition = new Vector3(0, 0.5f);
        anim.transform.localRotation = Quaternion.identity;
            
        transform.localRotation = Quaternion.identity;
        transform.localScale = new Vector3(3, 3);
    }

    public void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, 0.4f);
    }
}
