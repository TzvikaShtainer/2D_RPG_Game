using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    public override void UseSkill()
    {
        base.UseSkill();

        if (currentCrystal == null)
            currentCrystal = Instantiate(crystalPrefab, Player.transform.position, Quaternion.identity);
        else
        {
            Player.transform.position = currentCrystal.transform.position;
            Destroy(currentCrystal);
        }
    }
}
