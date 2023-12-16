using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill : Skill
{
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown = 0.3f;

    private BlackHole_Skill_Controller currentBlackHole;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
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
        currentBlackHole.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown);
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
}
