using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
물병이 어딘가에 닿았을 때
*/
public class BottleCollision : MonoBehaviour
{
    public PadStrength padStrength;
    private BottleGenerator bottleGenerator;
    private SuperPowerController superPowerController;
    private PlayerController playerController;

    private BottleController bottleContrlloer;


    void Start()
    {
        bottleGenerator = GameObject.Find("BottleGenerator").GetComponent<BottleGenerator>();
        superPowerController = GameObject.Find("SuperPower").GetComponent<SuperPowerController>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        bottleContrlloer = GameObject.Find("BottlePrefab").GetComponent<BottleController>();

    }

    //동전에 부딪혔을때. 동전은 isTrigger= True 상태여야함
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "coin")
        {
            CoinStatus coin = col.gameObject.GetComponent<CoinStatus>();
            ScoreManager.setScore(coin.value);
            Destroy(col.gameObject, 0f);
        }
    }

    //어딘가에 부딪혔을때
    void OnCollisionEnter2D(Collision2D col)
    {
        bottleContrlloer.isSuperPowerAvailabe = false; //더 이상 초능력을 적용할 수 없음
        if (gameObject.CompareTag("isActive"))
        {

            gameObject.tag = "Untagged";//태그가 사라짐
            bottleGenerator.GenerateBottle();//물병 생성
            padStrength.ReselectBottle(); //물병 재선택
            superPowerController.ReselectBottle(); //물병 재선택
            playerController.ReselectBottle(); //물병 재선택
        }
    }
}
