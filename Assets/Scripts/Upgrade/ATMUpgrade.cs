using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATMUpgrade : BaseUpgrade
{
    protected override void ExecuteUpgrade()
    {
        if (CurrentLevel % 10 == 0)
        {
            _atm.AddMiner();
        }

        foreach (AtmMiner miner in _atm.Miners) // for every miner inside the atm miner list...
        {
            miner.collectCapacity *= CollectCapacityMultiplier;
            miner.collectPerSecond *= CollectPerSecondMultiplier;


            if (CurrentLevel % 10 == 0)
            {
                miner.MoveSpeed *= MoveSpeedMultiplier;
            }
        }
    }
}
