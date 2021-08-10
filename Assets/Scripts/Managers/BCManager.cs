using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCManager : Singleton<BCManager>
{

    public float CurrentBitcoin { get; set; }
    private readonly string BC_KEY = "MY_BITCOIN";
    [SerializeField] private float testBC = 0;


    private void Start()
    {
        PlayerPrefs.DeleteAll();
        LoadBC();
    }

    public void AddBC(float amount)
    {
        CurrentBitcoin += amount;
        PlayerPrefs.SetFloat(BC_KEY, CurrentBitcoin);
        PlayerPrefs.Save();
    }

    public void RemoveBC(float amount)
    {

        if (amount <= CurrentBitcoin)
        {
            CurrentBitcoin -= amount;

        }
        PlayerPrefs.SetFloat(BC_KEY, CurrentBitcoin);
        PlayerPrefs.Save();
    }

    private void LoadBC()
    {
        CurrentBitcoin = PlayerPrefs.GetFloat(BC_KEY, testBC);
    }
    

}
