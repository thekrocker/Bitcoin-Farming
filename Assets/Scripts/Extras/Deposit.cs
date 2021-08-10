using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deposit : MonoBehaviour
{


    public float CurrentBitCoin { get; set; }

    public bool CanCollectGold => CurrentBitCoin > 0;


    public void RemoveCoin(float amount) // Depozitten silinecek. 
    {
        if (amount <= CurrentBitCoin)
        {
            CurrentBitCoin -= amount;
        }
        
    }

    public void DepositBitCoin(float amount) // Buradaki Deposit Bitcoin, Solda bulunan alete bitcoin atmamızı sağlar. 
    {
        CurrentBitCoin += amount;

    }

    public float CollectGold(BaseMiner miner)
    {
        float minerCapacity = miner.collectCapacity - miner.currentBitCoin;
        return EvaluateAmountToCollect(minerCapacity);
    }

    
    // 200 Miner'in Kapasite var diyelim. Depozitte de 600 var diyelim.. minerCollectCapacity return alır yani miner'in kaç kapasitesi varsa onu alır bu bilgi. 
    public float EvaluateAmountToCollect(float minerCollectCapacity)
    {
        if (minerCollectCapacity <= CurrentBitCoin) // If miner can only collect his own capacity. 
        {
            return minerCollectCapacity;

        }

        return CurrentBitCoin; // Eğer kapasitesi, depozitodan büyükse, direkt oradaki sayıyı alır üstüne.
    }
}
