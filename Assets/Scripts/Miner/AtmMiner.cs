using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AtmMiner : BaseMiner
{
    public Deposit ElevatorDeposit { get; set; }
    public Vector3 ElevatorDepositLocation { get; set; }
    public Vector3 AtmLocation { get; set; }


    private int _driveAnimation = Animator.StringToHash("isDriving");

    protected override void MoveMiner(Vector3 newPosition)
    {
        base.MoveMiner(newPosition); 
        _anim.SetBool(_driveAnimation, true);
    }

    public override void OnClick()
    {
        RotateMiner(-1);
        MoveMiner(ElevatorDepositLocation);
    }

    protected override void CollectBitCoin()
    {
        if (!ElevatorDeposit.CanCollectGold)
        {
            RotateMiner(1);
            ChangeGoal();
            MoveMiner(AtmLocation);
            return;
        }
        _anim.SetBool(_driveAnimation, false);
        float amountToCollect = ElevatorDeposit.CollectGold(this);
        float collectTime = collectCapacity / collectPerSecond;
        StartCoroutine(IECollect(amountToCollect, collectTime));
    }

    protected override IEnumerator IECollect(float bitcoin, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);

        currentBitCoin = bitcoin;
        ElevatorDeposit.RemoveCoin(bitcoin);
        _anim.SetBool(_driveAnimation, true);
        
        RotateMiner(1);
        ChangeGoal();
        MoveMiner(AtmLocation);

    }

    protected override void DepositBitCoin()
    {
        if (currentBitCoin <= 0)
        {
            RotateMiner(-1);
            ChangeGoal();
            MoveMiner(ElevatorDepositLocation);
            return;
        }
        
        _anim.SetBool(_driveAnimation, false);
        float depositTime = currentBitCoin / collectPerSecond;
        StartCoroutine(IEDeposit(depositTime));
    }

    protected override IEnumerator IEDeposit(float depositTime) // ATM'ye depozit etme. 
    {
        yield return new WaitForSeconds(depositTime);
        
        BCManager.Instance.AddBC(currentBitCoin);
        currentBitCoin = 0;
        
        RotateMiner(-1);
        ChangeGoal();
        MoveMiner(ElevatorDepositLocation);
    }
}
