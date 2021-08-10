using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AtmUI : MonoBehaviour
{

    public static Action<ATMUpgrade> OnUpgradeRequest;

    private ATMUpgrade _atmUpgrade;

    [SerializeField] private TextMeshProUGUI currentLevel;
    
    

    private void Start()
    {
        _atmUpgrade = GetComponent<ATMUpgrade>();
    }
//
    public void OpenATMUpgradePanel()
    {
        OnUpgradeRequest?.Invoke(_atmUpgrade);
        
    }
    
    private void UpgradeCompleted(BaseUpgrade upgrade)
    {
        if (_atmUpgrade == upgrade)
        {
            currentLevel.text = upgrade.CurrentLevel.ToString();
        }
        
        
    }
    private void OnEnable()
    {
        ATMUpgrade.OnUpgradeCompleted += UpgradeCompleted;
    }



    private void OnDisable()
    {
        ATMUpgrade.OnUpgradeCompleted -= UpgradeCompleted;

    }


}
