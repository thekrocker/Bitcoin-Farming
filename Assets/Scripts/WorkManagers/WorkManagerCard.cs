using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkManagerCard : MonoBehaviour
{

    public static Action<WorkManagerCard> OnAssignRequest;
    
    [SerializeField] private Image boostIcon;
    [SerializeField] private TextMeshProUGUI managerType;
    [SerializeField] private TextMeshProUGUI boostDuration;
    [SerializeField] private TextMeshProUGUI boostDescription;



    public WorkManagerInfo ManagerInfoAssigned { get; set; }
    public void SetupWorkManagerCard(WorkManagerInfo managerInfo)
    {
        ManagerInfoAssigned = managerInfo;
        boostIcon.sprite = managerInfo.boostIcon;
        managerType.text = managerInfo.managerType.ToString();
        managerType.color = managerInfo.levelColor;
        boostDuration.text = $"Duration: {managerInfo.boostDuration}";
        boostDescription.text = managerInfo.boostDescription;
        


    }

    public void AssignManager()
    {
        OnAssignRequest?.Invoke(this);
        gameObject.SetActive(false);
    }
    
}
