using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statDescription;
    
    public void ShowToolTip(string text)
    {
        statDescription.text = text;
        
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        statDescription.text = "";
        gameObject.SetActive(false);   
    }
}
