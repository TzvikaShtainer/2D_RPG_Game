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
    }
}
