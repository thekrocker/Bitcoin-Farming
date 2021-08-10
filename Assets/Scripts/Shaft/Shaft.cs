using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaft : MonoBehaviour, IMineLocation
{

    [Header("Prefab")]
    [SerializeField] private BitcoinShaftMiner minerPrefab;
    [SerializeField] private Deposit depositPrefab;
    
    [Header("Managers")]

    [SerializeField] private ShaftWorkManager shaftWorkManagerPrefab;
    [SerializeField] private Transform shaftManagerPosition;
    
    
    [Header("Locations")]
    [SerializeField] private Transform miningLocation;
    [SerializeField] private Transform depositLocation;
    [SerializeField] private Transform depositCreationLocation;


    public int ShaftID { get; set; }
    
    public Transform MiningLocation => miningLocation;  // Burada BitcoinShaftMiner currentShaft propertiesinde kullanabilceğimiz referansları oluşturuyoruz. Yani kısaca üstte bulunan Transformları BCShaftMiner'a aktaracağız.
    public Transform DepositLocation => depositLocation;
    public Deposit ShaftDeposit { get; set; }
    public ShaftUI ShaftUI { get; set; }

    public List<BitcoinShaftMiner> Miners => _miners;
    private List<BitcoinShaftMiner> _miners = new List <BitcoinShaftMiner>();
    public ShaftWorkManager WorkManager { get; set; }
    private void Awake()
    {
        ShaftUI = GetComponent<ShaftUI>();
    }


    void Start()
    {

        CreateMiner();
        CreateDeposit();
        CreateManager();
    }


    public void CreateMiner()
    {
        BitcoinShaftMiner newMiner = Instantiate(minerPrefab, depositLocation.position, Quaternion.identity);
        newMiner.currentShaft = this;
        newMiner.transform.SetParent(transform);

        if (_miners.Count > 0) // if we have already 1 miner..  burada yeni gelecek miner'lerin statlarını, zaten var olan miner'ın statlarıyla eşitliyoruz.
        {
            newMiner.collectCapacity = _miners[0].collectCapacity;
            newMiner.collectPerSecond = _miners[0].collectPerSecond;
            newMiner.MoveSpeed = _miners[0].MoveSpeed;

        }
        _miners.Add(newMiner);

    }

    private void CreateDeposit()
    {

         ShaftDeposit = Instantiate(depositPrefab, depositCreationLocation.position, Quaternion.identity);
         ShaftDeposit.transform.SetParent(transform);

    }

    private void CreateManager()
    {
        WorkManager = Instantiate(shaftWorkManagerPrefab, shaftManagerPosition.position, Quaternion.identity);
        WorkManager.transform.SetParent(transform);
        WorkManager.CurrentMineLocation = this;

    }


    public void ApplyManagerBoost()
    {
        switch (WorkManager.ManagerAssigned.boostType)
        {
            case BoostType.MoveSpeed:
                foreach (BitcoinShaftMiner miner in _miners)
                {
                    WorkManagerController.Instance.RunMovementBoost(miner, WorkManager.ManagerAssigned.boostDuration, WorkManager.ManagerAssigned.boostValue);
                }
                break;
            
            case BoostType.Loading:
                foreach (BitcoinShaftMiner miner in _miners)
                {
                    WorkManagerController.Instance.RunLoadingBoost(miner, WorkManager.ManagerAssigned.boostDuration, WorkManager.ManagerAssigned.boostValue);
                }
                
                break;
        }
    }
}
