using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour
{
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;

    [SerializeField] private Color lockedSkillColor;
    
    
    public bool unlocked;
    
    [SerializeField] private UI_SkillSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillSlot[] shouldBeLocked;

    [SerializeField] private Image skillImage;

    private void OnValidate()
    {
        gameObject.name = "SkillSlot_UI - " + skillName;
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;
        
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    public void UnlockSkillSlot()
    {
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (!shouldBeUnlocked[i].unlocked)
                return;
        }
        
        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked)
                return;
        }

        unlocked = true;
        skillImage.color = Color.white;
    }
}
