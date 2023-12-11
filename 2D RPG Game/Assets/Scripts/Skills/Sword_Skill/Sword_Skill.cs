using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Sword Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, Player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordSkillScript = newSword.GetComponent<Sword_Skill_Controller>();
        
        newSwordSkillScript.SetupSword(launchDir, swordGravity);
    }
}
