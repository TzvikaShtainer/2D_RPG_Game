using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodesList;
    
    public float maxSize;
    public float growSpeed;
    public bool canGrow;

    private bool canAttack;
    public int amountOfAttacks = 4;
    public float cloneAttackCooldown = 0.3f;
    private float cloneAttackTimer;
    
    List<Transform> targets = new List<Transform>();

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
            canAttack = true;
        
        if (cloneAttackTimer <= 0 && canAttack)
        {
            cloneAttackTimer = cloneAttackCooldown;
            SkillManager.instance.Clone.CreateClone(targets[Random.Range(0, targets.Count)]);
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
                canAttack = false;
        }
        
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime); //make the circle to grow until max over time
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

    private void CreateHotKey(Collider2D other, Enemy.Enemy enemy)
    {
        if(keyCodesList.Count <= 0)
            return;
        
        GameObject newHotKey = Instantiate(hotKeyPrefab, other.transform.position + new Vector3(0, 2),
            Quaternion.identity);

        KeyCode chosenKey = keyCodesList[Random.Range(0, keyCodesList.Count)];

        keyCodesList.Remove(chosenKey);

        BlackHole_HotKey_Controller newHotKeyScriptController = newHotKey.GetComponent<BlackHole_HotKey_Controller>();
        newHotKeyScriptController.SetupHotKey(chosenKey, enemy.transform, this);
    }

    public void AddEnemyToList(Transform newEnemyTransform) => targets.Add(newEnemyTransform);

}
