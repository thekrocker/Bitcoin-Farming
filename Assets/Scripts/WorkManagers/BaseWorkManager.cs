using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public interface IMineLocation
{
    void ApplyManagerBoost();

}

public class BaseWorkManager : MonoBehaviour
{

    [SerializeField] private GameObject boostButton;
    [SerializeField] private Image boostIcon;
    public IMineLocation CurrentMineLocation { get; set; }
    public WorkManagerInfo ManagerAssigned { get; set; }
    
    
    
    public static Action<IMineLocation> OnManagerClicked;


    private void Awake()
    {
        HideBoostButton();

    }

    private void Start()
    {
        Debug.Log(ManagerAssigned);
    }

    public void RunBoost()
    {
        CurrentMineLocation?.ApplyManagerBoost();
        
    }

    private void OnMouseDown()
    {
        OnManagerClicked?.Invoke(CurrentMineLocation);
    }

    private void HideBoostButton()
    {
        boostButton.SetActive(false);
    }

    
    public void SetupBoostButton()
    {
        if (ManagerAssigned != null)
        {
            boostButton.SetActive(true);
            boostIcon.sprite = ManagerAssigned.boostIcon;
        }
        

    }
}
