using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShaftUpgrade : BaseUpgrade
{
    
    
    protected override void ExecuteUpgrade()
    {
        if (CurrentLevel % 10 == 0)  // Eğer 10 veya 10'un katı bir seviyeye geldiyse, Miner oluştur. 
        {
            _shaft.CreateMiner();
            
        }

        foreach (BitcoinShaftMiner miner in _shaft.Miners)
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
