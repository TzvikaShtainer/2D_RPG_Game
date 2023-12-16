using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;

    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    public override void UseSkill()
    {
        base.UseSkill();

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, Player.transform.position, Quaternion.identity);
            Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
            
            currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform));
        }
        else
        {
            if(canMoveToEnemy)
                return;
            
            SwitchPlaceWithCrystal();

            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
        }
    }

    private void SwitchPlaceWithCrystal()
    {
        Vector2 playerPos = Player.transform.position;
        Player.transform.position = currentCrystal.transform.position;
        currentCrystal.transform.position = playerPos;
    }
}
