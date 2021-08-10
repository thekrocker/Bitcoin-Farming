using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElevatorUI : MonoBehaviour
{
    public static Action<ElevatorUpgrade> OnUpgradeRequest;
    
    [SerializeField] private TextMeshProUGUI elevatorDepositedCoinText;
    [SerializeField] private TextMeshProUGUI currentLevel;
    
    
    private Elevator _elevator;
    private ElevatorUpgrade _elevatorUpgrade;

    private void Start()
    {
        _elevator = GetComponent<Elevator>();
        _elevatorUpgrade = GetComponent<ElevatorUpgrade>();

    }

    private void Update()
    {
        elevatorDepositedCoinText.text = _elevator.ElevatorDeposit.CurrentBitCoin.ToCurrency();
    }

    public void OpenElevatorUpgrade()
    {
        OnUpgradeRequest?.Invoke(_elevatorUpgrade);
    }

    
    private void UpgradeCompleted(BaseUpgrade upgrade)
    {
        if (_elevatorUpgrade == upgrade)
        {
            currentLevel.text = upgrade.CurrentLevel.ToString();
        }
        
        
    }
    private void OnEnable()
    {
        ElevatorUpgrade.OnUpgradeCompleted += UpgradeCompleted;
    }



    private void OnDisable()
    {
        ElevatorUpgrade.OnUpgradeCompleted -= UpgradeCompleted;

    }
}
