using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShaftManager : Singleton<ShaftManager>
{



    [SerializeField] private Shaft shaftPrefab;
    [SerializeField] private float newShaftYPosition;
    [SerializeField] private float newShaftCost = 5000;
    [SerializeField] private float newShaftCostMultiplier = 10;
    
    [SerializeField] private List<Shaft> shafts;
    public List<Shaft> Shafts => shafts; // Bu propertyde, shafts değişkenini public yaptık.

    private int _currentShaftIndex;


    public float ShaftCost { get; set; }


    private void Start()
    {
        ShaftCost = newShaftCost;
    }

    void Update()
    {
        if (Input.GetKeyDown((KeyCode.N)))
        {
            AddShaft();
            
        }

    }


    public void AddShaft()
    {
        Transform lastShaft = shafts[_currentShaftIndex].transform;
        Vector3 newShaftPos = new Vector3(lastShaft.position.x, lastShaft.position.y - newShaftYPosition);
        
        Shaft newShaft = Instantiate(shaftPrefab, newShaftPos, quaternion.identity);
        _currentShaftIndex++; // başlangıçta 0 iken, her kodu çağırdığımızda sayı artacak, böylece Pozisyon olarak bir önceki Shaft'ın Y'sine göre pozisyon alacak.

        ShaftCost *= newShaftCostMultiplier;
        newShaft.ShaftUI.SetShaftUI(_currentShaftIndex);
        newShaft.ShaftUI.SetNewShaftCost(ShaftCost);

        shafts.Add(newShaft);
    }
}
