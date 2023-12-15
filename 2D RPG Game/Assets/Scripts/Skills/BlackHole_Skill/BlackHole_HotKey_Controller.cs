using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackHole_Skill_Controller blackhole;

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackhole.AddEnemyToList(myEnemy);
            
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }

    public void SetupHotKey(KeyCode newHotKey, Transform myEnemy, BlackHole_Skill_Controller blackhole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        this.myEnemy = myEnemy;
        this.blackhole = blackhole;
        
        myHotKey = newHotKey;
        myText.text = newHotKey.ToString();
    }
}
