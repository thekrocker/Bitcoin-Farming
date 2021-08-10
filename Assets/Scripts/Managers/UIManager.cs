﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI totalGoldText;

    private void Update()
    {
        totalGoldText.text = BCManager.Instance.CurrentBitcoin.ToCurrency();
    }
}
