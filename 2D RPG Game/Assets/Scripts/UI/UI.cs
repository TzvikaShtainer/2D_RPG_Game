using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;

    public UI_ItemToolTip itemToolTip;

    private void Start()
    {
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        itemToolTip.HideToolTip();
    }

    public void SwitchTo(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        
        if(menu != null)
            menu.SetActive(true);
    }
}
