using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;

    [Header("Crystal Mirage")]
    [SerializeField] private UI_SkillSlot unlockCloneInsteadButton;
    [SerializeField] private bool canCloneInsteadOfCrystal;

    [Header("Crystal Simple")]
    [SerializeField] private UI_SkillSlot unlockCrystalButton;
    public bool crystalUnlocked { get; private set; }
    
    
    [Header("Explosive Crystal")]
    [SerializeField] private UI_SkillSlot unlockExplosiveButton;
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private UI_SkillSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi Stacking Crystal")] 
    [SerializeField] private UI_SkillSlot unlockMultiStackButton;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfCrystals;
    [SerializeField] private float multiCrystalCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalsLeft = new List<GameObject>();


    protected override void Start()
    {
        base.Start();
        
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStackingCrystal);
    }

    #region Unlock Skill
    public void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
            crystalUnlocked = true;
    }
    
    public void UnlockCrystalMirage()
    {
        if (unlockCloneInsteadButton.unlocked)
            canCloneInsteadOfCrystal = true;
    }
    
    public void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveButton.unlocked)
            canExplode = true;
    }
    
    public void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
            canMoveToEnemy = true;
    }
    
    public void UnlockMultiStackingCrystal()
    {
        if (unlockMultiStackButton.unlocked)
            canUseMultiStacks = true;
    }
    #endregion
    public override void UseSkill()
    {
        base.UseSkill();

        if(CanUseMultiCrystals())
            return;
        
        if (currentCrystal == null)
        {
            CreateCrystal();
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

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, Player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
            
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), Player);
    }

    public void CurrentCrystalChooseRandomEnemy() =>
        currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

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
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), Player);

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
