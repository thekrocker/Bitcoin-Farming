using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WorkManagerController : Singleton<WorkManagerController>
{

    [SerializeField] private GameObject managerCardPrefab;
    [SerializeField] private Transform managerPanelContainer;
    [SerializeField] private List<WorkManagerInfo> availableManagers;
    [SerializeField] private GameObject managerPanel;

        [Header("AssignManager Card")]
    [SerializeField] private GameObject assignManagerCard;
    [SerializeField] private Image boostIcon;
    [SerializeField] private TextMeshProUGUI managerType;
    [SerializeField] private TextMeshProUGUI boostDuration;
    [SerializeField] private TextMeshProUGUI boostDescription;

    [SerializeField] private float initialManagerCost = 200;
    [SerializeField] private float managerCostMultiplier = 2;
    [SerializeField] private TextMeshProUGUI hireCost;
    

    public BaseWorkManager CurrentWorkManagerSelected { get; set; }
    public float CurrentManagerCost { get; set; }

    private List<WorkManagerCard> _workManagerCardsAssigned = new List<WorkManagerCard>();


    private void Start()
    {
        CurrentManagerCost = initialManagerCost;
    }

    private void Update()
    {
        hireCost.text = CurrentManagerCost.ToString();
    }

    public void OpenCloseManagerPanel(bool status)
    {
        managerPanel.SetActive(status);
        
    }
    
    #region Boost

    public void RunMovementBoost(BaseMiner miner, float duration, float value)
    {
        StartCoroutine(IEMovementBoost(miner, duration, value));
    }

    public void RunLoadingBoost(BaseMiner miner, float duration, float value)
    {
        StartCoroutine(IELoadingBoost(miner, duration, value));
    }

    private IEnumerator IEMovementBoost(BaseMiner miner, float duration, float value)
    {
        float startMovementSpeed = miner.MoveSpeed;
        miner.MoveSpeed /= value;
        yield return new WaitForSeconds(duration * 60);
        miner.MoveSpeed = startMovementSpeed;

    }

    private IEnumerator IELoadingBoost(BaseMiner miner, float duration, float value)
    {
        float startCollectPerSecond = miner.collectPerSecond;
        miner.collectPerSecond *= value;
        yield return new WaitForSeconds(duration * 60);
        miner.collectPerSecond = startCollectPerSecond;
    }
    
    
    
    
    #endregion
    
    
    public void HireManager() // Used on hire button.
    {

        if (availableManagers.Count > 0 && BCManager.Instance.CurrentBitcoin >= CurrentManagerCost)
        {
            GameObject managerCardGO = Instantiate(managerCardPrefab, managerPanelContainer);
            WorkManagerCard managerCard = managerCardGO.GetComponent<WorkManagerCard>();
            int randomManagerIndex = Random.Range(0, availableManagers.Count);
        
        
            WorkManagerInfo managerInfo = availableManagers[randomManagerIndex];
        
            managerCard.SetupWorkManagerCard(managerInfo);
            availableManagers.RemoveAt(randomManagerIndex);
            
            BCManager.Instance.RemoveBC(CurrentManagerCost);
            CurrentManagerCost *= managerCostMultiplier;

            Debug.Log(randomManagerIndex);
            
        }



    }

    public void UnassignManager()
    {
        RestoreManagerCardAssigned();
        UpdateAssignManagerCard();
    }
    
    private void RestoreManagerCardAssigned()
    {
        WorkManagerCard managerCardAssigned = null;
        for (int i = 0; i < _workManagerCardsAssigned.Count; i++)
        {
            if (CurrentWorkManagerSelected.ManagerAssigned == _workManagerCardsAssigned[i].ManagerInfoAssigned)
            {
                managerCardAssigned = _workManagerCardsAssigned[i];
            }
        }

        if (managerCardAssigned != null)
        {
            managerCardAssigned.gameObject.SetActive(true);
            _workManagerCardsAssigned.Remove(managerCardAssigned);
            CurrentWorkManagerSelected.ManagerAssigned = null;
        }
    }


    private void UpdateAssignManagerCard()
    {
        if (CurrentWorkManagerSelected.ManagerAssigned != null)
        {
            assignManagerCard.SetActive(true);
            boostIcon.sprite = CurrentWorkManagerSelected.ManagerAssigned.boostIcon;
            managerType.text = CurrentWorkManagerSelected.ManagerAssigned.managerType.ToString();
            managerType.color = CurrentWorkManagerSelected.ManagerAssigned.levelColor;
            boostDuration.text = $"Duration: {CurrentWorkManagerSelected.ManagerAssigned.boostDuration.ToString()}";
            boostDescription.text = CurrentWorkManagerSelected.ManagerAssigned.boostDescription;
            
            

        }
        else
        {
            assignManagerCard.SetActive(false);
        }
        
    }
    private void ManagerClicked(IMineLocation location)
    {
        if (location is Shaft shaft)
        {
            CurrentWorkManagerSelected = shaft.WorkManager;
        }
        
        else if (location is Elevator elevator)
        {
            CurrentWorkManagerSelected = elevator.WorkManager;
        }
        
        else if (location is Atm atm)
        {
            CurrentWorkManagerSelected = atm.WorkManager;
        }

        if (CurrentWorkManagerSelected.ManagerAssigned == null)
        {
            assignManagerCard.SetActive(false);
        }
        else
        {
            assignManagerCard.SetActive(true);

        }
        
        
        UpdateAssignManagerCard();
        OpenCloseManagerPanel(true);
    }

    private void WorkManagerCardAssigned(WorkManagerCard managerCard)
    {
        _workManagerCardsAssigned.Add(managerCard);
        CurrentWorkManagerSelected.ManagerAssigned = managerCard.ManagerInfoAssigned;
        CurrentWorkManagerSelected.SetupBoostButton();
        UpdateAssignManagerCard();
    }
    private void OnEnable()
    {
        BaseWorkManager.OnManagerClicked += ManagerClicked;
        WorkManagerCard.OnAssignRequest += WorkManagerCardAssigned;
    }

    private void OnDisable()
    {
        BaseWorkManager.OnManagerClicked -= ManagerClicked;
        WorkManagerCard.OnAssignRequest -= WorkManagerCardAssigned;



    }
}
