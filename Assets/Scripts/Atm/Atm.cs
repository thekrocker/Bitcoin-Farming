using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Atm : MonoBehaviour, IMineLocation
{
    [Header("Prefab")]
    [SerializeField] private AtmMiner minerPrefab;

    
    [Header("Extras")]

    [SerializeField] private Deposit elevatorDeposit;
    [SerializeField] private Transform elevatorDepositLocation;
    [SerializeField] private Transform AtmDepositLocation;

    
    [Header("Managers")]
    [SerializeField] private AtmWorkManager atmWorkManagerPrefab;
    [SerializeField] private Transform atmWorkManagerPosition;

    public AtmWorkManager WorkManager { get; set; }



    private List<AtmMiner> _miners = new List<AtmMiner>();
    public List<AtmMiner> Miners => _miners;
    
    void Start()
    {
        AddMiner();
        CreateManager();

    }


    public void AddMiner()
    {
        AtmMiner newMiner =  Instantiate(minerPrefab, AtmDepositLocation.position, Quaternion.identity);
        newMiner.ElevatorDeposit = this.elevatorDeposit;
        newMiner.ElevatorDepositLocation =
            new Vector3(elevatorDepositLocation.position.x, AtmDepositLocation.position.y);
        newMiner.AtmLocation = new Vector3(AtmDepositLocation.position.x, AtmDepositLocation.position.y);

        if (_miners.Count > 0)
        {
            newMiner.collectCapacity = _miners[0].collectCapacity;
            newMiner.collectPerSecond = _miners[0].collectPerSecond;
            newMiner.MoveSpeed = _miners[0].MoveSpeed;
        }
        
        _miners.Add(newMiner);
        
    }

    public void CreateManager()
    {
        WorkManager = Instantiate(atmWorkManagerPrefab, atmWorkManagerPosition.position, Quaternion.identity);
        WorkManager.transform.SetParent(transform);
        WorkManager.CurrentMineLocation = this;

    }
    
    public void ApplyManagerBoost()
    {
        switch (WorkManager.ManagerAssigned.boostType)
        {
            case BoostType.MoveSpeed:
                foreach (AtmMiner miner in _miners)
                {
                    WorkManagerController.Instance.RunMovementBoost(miner, WorkManager.ManagerAssigned.boostDuration, WorkManager.ManagerAssigned.boostValue);
                }
                break;
            
            case BoostType.Loading:
                foreach (AtmMiner miner in _miners)
                {
                    WorkManagerController.Instance.RunLoadingBoost(miner, WorkManager.ManagerAssigned.boostDuration, WorkManager.ManagerAssigned.boostValue);
                }
                
                break;
        }
    }
}
