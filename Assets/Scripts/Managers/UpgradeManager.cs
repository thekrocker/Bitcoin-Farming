using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradeContainer;
    [SerializeField] private Image panelMinerImage;
    [SerializeField] private TextMeshProUGUI panelTitle;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI nextBoost;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private Image progressBar;
    [Header("Stat Title")] 
    [SerializeField] private TextMeshProUGUI stat1Title;
    [SerializeField] private TextMeshProUGUI stat2Title;
    [SerializeField] private TextMeshProUGUI stat3Title;
    [SerializeField] private TextMeshProUGUI stat4Title;

    [Header("Upgr Buttons")] 
    [SerializeField] private GameObject[] upgradeButtons;
    [SerializeField] private Color buttonDisableColor;
    [SerializeField] private Color buttonEnableColor;

    [Header("Stats")]
    [SerializeField] private GameObject[] stats;
    
    [Header("Stat Values")] 
    [SerializeField] private TextMeshProUGUI stat1CurrentValue;
    [SerializeField] private TextMeshProUGUI stat2CurrentValue;
    [SerializeField] private TextMeshProUGUI stat3CurrentValue;
    [SerializeField] private TextMeshProUGUI stat4CurrentValue;
    
    
    [Header("Upgrade Values")] 
    [SerializeField] private TextMeshProUGUI stat1UpgradeValue;
    [SerializeField] private TextMeshProUGUI stat2UpgradeValue;
    [SerializeField] private TextMeshProUGUI stat3UpgradeValue;
    [SerializeField] private TextMeshProUGUI stat4UpgradeValue;
    
    
    [Header("Stat Icons")] 
    [SerializeField] private Image stat1Icon;
    [SerializeField] private Image stat2Icon;
    [SerializeField] private Image stat3Icon;
    [SerializeField] private Image stat4Icon;

    [Header("Panel Info")] [SerializeField]
    private UpgradePanelInfo shaftMinerInfo;
    [SerializeField] private UpgradePanelInfo elevatorMinerInfo;
    [SerializeField] private UpgradePanelInfo atmMinerInfo;


    public int UpgradeAmount { get; set; }  
    
    private Shaft _currentShaft;
    private BaseUpgrade _currentUpgrade;
    private UpgradePanelInfo _currentPanelInfo;
    private BaseMiner _currentMiner;
    private int _currentActiveButton;
    private int _minerCount;

    

    private void UpdateUpgradeInfo()
    {
        if (_currentPanelInfo.location == Locations.Elevator)
        {
            stats[3].SetActive(false);
        }
        else
        {
            stats[3].SetActive(true);

        }
        
        panelTitle.text = _currentPanelInfo.panelTitle;
        panelMinerImage.sprite = _currentPanelInfo.panelMinerIcon;

        stat1Title.text = _currentPanelInfo.stat1Title;
        stat2Title.text = _currentPanelInfo.stat2Title;
        stat3Title.text = _currentPanelInfo.stat3Title;
        stat4Title.text = _currentPanelInfo.stat4Title;

        // ShaftMinerInfodan aldığımız bilgileri, bu UpgradeManager GameObject'inde bulunan objelerle referans oluşturduk. Stat1icon örneğin UpgradeManager içinde bulunan  Stat1 gameObjecti(textmesh) olacak. 
        stat1Icon.sprite = _currentPanelInfo.stat1Icon;
        stat2Icon.sprite = _currentPanelInfo.stat2Icon;
        stat3Icon.sprite = _currentPanelInfo.stat3Icon;
        stat4Icon.sprite = _currentPanelInfo.stat4Icon;
        

    }

    public void OpenCloseUpgradeContainer(bool status)
    {
        UpgradeX1(false);

        upgradeContainer.SetActive(status);
    }

    public void Upgrade()
    {
        if (BCManager.Instance.CurrentBitcoin >= _currentUpgrade.UpgradeCost) // if we have enough money....
        {
            _currentUpgrade.Upgrade(UpgradeAmount);
            UpdatePanelValues();
            RefreshUpgradeAmount();
        }
        
    }

    #region Upgrade Buttons

    public void UpgradeX1(bool animateButton)
    {
        ActivateButton(0, animateButton);
        UpgradeAmount = CanUpgradeManyTimes(1, _currentUpgrade) ? 1 : 0;
        upgradeCost.text = GetUpgradeCost(1, _currentUpgrade).ToCurrency();
    }
    
    public void UpgradeX10(bool animateButton)
    {
        
        ActivateButton(1, animateButton);
        UpgradeAmount = CanUpgradeManyTimes(10, _currentUpgrade) ? 10 : 0;  // Eğer 10 kere upgrade ediliyorsa, 10 kere upgrade et, edilmiyorsa 0. 
        upgradeCost.text = GetUpgradeCost(10, _currentUpgrade).ToCurrency();

    }
    
    public void UpgradeX50(bool animateButton)
    {
        ActivateButton(2, animateButton);
        UpgradeAmount = CanUpgradeManyTimes(50, _currentUpgrade) ? 50 : 0; 
        upgradeCost.text = GetUpgradeCost(50, _currentUpgrade).ToCurrency();
        

    }
    
    public void UpgradeMax(bool animateButton)
    {
        ActivateButton(3, animateButton);
        UpgradeAmount = CalculateUpgradeCount(_currentUpgrade);
        upgradeCost.text = GetUpgradeCost(UpgradeAmount, _currentUpgrade).ToCurrency();



    }
    
    private void ActivateButton(int buttonIndex, bool animateButton)
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].GetComponent<Image>().color = buttonDisableColor;
        }

        _currentActiveButton = buttonIndex;
        upgradeButtons[buttonIndex].GetComponent<Image>().color = buttonEnableColor;

        if (animateButton)
        {
            upgradeButtons[buttonIndex].transform.DOPunchPosition(transform.localPosition + new Vector3(0f, -5f, 0f), 0.5f)
                .Play();
            
        }

        
    }

    private void RefreshUpgradeAmount() // tekrar basmadığımızda hala upgrade 10 olarak görünüyordu. bu yöntemle, updateleyeceğiz. 
    {
        switch (_currentActiveButton)
        {
            case 0:
                UpgradeX1(false);
                break;
            
            case 1:
                UpgradeX10(false);
                break;
            case 2:
                UpgradeX50(false);
                break;
            case 3:
                UpgradeMax(false);
                break;
        }
    }

    private int CalculateUpgradeCount(BaseUpgrade upgrade)
    {
        int count = 0;
        float currentBC = BCManager.Instance.CurrentBitcoin;
        float currentUpgradeCost = upgrade.UpgradeCost;

        if (BCManager.Instance.CurrentBitcoin >= currentUpgradeCost)
        {   
            for (float i = currentBC; i >= 0; i-= currentUpgradeCost)
            {
                count++;
                currentUpgradeCost *= upgrade.UpgradeCostMultiplier;

            }
            
        }

        return count;
    }

    private bool CanUpgradeManyTimes(int upgradeAmount, BaseUpgrade upgrade)
    {
        int count = CalculateUpgradeCount(upgrade);
        if (count >= upgradeAmount)
        {
            return true;
        }

        return false;

    }

    private float GetUpgradeCost(int amount, BaseUpgrade upgrade)
    {
        float cost = 0f;
        float currentUpgradeCost = upgrade.UpgradeCost;

        for (int i = 0; i < amount; i++)
        {
            cost += currentUpgradeCost;
            currentUpgradeCost *= upgrade.UpgradeCostMultiplier;
        }

        return cost;
    }

    #endregion

 

    private void UpdatePanelValues()
    {
        upgradeCost.text = _currentUpgrade.UpgradeCost.ToString();
        level.text = $"Level {_currentUpgrade.CurrentLevel}";

        progressBar.DOFillAmount(_currentUpgrade.GetNextBoostProgress(), 0.5f).Play();
        nextBoost.text = $"Next Boost at Level {_currentUpgrade.BoostLevel}";
        
        
        
        
        // Walk Speed Upgrade
        float minerMoveSpeed = _currentMiner.MoveSpeed;
        float minerMoveSpeedUpgraded =
            Mathf.Abs(minerMoveSpeed - (minerMoveSpeed * _currentUpgrade.MoveSpeedMultiplier));

        // MiningSpeed Upgrade
        
        float minerCollectPerSecond = _currentMiner.collectPerSecond;  // İlk sıradaki miner'in yani bitcoin miner'in saniye başı toplaması.. minerCollectPersec değişikenine ait.
        float collectPerSecondUpgrade = Mathf.Abs(minerCollectPerSecond -
                                                  (minerCollectPerSecond * _currentUpgrade.CollectPerSecondMultiplier));
        
        // Capacity Upgrade
        float minerCollectCapacity = _currentMiner.collectCapacity;  // İlk sıradaki miner'in yani bitcoin miner'in collect capacitysi.. minerCollectCapcity değişikenine ait.
        float collectCapacityUpgrade =
            Mathf.Abs(minerCollectCapacity - (minerCollectCapacity * _currentUpgrade.CollectCapacityMultiplier));
// Üstte bulunan Math.ABS formülü = Math.ABS(200 - (200 * 2) = -200 yapar. Abs ile pozitif 200e dönüşür. 


        if (_currentPanelInfo.location == Locations.Elevator)
        {
            stat1CurrentValue.text = $"{_currentMiner.collectCapacity}";
            stat2CurrentValue.text = $"{_currentMiner.MoveSpeed}";
            stat3CurrentValue.text = $"{_currentMiner.collectPerSecond}";
            
            stat1CurrentValue.text = $"{_currentMiner.collectCapacity}"; // Collect Capacity
            stat2UpgradeValue.text = (_currentUpgrade.CurrentLevel + 1) % 10 == 0 ? $"+{minerMoveSpeedUpgraded}/s" : "0";
            stat3UpgradeValue.text = $"+{collectPerSecondUpgrade}";
            // This is for ELEVATOR UpgradePanel
        }
        else  // if we are opening shaft or warehouse
        {
            //Current Values 
            stat1CurrentValue.text = $"{_minerCount}"; // Miner Count
            stat2CurrentValue.text = $"{_currentMiner.MoveSpeed}"; // Walk speed
            stat3CurrentValue.text = $"{_currentMiner.collectPerSecond}"; // Mining Speed
            stat4CurrentValue.text = $"{_currentMiner.collectCapacity}"; // Collect Capacity

        
        
            // Miner Upgrade Count 
            stat1UpgradeValue.text = (_currentUpgrade.CurrentLevel + 1) % 10 == 0 ? "+1" : "+0";   //  % 10 == 0 ?  demek şuanki levelin +1 'i  10 ve 10 un katıysa. +1 yaz, değilse +0 yaz. // if in kısa yolu bu ? : 
            stat2UpgradeValue.text = (_currentUpgrade.CurrentLevel + 1) % 10 == 0 ? $"+{minerMoveSpeedUpgraded}/s" : "0";
            stat3UpgradeValue.text = $"+{collectPerSecondUpgrade}";
            stat4UpgradeValue.text = $"+{collectCapacityUpgrade}";
        }

        

    }
    private void ShaftUpgradeRequest(Shaft selectedShaft, ShaftUpgrade selectedUpgrade)
    {
      //  _currentShaft = selectedShaft;
      _minerCount = selectedShaft.Miners.Count;
        _currentMiner = selectedShaft.Miners[0];  // Listeed 0. indekse sahip miner'i al. yani shaft miner.
        _currentUpgrade = selectedUpgrade;
        _currentPanelInfo = shaftMinerInfo;

        UpdateUpgradeInfo();
        UpdatePanelValues();
        OpenCloseUpgradeContainer(true);
    }

    private void ElevatorUpgradeRequest(ElevatorUpgrade selectedUpgrade)
    {
        _currentPanelInfo = elevatorMinerInfo;
        _currentUpgrade = selectedUpgrade;
        _currentMiner = selectedUpgrade.GetComponent<Elevator>().Miner;

        
        UpdateUpgradeInfo();
        UpdatePanelValues();
        OpenCloseUpgradeContainer(true);
    }
    
    private void AtmUpgradeRequest(ATMUpgrade atmUpgrade)
    {
        _minerCount = atmUpgrade.GetComponent<Atm>().Miners.Count;

        _currentMiner = atmUpgrade.GetComponent<Atm>().Miners[0];
        _currentUpgrade = atmUpgrade;
        _currentPanelInfo = atmMinerInfo;
        
        UpdateUpgradeInfo();
        UpdatePanelValues();
        OpenCloseUpgradeContainer(true);

        
        
    }
    private void OnEnable()
    {
        ShaftUI.OnUpgradeRequest += ShaftUpgradeRequest;
        ElevatorUI.OnUpgradeRequest += ElevatorUpgradeRequest;
        AtmUI.OnUpgradeRequest += AtmUpgradeRequest;
    }



    private void OnDisable()
    {
        ShaftUI.OnUpgradeRequest -= ShaftUpgradeRequest;
        ElevatorUI.OnUpgradeRequest -= ElevatorUpgradeRequest;
        AtmUI.OnUpgradeRequest -= AtmUpgradeRequest;


    }
}