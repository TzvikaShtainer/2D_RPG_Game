using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodesList;
    
    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    
    private bool canGrow = true;
    private bool canShrink;
    private bool canCreateHotKey = true;
    private bool cloneAttackReleased;

    private int amountOfAttacks;
    private float cloneAttackCooldown;
    private float cloneAttackTimer;
    
    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKeys = new List<GameObject>();

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.X))
        {
            ReleaseCloneAttack();
        }
        
        CloneAttackLogic();
        
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime); //make the circle to grow until max over time
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1),shrinkSpeed * Time.deltaTime);
            
            if(transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    public void SetupBlackHole(float maxSize, float growSpeed, float shrinkSpeed, int amountOfAttacks, float cloneAttackCooldown)
    {
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
        this.shrinkSpeed = shrinkSpeed;
        this.amountOfAttacks = amountOfAttacks;
        this.cloneAttackCooldown = cloneAttackCooldown;
    }
    private void ReleaseCloneAttack()
    {
        DestroyHotKeys(); //destroy the hot keys that we make on top the enemies
        cloneAttackReleased = true;
        canCreateHotKey = false;
        
        PlayerManager.instance.player.MakeTransparent(true);
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer <= 0 && cloneAttackReleased)
        {
            cloneAttackTimer = cloneAttackCooldown;

            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;
            
            SkillManager.instance.Clone.CreateClone(targets[Random.Range(0, targets.Count)], new Vector3(xOffset, 0));
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                Invoke(nameof(FinishBlackHoleAbility), 0.5f);
            }
        }
    }

    private void FinishBlackHoleAbility()
    {
        PlayerManager.instance.player.ExitBlackHoleAbility();
        canShrink = true;
        cloneAttackReleased = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy.Enemy>() != null)
        {
            Enemy.Enemy enemy = other.GetComponent<Enemy.Enemy>();
            
            enemy.FreezeTime(true);
            CreateHotKey(other, enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Enemy.Enemy>() != null)
            other.GetComponent<Enemy.Enemy>().FreezeTime(false);
    }

    private void DestroyHotKeys()
    {
        if(createdHotKeys.Count <= 0 )
            return;

        for (int i = 0; i < createdHotKeys.Count; i++)
        {
           Destroy(createdHotKeys[i]);
        }
    }
    private void CreateHotKey(Collider2D other, Enemy.Enemy enemy)
    {
        if(keyCodesList.Count <= 0)
            return;
        
        if(!canCreateHotKey)
            return;
        
        GameObject newHotKey = Instantiate(hotKeyPrefab, other.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKeys.Add(newHotKey);

        KeyCode chosenKey = keyCodesList[Random.Range(0, keyCodesList.Count)];

        keyCodesList.Remove(chosenKey);

        BlackHole_HotKey_Controller newHotKeyScriptController = newHotKey.GetComponent<BlackHole_HotKey_Controller>();
        newHotKeyScriptController.SetupHotKey(chosenKey, enemy.transform, this);
    }

    public void AddEnemyToList(Transform newEnemyTransform) => targets.Add(newEnemyTransform);

}
