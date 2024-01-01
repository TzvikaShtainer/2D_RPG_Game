using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillNameText;

    public void ShowToolTip(string skillDescription, string skillName)
    {
        skillNameText.text = skillName;
        skillText.text = skillDescription;
        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false);
}
