using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShaftUI : MonoBehaviour
{

    public static Action<Shaft, ShaftUpgrade> OnUpgradeRequest;

    [SerializeField] private TextMeshProUGUI depositGold;
    [SerializeField] private TextMeshProUGUI shaftID;
    [SerializeField] private TextMeshProUGUI shaftLevel;
    [SerializeField] private TextMeshProUGUI newShaftCost;
    [SerializeField] private GameObject newShaftButton;


    
    private Shaft _shaft;
    private ShaftUpgrade _shaftUpgrade;
    
    // Start is called before the first frame update
    void Awake()
    {
        _shaft = GetComponent<Shaft>();
        _shaftUpgrade = GetComponent<ShaftUpgrade>();
    }

    // Update is called once per frame
    void Update()
    {
        depositGold.text = _shaft.ShaftDeposit.CurrentBitCoin.ToCurrency();
    }

    public void AddShaft()
    {
        if (BCManager.Instance.CurrentBitcoin >= ShaftManager.Instance.ShaftCost)  // If we have enough money to buy...
        {
            BCManager.Instance.RemoveBC(ShaftManager.Instance.ShaftCost); // Kaç paraysa Shaft o parayı sil. 
            ShaftManager.Instance.AddShaft();
            newShaftButton.SetActive(false);
        }

        
    }

    public void OpenUpgradeContainer()
    {
        OnUpgradeRequest?.Invoke(_shaft, _shaftUpgrade); // if not null.. invoke it it. 
        
    }

    public void SetShaftUI(int ID)
    {
        _shaft.ShaftID = ID;
        shaftID.text = (ID + 1).ToString();

    }

    public void SetNewShaftCost(float newCost)
    {
        newShaftCost.text = newCost.ToString();

    }


    private void UpgradeCompleted(BaseUpgrade upgrade)
    {
        if (_shaftUpgrade == upgrade)
        {
            shaftLevel.text = upgrade.CurrentLevel.ToString();

        }
        
    }
    private void OnEnable()
    {
        ShaftUpgrade.OnUpgradeCompleted += UpgradeCompleted;
    }

    private void OnDisable()
    {
        ShaftUpgrade.OnUpgradeCompleted -= UpgradeCompleted;

    }
}
