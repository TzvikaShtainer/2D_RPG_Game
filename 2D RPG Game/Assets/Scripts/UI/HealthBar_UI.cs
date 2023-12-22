using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
     private Entity entity;
     private CharacterStats myStats;
    private RectTransform healthBar;
    private Slider healthSlider;
    

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        healthBar = GetComponent<RectTransform>();
        healthSlider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();
    }
    private void OnEnable()
    {
        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }

    private void Start()
    {
        UpdateHealthUI();
    }
    
    private void FlipUI()
    {
        healthBar.Rotate(0, 180, 0);
    }

    public void UpdateHealthUI()
    {
        healthSlider.maxValue = myStats.GetMaxHealthValue();
        healthSlider.value = myStats.currentHealth; 
    }
}
