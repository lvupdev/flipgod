using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PsychokinesisController : SuperPowerController
{

    private GameObject redAura;
    private ShadowThresholdCustomEffect shadowEffect;
    private int kinesisNum = 1; //염력 모드

    void Start()
    {
        bottle = GameObject.Find("BottlePrefab");
        bottleController = bottle.GetComponent<BottleController>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        SPPController = GameObject.Find("SuperPowerPanel").GetComponent<SuperPowerPanelController>();
        blurEffect = GameObject.Find("Main Camera").GetComponent<RadialBlurImageEffect>();
        height = 2 * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        blurTime = 1;
        superPowerLV = 1;
        redAura = bottle.transform.Find("RedAura").gameObject;
        shadowEffect = GameObject.Find("Main Camera").GetComponent<ShadowThresholdCustomEffect>();
    }

    void FixedUpdate()
    {
        if (bottleController.isSuperPowerAvailabe && (playerImageController.playingChr == 0)) Activate();

        if ((!shadowEffect.enabled) && (blurEffect.samples > 1))
        {
            blurTime -= 20.0f * Time.fixedDeltaTime;
            blurEffect.samples = (int)blurTime;
        }
    }

    private void Activate()
    {
        //SuperPowePanelController 값 가져오기
        initPos = SPPController.GetInitPos();
        endPos = SPPController.GetEndPos();
        isTouch = SPPController.GetIsTouch();

        if (isTouch)
        {
            if (kinesisNum == 1)//염력 특수효과 발동
            {
                shadowEffect.enabled = true;
                blurEffect.enabled = true;
                Time.timeScale = 0.6f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                redAura.SetActive(true);//빨간 오러 켜기
                kinesisNum = 0;
            }

            if (initPos.x > Screen.width / 2.0f) //화면 터치 위치가 스크린 오른편이면 시계방향으로 회전 힘을 가한다.
            {
                bottleController.rb.AddTorque(-superPowerLV / 60.0f, ForceMode2D.Impulse); //가하는 힘은 초능력 강화 레벨을 60으로 나눈 수치
            }
            if (initPos.x <= Screen.width / 2.0f)//화면 터치 위치가 스크린 왼편이면 시계반대방향으로 회전 힘을 가한다.
            {
                bottleController.rb.AddTorque(superPowerLV / 60.0f, ForceMode2D.Impulse);
            }
        }

        if (shadowEffect.enabled) //염력 특수효과 발동
        {
            if (blurTime < 10)
            {
                blurTime += 20 * Time.fixedDeltaTime;
                blurEffect.samples = (int)blurTime;
            }
            blurEffect.blurCenterPos = new Vector2(0.5f + 0.5f * bottle.transform.position.x / (width / 2.0f), 0.5f + 0.5f * bottle.transform.position.y / (height / 2.0f));
        }
    }

    public void ReselectBottle()
    {
        bottle = GameObject.FindWithTag("isActBottle");
        bottleController = bottle.GetComponent<BottleController>();//힘을 적용할 물병을 태그에 따라 재설정
        redAura = bottle.transform.Find("RedAura").gameObject;

        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        kinesisNum = 1;
        shadowEffect.enabled = false;
    }
}