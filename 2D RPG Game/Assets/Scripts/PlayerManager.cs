using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;

    public int currency;

    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public bool HaveEnoughCurrency(int price)
    {
        if (price > currency)
        {
            Debug.Log("you poor for that");
            return false;
        }

        currency -= price;
        return true;
    }

    public int GetCurrency() => currency;
}
