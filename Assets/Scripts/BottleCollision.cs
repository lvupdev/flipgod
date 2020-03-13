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
    private PlayerImageController playerImageController;
    private SuperPowerPanelController superPowerPanelController;
    private BottleController bottleController; //NEW: 오타 수정
    private GameObject redAura;


    void Start()
    {
        bottleGenerator = GameObject.Find("BottleGenerator").GetComponent<BottleGenerator>();
        superPowerController = GameObject.Find("Player").GetComponent<SuperPowerController>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        superPowerPanelController = GameObject.Find("SuperPowerPanel").GetComponent<SuperPowerPanelController>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        bottleController = GameObject.FindWithTag("isActBottle").GetComponent<BottleController>(); //NEW: 처음에 시작할 때 태그로 찾아줘야 함
        redAura = transform.Find("RedAura").gameObject;
    }

    //동전에 부딪혔을때. 동전은 isTrigger= True 상태여야함
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "coin")
        {
            CoinStatus coin = col.gameObject.GetComponent<CoinStatus>();
            ScoreManager.setScore(coin.value);
            Destroy(col.gameObject, 0f);
            StageGameMgr.instance.AddCoin();    // (0229 수정) 코인 텍스트 업데이트
        }
    }

    //어딘가에 부딪혔을때
    void OnCollisionEnter2D(Collision2D col)
    {
        bottleController.isSuperPowerAvailabe = false; //더 이상 초능력을 적용할 수 없음
        redAura.SetActive(false);

        if (gameObject.CompareTag("isActBottle"))
        {
            gameObject.tag = "unActBottle";//태그 변경
            bottleGenerator.GenerateBottle();//물병 생성
            padStrength.ReselectBottle(); //물병 재선택
            superPowerController.ReselectBottle(); //물병 재선택
            playerImageController.ReselectBottle(); //물병 재선택
            superPowerPanelController.ReselectBottle(); //물병 재선택
        }

        if (col.gameObject.CompareTag("floor")) bottleController.onFloor = true;
    }
}
