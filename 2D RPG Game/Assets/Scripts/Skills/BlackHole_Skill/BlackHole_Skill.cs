using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BlackHole_Skill : Skill
{
    [SerializeField] private UI_SkillSlot blackHoleUnlockButton;
    public bool blackHoleUnlocked { get; private set; }
    
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown = 0.3f;
    [SerializeField] private float blackHoleDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    private BlackHole_Skill_Controller currentBlackHole;


    protected override void Start()
    {
        base.Start();
        
        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
    }

    void UnlockBlackHole()
    {
        if (blackHoleUnlockButton.unlocked)
            blackHoleUnlocked = true;
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        CreateBlackHole();
    }

    private void CreateBlackHole ()
    {
        GameObject newBlackHole = Instantiate(blackHolePrefab, Player.transform.position, Quaternion.identity);
        currentBlackHole = newBlackHole.GetComponent<BlackHole_Skill_Controller>();
        currentBlackHole.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown, blackHoleDuration);
    }

    public bool BlackHoleCompleted()
    {
        if (!currentBlackHole)
            return false;
        
        if (currentBlackHole.PlayerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }

        return false;
    }

    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }
}
