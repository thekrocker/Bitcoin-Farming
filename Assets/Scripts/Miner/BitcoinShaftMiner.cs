using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitcoinShaftMiner : BaseMiner
{

    public Shaft currentShaft { get; set; } // Shaftta bulunan Deposit Location vs. gibi referansları çekeceğiz. Bunun için oluşturduk.

    public Vector3 DepositLocation => new Vector3(currentShaft.DepositLocation.position.x, transform.position.y); // Burada Shaft'ta bulunan Deposit lokasyonunu çekip DepositLocation değişkenine atadık.
    public Vector3 MiningLocation => new Vector3(currentShaft.MiningLocation.position.x, transform.position.y); // Burada Shaft'ta bulunan Mining lokasyonunu çekip MiningLocation değişkenine atadık.


    private int walkAnimation = Animator.StringToHash("Walk");
    private int miningAnimation = Animator.StringToHash("Mining");


    protected override void MoveMiner(Vector3 newPosition)
    {
        base.MoveMiner(newPosition);
        _anim.SetTrigger(walkAnimation);
    }

    public override void OnClick()
    {
        MoveMiner(MiningLocation);
    }

    protected override void CollectBitCoin()
    {

        _anim.SetTrigger(miningAnimation);
        float collectTime = collectCapacity / collectPerSecond;
        // Burada collectTime şu sonuca eşit. Örneğin: CollectCapacity, 200. Yani maksimum 200 gold alıyor. Bunu Saniyede toplanılan bitcoine bölüyoruz. Örneğin 50.
        // 200/50 = 4 saniye yapar. CollectTime burada 4 saniyeye tekabül eder.

        StartCoroutine(IECollect(collectCapacity, collectTime));


    }

    protected override IEnumerator IECollect(float bitcoin, float collectTime)
    {

        yield return new WaitForSeconds(collectTime); // Yani bu yukarıda oluşturduğumuz collectTime a tekabül eder. 


        currentBitCoin = bitcoin;


        ChangeGoal();  // isTimetoCollect = true iken false a döndü. 
        Debug.Log(isTimeToCollect);
        RotateMiner(-1);

        MoveMiner(DepositLocation);

    }

    protected override void DepositBitCoin()
    {

        currentShaft.ShaftDeposit.DepositBitCoin(currentBitCoin); //  Buradaki Deposit Bitcoin, Solda bulunan alete bitcoin atmamızı sağlar. İçinde bulunduğumuz method değil. 

        currentBitCoin = 0;
        
        ChangeGoal();
        Debug.Log(isTimeToCollect);
        RotateMiner(1);

        MoveMiner(MiningLocation);

        //Burada isTimetoCollect'i true a çevirmemiz gerekiyor. Bu yüzden tekrar çağırarak bunu hallettik. 
    }



}
