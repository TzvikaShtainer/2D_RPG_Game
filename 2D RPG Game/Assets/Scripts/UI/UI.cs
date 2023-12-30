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

    
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;
    private void Start()
    {
        SwitchTo(null);
        
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
            return;
        }
        
        SwitchTo(_menu);
    }
}
