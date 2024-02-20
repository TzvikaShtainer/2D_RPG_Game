using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }
    
    public override void Die()
    {
        base.Die();
        
        player.Die();

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    public override void DecreaseHealth(int _damage)
    {
        base.DecreaseHealth(_damage);

        ActivateArmorEffect();
    }

    private void ActivateArmorEffect()
    {
        ItemData_Equipment currentArmor = Inventory.instance.GetEquippedItem(EquipmentType.Armor);

        if (currentArmor != null)
        {
            currentArmor.ExecuteItemEffect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        player.Skill.Dodge.CreateMirageOnDoDodge();
    }
}
