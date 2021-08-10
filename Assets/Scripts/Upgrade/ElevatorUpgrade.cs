using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorUpgrade : BaseUpgrade
{
    
    
    protected override void ExecuteUpgrade()
    {
        _elevator.Miner.collectCapacity *= CollectCapacityMultiplier;
        _elevator.Miner.collectPerSecond *= CollectPerSecondMultiplier;

        if (CurrentLevel + 1 % 10 == 0)
        {
            _elevator.Miner.MoveSpeed *= MoveSpeedMultiplier;
        }
    }
}
