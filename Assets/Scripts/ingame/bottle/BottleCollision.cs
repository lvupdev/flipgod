using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
물병이 어딘가에 닿았을 때
*/
public class BottleCollision : MonoBehaviour
{
    public PadStrength padStrength;
    BottleSelectController bottleSelectController;
    private BottleGenerator bottleGenerator;
    private BottleController bottleController; //NEW: 오타 수정
    private UsefullOperation usefullOperation;
    private GameObject redAura;
    private GameObject freezeRange;
    private ScreenEffectController screenEffectController;



    void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        bottleGenerator = GameObject.Find("BottleManager").GetComponent<BottleGenerator>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        bottleController = GameObject.FindWithTag("isActBottle").GetComponent<BottleController>(); //NEW: 처음에 시작할 때 태그로 찾아줘야 함
        usefullOperation = GameObject.Find("GameResource").GetComponent<UsefullOperation>();
        redAura = transform.Find("RedAura").gameObject;
        freezeRange = transform.Find("FreezeRange").gameObject;
        screenEffectController = GameObject.Find("Main Camera").GetComponent<ScreenEffectController>();
    }

    //동전에 부딪혔을때. 동전은 isTrigger= True 상태여야함
    void OnTriggerEnter2D(Collider2D col)
    {
        // if 
        if (col.gameObject.tag == "coin")
        {
            Destroy(col.gameObject, 0f);
        }
    }

    //어딘가에 부딪혔을때
    void OnCollisionEnter2D(Collision2D col)
    {
        if(!(col.gameObject.CompareTag("Membrane")))
        {
            bottleController.isSuperPowerAvailabe = false; //더 이상 초능력을 적용할 수 없음
            if (gameObject.CompareTag("isActBottle"))
            {
                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                    Time.fixedDeltaTime = 0.02f * Time.timeScale;
                    screenEffectController.shadowEffect.enabled = false;
                    screenEffectController.screenEffectNum = 1;
                }

                gameObject.tag = "unActBottle";//태그 변경
                usefullOperation.FadeOut(false, redAura.GetComponent<SpriteRenderer>());
                freezeRange.SetActive(false);

                bottleSelectController.bottleSelected = false;
                bottleGenerator.GenerateBottleWithDelay(1);//딜레이를 주고 물병 생성
                bottleSelectController.ReselectBottleWithDelay(1); //딜레이를 주고 물병 재선택
            }
        }

        if (col.gameObject.CompareTag("floor")) bottleController.onFloor = true;
    }
}
