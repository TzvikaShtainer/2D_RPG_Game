using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] UI_SkillSlot parryUnlockedButton;
    public bool parryUnlocked;
    
    [Header("Parry Restore")]
    [SerializeField] UI_SkillSlot parryRestoreUnlockedButton;
    public bool parryRestoreUnlocked;
    [Range(0, 1)]
    [SerializeField] private float parryHealthPercentageToRestore;
    
    [Header("Parry With Mirage")]
    [SerializeField] UI_SkillSlot parryWithMirageUnlockedButton;
    public bool parryWithMirageUnlocked;

    private void Start()
    {
        parryUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        parryRestoreUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    public override void UseSkill()
    {
        base.UseSkill();
        
        if (parryRestoreUnlocked)
        {
            Debug.Log(Player);
            int amount = Mathf.RoundToInt(parryHealthPercentageToRestore * Player.Stats.GetMaxHealthValue());
            Player.Stats.IncreaseHealth(amount);
        }
    }

    public void UnlockParry()
    {
        if(parryUnlockedButton.unlocked)
            parryUnlocked = true;
    }
    
    public void UnlockParryRestore()
    {
        if(parryRestoreUnlockedButton.unlocked)
            parryRestoreUnlocked = true;
    }
    
    public void UnlockParryWithMirage()
    {
        if(parryWithMirageUnlockedButton.unlocked)
            parryWithMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform enemy)
    {
        if(parryWithMirageUnlocked)
            SkillManager.instance.Clone.CreateCloneWithDelay(enemy);
    }
}
