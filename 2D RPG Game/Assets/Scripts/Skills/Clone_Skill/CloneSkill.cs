using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration = 1.5f;
    [SerializeField] private float delayForCloneSpawn = 0.4f;
    [Space]
    [SerializeField] private bool canAttack;

    [SerializeField] private bool canCreateCloneOnDashStart;
    [SerializeField] private bool canCreateCloneOnDashOver;
    [SerializeField] private bool canCreateCloneOnCounterAttack;
    
    [Header("Clone Can Duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    
    [Header("Crystal Duplicate")]
    public bool canCrystalInsteadOfClone;
    
    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (canCrystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }
        
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().
            SetupClone(clonePosition, cloneDuration, canAttack, offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate, Player);
    }

    public void CanCreateCloneOnDashStart()
    {
        if(canCreateCloneOnDashStart)
            CreateClone(Player.transform, Vector3.zero);
    }
    
    public void CanCreateCloneOnDashOver()
    {
        if(canCreateCloneOnDashOver)
            CreateClone(Player.transform, Vector3.zero);
    }
    
    public void CanCreateCloneOnCounterAttack(Transform enemy)
    {
        if (canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(enemy, new Vector3(2 * Player.FacingDir, 0, 0)));

    }

    private IEnumerator CreateCloneWithDelay(Transform transform, Vector3 offset)
    {
        yield return new WaitForSeconds(delayForCloneSpawn);
        CreateClone(transform, offset);
    }
}
