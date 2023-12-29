using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;  
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string description;
    
    private UI ui;

    private void OnValidate()
    {
        gameObject.name = "Stat -" + statName;

        if (statNameText != null)
            statNameText.text = statName;
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();
        
        UpdateStatType();
    }

    public void UpdateStatType()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetBaseValue().ToString();


            if (statType == StatType.maxHealth)
                statValueText.text = playerStats.GetMaxHealthValue().ToString();
            
            if (statType == StatType.damage)
                statValueText.text = (playerStats.damage.GetBaseValue() + playerStats.strength.GetBaseValue()).ToString();
            
            if (statType == StatType.critPower)
                statValueText.text = (playerStats.critPower.GetBaseValue() + playerStats.strength.GetBaseValue()).ToString();
            
            if (statType == StatType.critChance)
                statValueText.text = (playerStats.critChance.GetBaseValue() + playerStats.agility.GetBaseValue()).ToString();
            
            if (statType == StatType.evasion)
                statValueText.text = (playerStats.evasion.GetBaseValue() + playerStats.agility.GetBaseValue()).ToString();
            
            if (statType == StatType.magicResistance)
                statValueText.text = (playerStats.magicResistance.GetBaseValue() + (playerStats.intelligence.GetBaseValue() * playerStats.magicResPointFromIntelligent)).ToString();

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideToolTip();
    }
}
