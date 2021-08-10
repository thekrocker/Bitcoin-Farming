using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMiner : BaseMiner
{

    [SerializeField] private Elevator elevator;

    public Vector3 ElevatorDepositLocation =>
        new Vector3(transform.position.x, elevator.ElevatorDepositLocation.position.y);


    private Deposit _currentShaftDeposit;
    private int _currentShaftIndex = -1;

    
    

    public override void OnClick()
    {
        MoveToNextLocation();
    }

    private void MoveToNextLocation()
    {
        _currentShaftIndex++;
        Shaft currentShaft = ShaftManager.Instance.Shafts[_currentShaftIndex]; // Burada hangi shafta bulunduğunun bilgisini aldık.
        _currentShaftDeposit = currentShaft.ShaftDeposit;
        Vector3 shaftDepositPos = currentShaft.DepositLocation.position;
        Vector3 fixedPos = new Vector3(transform.position.x, shaftDepositPos.y); // Burada sadece Y bölgesinde hareket etmesini söyledik. 
        MoveMiner(fixedPos);
    }

    protected override void CollectBitCoin()
    {
        if (_currentShaftIndex == ShaftManager.Instance.Shafts.Count - 1 && !_currentShaftDeposit.CanCollectGold) // If we are in the last shaft and there is no gold to collect....
        {
            // return  elevator miner to deposit location
            ChangeGoal();
            MoveMiner(ElevatorDepositLocation);
            _currentShaftIndex = -1;
            return;
        }

        float amountToCollect = _currentShaftDeposit.CollectGold(this); // Depozitodaki bitcoini toplama methodu.. // CollectGold değil aslında CollectBitCoin
        float collectTime = amountToCollect / collectPerSecond;
        StartCoroutine(IECollect(amountToCollect, collectTime));

    }

    protected override IEnumerator IECollect(float bitcoin, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);

        if (currentBitCoin < collectCapacity && bitcoin <= (collectCapacity - currentBitCoin))
        {
            currentBitCoin += bitcoin;
            _currentShaftDeposit.RemoveCoin(bitcoin);

        }

        yield return new WaitForSeconds(0.5f);

        if (currentBitCoin < collectCapacity && _currentShaftIndex != ShaftManager.Instance.Shafts.Count - 1) // If the mine can still some gold, and he is not on last shaft.... 
        {
            MoveToNextLocation(); // Go Next shaft...
        }
        else // miner is either full of gold or last shaft to collect...
        {
            ChangeGoal();
            MoveMiner(ElevatorDepositLocation);
            _currentShaftIndex = -1;
        }
    }

    protected override void DepositBitCoin()
    {
        if (currentBitCoin <= 0)
        {
            ChangeGoal();
            MoveToNextLocation();
            return;
        }

        float depositTime = currentBitCoin / collectPerSecond;
        StartCoroutine(IEDeposit(depositTime));
    }

    protected override IEnumerator IEDeposit(float depositTime)
    {
        yield return new WaitForSeconds(depositTime);

        elevator.ElevatorDeposit.DepositBitCoin(currentBitCoin);
        currentBitCoin = 0;
        
        ChangeGoal();
        MoveToNextLocation();
    }
}
