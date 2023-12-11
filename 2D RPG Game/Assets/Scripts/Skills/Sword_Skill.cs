using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Sword Skill Info")]
    [SerializeField] private Transform swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;
}
