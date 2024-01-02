using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked;
    [SerializeField] private UI_SkillSlot dashUnlockedButton;
    
    [Header("Clone On Dash")]
    public bool cloneOnDashUnlocked;
    [SerializeField] private UI_SkillSlot cloneOnDashUnlockedButton;
    
    [Header("Clone On Arrival")]
    public bool cloneOnArrivalUnlocked;
    [SerializeField] private UI_SkillSlot cloneOnArrivalUnlockedButton;


    protected override void Start()
    {
        base.Start();
        
        dashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    private void UnlockDash()
    {
        if (dashUnlockedButton.unlocked)
            dashUnlocked = true;
    }
    
    private void UnlockCloneOnDash()
    {
        if(cloneOnDashUnlockedButton.unlocked)
            cloneOnDashUnlocked = true;
    }
    
    private void UnlockCloneOnArrival()
    {
        if(cloneOnArrivalUnlockedButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }
    
    public void CloneOnDash()
    {
        if(cloneOnDashUnlocked)
            SkillManager.instance.Clone.CreateClone(Player.transform, Vector3.zero);
    }
    
    public void CloneOnArrival()
    {
        if(cloneOnArrivalUnlocked)
            SkillManager.instance.Clone.CreateClone(Player.transform, Vector3.zero);
    }

}
