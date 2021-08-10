using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUpgrade : MonoBehaviour
{
    public static Action<BaseUpgrade> OnUpgradeCompleted;
    
    
    [Header("Upgrade")]
    [SerializeField] private float collectCapacityMultiplier = 2;
    [SerializeField] private float collectPerSecondMultiplier = 2;
    [SerializeField] private float moveSpeedMultiplier = 2;

    [Header("Cost")]
    [SerializeField] private float initialUpgradeCost = 600;
    [SerializeField] private float upgradeCostMultiplier = 2;

    public int CurrentLevel { get; set; }
    public float UpgradeCost { get; set; }
    public int BoostLevel { get; set; }
    public float CollectCapacityMultiplier => collectCapacityMultiplier;
    public float CollectPerSecondMultiplier => collectPerSecondMultiplier;
    public float MoveSpeedMultiplier => moveSpeedMultiplier;

    public float UpgradeCostMultiplier => upgradeCostMultiplier;

    protected Atm _atm;
    protected Shaft _shaft;
    protected Elevator _elevator;
    private int _currentNextBoostLevel = 1;
    private int _nextBoostResetValue = 1;
    
    private void Start()
    {
        _shaft = GetComponent<Shaft>();
        _elevator = GetComponent<Elevator>();
        _atm = GetComponent<Atm>();
        CurrentLevel = 1;
        UpgradeCost = initialUpgradeCost;
        BoostLevel = 10;
    }

    public void Upgrade(int amount)
    {
        if (amount > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                UpgradeCompleted();
                ExecuteUpgrade();
            }
        }
    }

    private void UpgradeCompleted()
    {
        BCManager.Instance.RemoveBC(UpgradeCost); // parayı sildik, upgradecost unu arttırdık ve leveli arttırdık. 
        UpgradeCost *= upgradeCostMultiplier;
        CurrentLevel++;
        UpdateNextBoostLevel();
        OnUpgradeCompleted?.Invoke(this);

    }

    protected virtual void ExecuteUpgrade()
    {
        
    }

    protected void UpdateNextBoostLevel()
    {
        _currentNextBoostLevel++;
        _nextBoostResetValue++;

        if (_currentNextBoostLevel == BoostLevel)  // If we reach boost level(10), make the boost level +10 more. 
        {
            _nextBoostResetValue = 1;
            BoostLevel += 10;

        }

    }

    public float GetNextBoostProgress()
    {
        return (float) _nextBoostResetValue / 10;
    }
    
    
    
}
