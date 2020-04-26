using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 스테이지 레벨1
 */

public class StageManager1 : MonoBehaviour
{
    //=====================================================================   
    // 저장된 데이터 (미션, 플레이어 자원) 같은 것을 관리하는 스크립트 필요
    // 해당 스크립트에서 받아오는 것으로
    //=====================================================================

    // Signleton of StageManager1
    // public static StageManager1 singleton;

    // Variable about mission
    public static int limitedBottleNum;
    public static float limitedTimeSec;

    public static int remainBottleNum;


    // Variable about goods to be gained
    public static int gainedCoinNum;

    // Dynamic Structure
    Car car;

    void Start()
    {
        // singleon = this;

        limitedBottleNum = 3;
        limitedTimeSec = 30.0f;

        remainBottleNum = limitedBottleNum;
        gainedCoinNum = 0;

        car = GameObject.Find("Car").GetComponent<Car>();
        car.setValue(new Vector2(-8.0f, -2.26f), new Vector2(-4.5f, -2.26f), 2f);
        //car.MoveDynamicStructure();


        Time.timeScale = 0.0f;
    }

    private void Update()
    {
        Timer();
    }

    // 이런 함수는 GameManager 같은 것을 따로 두어야 할 것 같다.
    public static void InitializeStage()
    {
        // To do something
    }

    public static void Timer()
    {
        if (limitedTimeSec > 0)
        {
            limitedTimeSec -= Time.deltaTime;
        }
    }

    public static void AddCoin()
    {
        gainedCoinNum += 1;
    }

    public void CountUsedBottle()
    {
        if (remainBottleNum > 0)
        {
            remainBottleNum -= 1;
        }
    }
}
