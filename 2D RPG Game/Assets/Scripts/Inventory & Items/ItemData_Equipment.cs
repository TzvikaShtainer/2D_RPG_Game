using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Unique Effects")]
    public float itemCooldown;
    public ItemEffect[] itemEffects;
    [TextArea]
    public string itemEffectDescription;
    
    [Header("Major Stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;
    
    [Header("Offensive Stats")]
    public int damage;
    public int critPower;
    public int critChance;
    
    [Header("Defensive Stats")]
    public int maxHealth;
    public int armor;
    public int evasion;
    public int magicResistance;
    
    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft Requirements")] 
    public List<InventoryItem> craftingMaterialsList;

    private int descriptionLength;
    private int minDescriptionLength = 5;

    public void ExecuteItemEffect(Transform enemyPos)
    {
        for (int i = 0; i < itemEffects.Length; i++)
        {
            itemEffects[i].ExecuteEffect(enemyPos);
        }
    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);
        
        playerStats.damage.AddModifier(damage);
        playerStats.critPower.AddModifier(critPower);
        playerStats.critChance.AddModifier(critChance);
        
        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);
        
        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);
        
        playerStats.damage.RemoveModifier(damage);
        playerStats.critPower.RemoveModifier(critPower);
        playerStats.critChance.RemoveModifier(critChance);
        
        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);
        
        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;
        
        AddItemDescription(strength ,"Strength");
        AddItemDescription(agility ,"agility");
        AddItemDescription(intelligence ,"intelligence");
        AddItemDescription(vitality ,"vitality");
        
        AddItemDescription(damage ,"damage");
        AddItemDescription(critPower ,"critPower");
        AddItemDescription(critChance ,"critChance");
        
        AddItemDescription(maxHealth ,"Health");
        AddItemDescription(armor ,"armor");
        AddItemDescription(evasion ,"evasion");
        AddItemDescription(magicResistance ,"magicResistance");
        
        AddItemDescription(fireDamage ,"fireDamage");
        AddItemDescription(iceDamage ,"iceDamage");
        AddItemDescription(lightingDamage ,"lightingDamage");

        if (descriptionLength < minDescriptionLength)
        {
            for (int i = 0; i < minDescriptionLength - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        if (itemEffectDescription.Length > 0)
        {
            sb.AppendLine();
            sb.Append(itemEffectDescription);
        }
        
        return sb.ToString();
    }

    private void AddItemDescription(int value, string name)
    {
        if (value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (value > 0)
                sb.Append("+ " +value + " " +name); // sb.Append(name + ": " + value);

            descriptionLength++;
        }
    }
}
