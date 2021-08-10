using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;using UnityEngine;

public class Elevator : MonoBehaviour, IMineLocation
{


    [SerializeField] private Transform elevatorDepositLocation;
    [SerializeField] private Deposit elevatorDeposit;
    [SerializeField] private ElevatorMiner miner;

    
    [Header("Managers")]
    [SerializeField] private ElevatorWorkmanager elevatorWorkmanagerPrefab;
    [SerializeField] private Transform elevatorWorkManagerPosition;

    public ElevatorWorkmanager WorkManager { get; set; }
    public Deposit ElevatorDeposit => elevatorDeposit;
    public Transform ElevatorDepositLocation => elevatorDepositLocation;
    public ElevatorMiner Miner => miner;


    private void Start()
    {
        CreateManager();
    }

    private void CreateManager()
    {
        WorkManager = Instantiate(elevatorWorkmanagerPrefab, elevatorWorkManagerPosition.position, Quaternion.identity);
        WorkManager.transform.SetParent(transform);
        WorkManager.CurrentMineLocation = this;
    }
    
    public void ApplyManagerBoost()
    {
        switch (WorkManager.ManagerAssigned.boostType)
        {
                case BoostType.MoveSpeed:

                    WorkManagerController.Instance.RunMovementBoost(miner, WorkManager.ManagerAssigned.boostDuration, WorkManager.ManagerAssigned.boostValue);

                break;
                
                
            
                case BoostType.Loading:

                    WorkManagerController.Instance.RunLoadingBoost(miner, WorkManager.ManagerAssigned.boostDuration, WorkManager.ManagerAssigned.boostValue);

                    break;
        }
    }
    
}
