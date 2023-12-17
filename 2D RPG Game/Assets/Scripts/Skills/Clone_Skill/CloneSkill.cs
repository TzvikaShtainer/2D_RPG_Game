using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration = 1.5f;
    [Space]
    [SerializeField] private bool canAttack;

    [SerializeField] private bool canCreateCloneOnDashStart;
    [SerializeField] private bool canCreateCloneOnDashOver;
    
    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(clonePosition, cloneDuration, canAttack, offset, FindClosestEnemy(newClone.transform));
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
}
