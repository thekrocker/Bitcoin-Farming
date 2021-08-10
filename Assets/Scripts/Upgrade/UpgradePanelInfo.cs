using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Locations
{
    Shaft,
    Elevator,
    Warehouse
    
    
}


[CreateAssetMenu(menuName = "UpgradeInfo")]
public class UpgradePanelInfo : ScriptableObject
{


    public string panelTitle;
    public Sprite panelMinerIcon;
    public Locations location;
    [Header("Stat Title")] 
    public string stat1Title;
    public string stat2Title;
    public string stat3Title;
    public string stat4Title;

    [Header("Stat Icon")] public Sprite stat1Icon;
    public Sprite stat2Icon;
    public Sprite stat3Icon;
    public Sprite stat4Icon;
}