using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackHoleImage;
    [SerializeField] private Image flaskImage;


    private SkillManager skills;

    private void Start()
    {
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;

        skills = SkillManager.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.Dash.DashUnlocked)
            SetCooldownOf(dashImage);
        
        if (Input.GetKeyDown(KeyCode.Q) && skills.Parry.parryUnlocked)
            SetCooldownOf(parryImage);
        
        if (Input.GetKeyDown(KeyCode.F) && skills.Crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);
        
        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.Sword.swordUnlocked)
            SetCooldownOf(swordImage);
        
        if (Input.GetKeyDown(KeyCode.C) && skills.BlackHole.blackHoleUnlocked)
            SetCooldownOf(blackHoleImage);
        
        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquippedItem(EquipmentType.Flask) != null)
            SetCooldownOf(flaskImage);
        
        CheckCooldownOf(dashImage, skills.Dash.cooldown);
        CheckCooldownOf(parryImage, skills.Parry.cooldown);
        CheckCooldownOf(crystalImage, skills.Crystal.cooldown);
        CheckCooldownOf(swordImage, skills.Sword.cooldown);
        CheckCooldownOf(blackHoleImage, skills.Sword.cooldown);
        
        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    void SetCooldownOf(Image image)
    {
        if (image.fillAmount <= 0)
            image.fillAmount = 1;
    }

    void CheckCooldownOf(Image image, float coolDown)
    {
        if (image.fillAmount > 0)
            image.fillAmount -= 1 / coolDown * Time.deltaTime;
    }
}
