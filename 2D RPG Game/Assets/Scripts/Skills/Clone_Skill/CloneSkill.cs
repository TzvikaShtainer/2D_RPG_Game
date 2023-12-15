using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration = 1.5f;
    [Space]
    [SerializeField] private bool canAttack = true;
    
    public void CreateClone(Transform clonePosition)
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(clonePosition, cloneDuration, canAttack, Vector3.zero);
    }
}
