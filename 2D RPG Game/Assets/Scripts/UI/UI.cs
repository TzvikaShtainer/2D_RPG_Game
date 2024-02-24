using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject inGameUI;

    
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;

    public UI_SkillToolTip skillToolTip;

    private void Awake()
    {
        SwitchTo(skillTreeUI); //fix for not running skillTree and nor sigh the events to buttons bec its disable
    }

    private void Start()
    {
        SwitchTo(inGameUI);
        
        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
            SwitchWithKeyTo(characterUI);
        
        if(Input.GetKeyDown(KeyCode.L))
            SwitchWithKeyTo(skillTreeUI);
        
        if(Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(craftUI);
        
        if(Input.GetKeyDown(KeyCode.J))
            SwitchWithKeyTo(optionUI);
    }
    
    
    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        
        if(_menu != null)
            _menu.SetActive(true);
    }
    
    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        
        SwitchTo(_menu);
    }

    void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf)
                return;
        }
        
        SwitchTo(inGameUI);
    }
}
