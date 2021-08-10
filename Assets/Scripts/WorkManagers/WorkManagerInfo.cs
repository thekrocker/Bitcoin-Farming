using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ManagerType
{
    Junior,
    Senior,
    Executive
}

public enum BoostType
{
    MoveSpeed,
    Loading
}
[CreateAssetMenu]
public class WorkManagerInfo : ScriptableObject
{
[Header("Manager Info")]
    public ManagerType managerType;
    public Color levelColor;

    public BoostType boostType;
    public Sprite boostIcon;
    public float boostDuration;
    public string boostDescription;
    public float boostValue;



}
