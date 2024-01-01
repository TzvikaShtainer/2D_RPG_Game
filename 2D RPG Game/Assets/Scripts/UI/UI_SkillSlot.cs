using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;

    [SerializeField] private Color lockedSkillColor;
    
    
    public bool unlocked;
    
    [SerializeField] private UI_SkillSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillSlot[] shouldBeLocked;

    [SerializeField] private Image skillImage;

    private UI ui;

    private void OnValidate()
    {
        gameObject.name = "SkillSlot_UI - " + skillName;
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;
        
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());

        ui = GetComponentInParent<UI>();
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
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName);

        DynamicToolTipPos();
    }

    private void DynamicToolTipPos()
    {
        Vector2 mousePos = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePos.x > 600)
            xOffset = -150;
        else
            xOffset = 150;

        if (mousePos.y > 600)
            yOffset = -150;
        else
            yOffset = 150;

        ui.skillToolTip.transform.position = new Vector2(mousePos.x + xOffset,mousePos.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }
}
