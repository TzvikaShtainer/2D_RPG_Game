using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescruption;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialsImage;

    public void SetupCraftWindow(ItemData_Equipment data)
    { 
        craftButton.onClick.RemoveAllListeners();
        
        CleanPrevData();

        SetMaterialsForCurrItem(data);

        SetCurrItemInfo(data);
        
        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(data, data.craftingMaterialsList));
    }

    private void CleanPrevData()
    {
        for (int i = 0; i < materialsImage.Length; i++)
        {
            materialsImage[i].color = Color.clear;
            materialsImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }
    }

    private void SetMaterialsForCurrItem(ItemData_Equipment data)
    {
        for (int i = 0; i < data.craftingMaterialsList.Count; i++)
        {
            if(data.craftingMaterialsList.Count > materialsImage.Length)
                Debug.LogWarning("you have more materials then space in craft window");
            
            materialsImage[i].sprite = data.craftingMaterialsList[i].data.icon;
            materialsImage[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialsImage[i].GetComponentInChildren<TextMeshProUGUI>();
            
            materialSlotText.text = data.craftingMaterialsList[i].stackSize.ToString();
            materialSlotText.color = Color.white;
        }
    }

    private void SetCurrItemInfo(ItemData_Equipment data)
    {
        itemIcon.sprite = data.icon;
        itemName.text = data.itemName;
        itemDescruption.text = data.GetDescription();
    }
}
