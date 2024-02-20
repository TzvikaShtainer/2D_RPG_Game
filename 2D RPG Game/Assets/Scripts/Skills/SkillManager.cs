using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public CloneSkill Clone { get; private set; }
    public Dash_Skill Dash { get; private set; }
    public Sword_Skill Sword { get; private set; }
    public BlackHole_Skill BlackHole { get; private set; }
    public Crystal_Skill Crystal{ get; private set; }
    public Parry_Skill Parry{ get; private set; }
    public Dodge_Skill Dodge { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        Clone = GetComponent<CloneSkill>();
        Dash = GetComponent<Dash_Skill>();
        Sword = GetComponent<Sword_Skill>();
        BlackHole = GetComponent<BlackHole_Skill>();
        Crystal = GetComponent<Crystal_Skill>();
        Parry = GetComponent<Parry_Skill>();
        Dodge = GetComponent<Dodge_Skill>();
    }
}
