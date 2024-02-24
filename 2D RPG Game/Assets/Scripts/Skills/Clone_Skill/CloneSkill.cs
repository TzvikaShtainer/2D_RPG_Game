using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration = 1.5f;
    [SerializeField] private float delayForCloneSpawn = 0.4f;
    [Space]
    
    [Header("Clone Attack")]
    [SerializeField] private UI_SkillSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;
    
    [Header("Aggressive Cloe")]
    [SerializeField] private UI_SkillSlot aggressiveCloneUnlockButton;
    [SerializeField] private float aggressiveCloneAttackMultiplier;
    public bool canApplyHitEffect { get; private set; }

    [Header("Multiple Clone")]
    [SerializeField] private UI_SkillSlot multipleUnlockButton;
    [SerializeField] private float multipleCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    
    [Header("Crystal Duplicate")]
    [SerializeField] private UI_SkillSlot crystalInsteadUnlockButton;
    public bool canCrystalInsteadOfClone;


    protected override void Start()
    {
        base.Start();
        
        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
    }

    #region UnlockSkills Region

    void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    void UnlockAggressiveClone()
    {
        if (aggressiveCloneUnlockButton.unlocked)
        {
            canApplyHitEffect = true;
            attackMultiplier = aggressiveCloneAttackMultiplier;
        }
    }

    void UnlockMultiClone()
    {
        if (multipleUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multipleCloneAttackMultiplier;
        }
    }

    void UnlockCrystalInstead()
    {
        if (crystalInsteadUnlockButton.unlocked)
            canCrystalInsteadOfClone = true;
    }

    #endregion
    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (canCrystalInsteadOfClone)
        {
            SkillManager.instance.Crystal.CreateCrystal();
            return;
        }
        
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().
            SetupClone(clonePosition, cloneDuration, canAttack, offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate, Player, attackMultiplier);
    }
    
    public void CreateCloneWithDelay(Transform enemy)
    {
        StartCoroutine(CloneDelayCoroutine(enemy, new Vector3(2 * Player.FacingDir, 0, 0)));
    }

    private IEnumerator CloneDelayCoroutine(Transform transform, Vector3 offset)
    {
        yield return new WaitForSeconds(delayForCloneSpawn);
        CreateClone(transform, offset);
    }
}
