using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodesList;
    
    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;
    public bool canGrow;
    public bool canShrink;

    private bool canCreateHotKey = true;
    private bool cloneAttackReleased;
    public int amountOfAttacks = 4;
    public float cloneAttackCooldown = 0.3f;
    private float cloneAttackTimer;
    
    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKeys = new List<GameObject>();

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.X))
        {
            DestroyHotKeys(); //destroy the hot keys that we make on top the enemies
            cloneAttackReleased = true;
            canCreateHotKey = false;
        }
        
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
                canShrink = true;
                cloneAttackReleased = false;
            }
        }
        
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy.Enemy>() != null)
        {
            Enemy.Enemy enemy = other.GetComponent<Enemy.Enemy>();
            
            enemy.FreezeTime(true);
            CreateHotKey(other, enemy);
        }
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
