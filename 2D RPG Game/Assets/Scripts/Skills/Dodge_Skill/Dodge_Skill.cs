using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")] 
    [SerializeField] private UI_SkillSlot unlockedDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;
    
    [Header("Dodge")] 
    [SerializeField] private UI_SkillSlot unlockedMirageDodgeButton;
    public bool mirageDodgeUnlocked;

    protected override void Start()
    {
        base.Start();   
        
        unlockedDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockedDodge);
        unlockedMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockedMirageDodge);
    }

    void UnlockedDodge()
    {
        if (unlockedDodgeButton.unlocked)
        {   
            Player.Stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }
    
    void UnlockedMirageDodge()
    {
        if (unlockedMirageDodgeButton.unlocked)
            mirageDodgeUnlocked = true;
    }

    public void CreateMirageOnDoDodge()
    {
        if (mirageDodgeUnlocked) 
            SkillManager.instance.Clone.CreateClone(Player.transform, new Vector3(2 * Player.FacingDir, 0));
    }
}
