using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillSlot dashUnlockedButton;
    public bool DashUnlocked {get; private set; }
    
    [Header("Clone On Dash")]
    [SerializeField] private UI_SkillSlot cloneOnDashUnlockedButton;
    public bool CloneOnDashUnlocked {get; private set; }
    
    [Header("Clone On Arrival")]
    [SerializeField] private UI_SkillSlot cloneOnArrivalUnlockedButton;
    public bool CloneOnArrivalUnlocked {get; private set; }


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
            DashUnlocked = true;
    }
    
    private void UnlockCloneOnDash()
    {
        if(cloneOnDashUnlockedButton.unlocked)
            CloneOnDashUnlocked = true;
    }
    
    private void UnlockCloneOnArrival()
    {
        if(cloneOnArrivalUnlockedButton.unlocked)
            CloneOnArrivalUnlocked = true;
    }
    
    public void CloneOnDash()
    {
        if(CloneOnDashUnlocked)
            SkillManager.instance.Clone.CreateClone(Player.transform, Vector3.zero);
    }
    
    public void CloneOnArrival()
    {
        if(CloneOnArrivalUnlocked)
            SkillManager.instance.Clone.CreateClone(Player.transform, Vector3.zero);
    }

}
