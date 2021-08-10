using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseMiner : MonoBehaviour, IClickable
{


    



    [Header("Başlangıç Değerleri")]
    [SerializeField] private float initialCollectCapacity;
    [SerializeField] private float initialCollectPerSecond;
    [SerializeField] protected float moveSpeed;
    
    
    public bool MinerClicked { get; set; }

    public float MoveSpeed { get; set; }

    public float currentBitCoin { get; set; }
    public float collectCapacity { get; set; }
    public float collectPerSecond { get; set; }
    public bool isTimeToCollect { get; set; }
    


    protected Animator _anim;


    private void Awake()
    {
        _anim = GetComponent<Animator>();
        isTimeToCollect = true;
        MoveSpeed = moveSpeed;
        collectCapacity = initialCollectCapacity;
        collectPerSecond = initialCollectPerSecond;
    }

    protected virtual void MoveMiner(Vector3 newPosition)
    {
        transform.DOMove(newPosition, MoveSpeed).SetEase(Ease.Linear).OnComplete((() => // Bu animasyon bittiğinde ne yapacak...
        {

            if(isTimeToCollect)
            {
                // Collect the Bitcoin
                CollectBitCoin();

            } else
            { // Topladıktan sonra bırakmaya dön. 
                DepositBitCoin();
            }


        })).Play();

    }

    public virtual void OnClick() //Iclickable dan aldığımız method.
    {
        //ShaftMinerda kullanacağız.
        
    }

    private void OnMouseDown()
    {
        if (!MinerClicked) // Eğer mineClicked false ise OnClick methodundaki Move methodunu çalıştır.
                           // Çalıştırınca minerclicked'i true yap. böylece sadece tek sefer tıklanır. çünkü artık !minerclicked koşulu çalışmayacak.
        {
            OnClick();
            MinerClicked = true;

        }
    }


    protected virtual void CollectBitCoin()
    {


    }

    protected virtual IEnumerator IECollect(float bitcoin, float collectTime)
    {
        yield return null;

    }

    protected virtual IEnumerator IEDeposit(float depositTime)
    {
        yield return null;

        
    }

    protected virtual void DepositBitCoin()
    {


    }

    
        
    protected void ChangeGoal()  // Bu kod, IsTimeToCollect'i true ve false 'a çevirir.
                                 // Böylece depositLocation'ûna çağırdığımızda isTimetoCollectGold(yani gold toplama) fonksiyonunu truea'a çevirmemizi sağlar.
    {
        isTimeToCollect = !isTimeToCollect;

        // if its true, make it false. if its false make it true. 

    }


    protected void RotateMiner(int direction)
    {
        if (direction == 1)
        {
            transform.localScale = new Vector3(x: 1, y: 1, z: 1);
        
        } else
        {
            transform.localScale = new Vector3(x: -1, y: 1, z: 1);
        }

    }

 
}
