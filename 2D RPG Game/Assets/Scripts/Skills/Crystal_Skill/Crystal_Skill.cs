using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;

    [Header("Crystal Mirage")]
    [SerializeField] private bool canCloneInsteadOfCrystal;
    
    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi Stacking Crystal")] 
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfCrystals;
    [SerializeField] private float multiCrystalCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalsLeft = new List<GameObject>();

    public override void UseSkill()
    {
        base.UseSkill();

        if(CanUseMultiCrystals())
            return;
        
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

            if (canCloneInsteadOfCrystal)
            {
                SkillManager.instance.Clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
        }
    }

    private void SwitchPlaceWithCrystal()
    {
        Vector2 playerPos = Player.transform.position;
        Player.transform.position = currentCrystal.transform.position;
        currentCrystal.transform.position = playerPos;
    }

    private bool CanUseMultiCrystals()
    {
        if (canUseMultiStacks)
        {
            if (crystalsLeft.Count == amountOfCrystals)
                Invoke(nameof(ResetAbility), useTimeWindow);
            
            if (crystalsLeft.Count > 0)
            {
                cooldown = 0;
                
                GameObject crystalToSpawn = crystalsLeft[crystalsLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, Player.transform.position, Quaternion.identity);
                
                crystalsLeft.Remove(crystalToSpawn);
                
                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform));

                if (crystalsLeft.Count <= 0)
                {
                    cooldown = multiCrystalCooldown;
                    RefillCrystals();
                }
                
                return true;
            }
        }

        return false;
    }
    
    private void RefillCrystals()
    {
        int amountToAdd = amountOfCrystals - crystalsLeft.Count;
        
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalsLeft.Add(crystalPrefab);
        }
    }
    
    private void ResetAbility()
    {
        if(cooldownTimer > 0)
            return;
        
        cooldownTimer = multiCrystalCooldown;
        RefillCrystals();
    }
}
