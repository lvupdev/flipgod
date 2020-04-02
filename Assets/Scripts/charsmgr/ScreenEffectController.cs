using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffectController : MonoBehaviour
{
    private BottleSelectController bottleSelectController;
    private PlayerImageController playerImageController;
    private RadialBlurImageEffect blurEffect;
    private SuperPowerPanelController SPPController;

    public ShadowThresholdCustomEffect shadowEffect;
    public GameObject redAura;

    private float blurTime; //블러가 적용되는 시간
    private float height; //게임화면 높이
    private float width; //게임화면 넓이

    public int screenEffectNum; //화면 특수효과 계수

    void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        blurEffect = GameObject.Find("Main Camera").GetComponent<RadialBlurImageEffect>();
        shadowEffect = GameObject.Find("Main Camera").GetComponent<ShadowThresholdCustomEffect>();
        SPPController = GameObject.Find("SuperPowerPanel").GetComponent<SuperPowerPanelController>();
        height = 2 * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        blurTime = 1;
        screenEffectNum = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (playerImageController.playingChr)
        {
            case 0:
                if (shadowEffect.enabled)
                {
                    if (blurTime < 10)
                    {
                        blurTime += 20 * Time.fixedDeltaTime;
                        blurEffect.samples = (int)blurTime;
                    }
                    blurEffect.blurCenterPos = new Vector2(0.5f + 0.5f * bottleSelectController.bottle.transform.position.x / (width / 2.0f), 
                                                                            0.5f + 0.5f * bottleSelectController.bottle.transform.position.y / (height / 2.0f));
                }
                else if (blurTime > 1)
                {
                    blurTime -= 20.0f * Time.fixedDeltaTime;
                    if (blurTime > 1) blurEffect.samples = (int)blurTime;
                    else blurTime = 1;
                }
                break;
            case 1:
                switch (screenEffectNum)
                {
                    case 1:
                        break;
                    case 2:   // 화면 효과 1단계 = 화면 줌
                        if (blurTime > 20) screenEffectNum = 3;
                        blurTime += 120 * Time.fixedDeltaTime;
                        blurEffect.samples = (int)blurTime;
                        Camera.allCameras[0].orthographicSize -= Time.fixedDeltaTime;
                        Camera.allCameras[1].orthographicSize -= Time.fixedDeltaTime;
                        break;
                    case 3: // 화면효과 2단계 = 화면 정상화
                        blurTime -= 120 * Time.fixedDeltaTime;
                        if (blurTime < 1)
                        {
                            screenEffectNum = 1;
                            blurTime = 1;
                            blurEffect.blurSize = 10;
                            Camera.allCameras[0].orthographicSize = height / 2;
                            Camera.allCameras[1].orthographicSize = height / 2;
                        }
                        else
                        {
                            blurEffect.samples = (int)blurTime;
                            Camera.allCameras[0].orthographicSize += Time.fixedDeltaTime;
                            Camera.allCameras[1].orthographicSize += Time.fixedDeltaTime;
                        }
                        break;

                }
                break;
        }
    }

    public void KinesisEffect() //염력 특수효과 발동
    {
        if(screenEffectNum == 1)
        {
            shadowEffect.enabled = true;
            Time.timeScale = 0.6f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            bottleSelectController.bottle.transform.Find("RedAura").gameObject.SetActive(true);//빨간 오러 켜기
            screenEffectNum = 0;
        }
    }

    public void MembraneEffect() // 탄성막 특수효과 발동
    {
        blurEffect.blurSize = 20;
        blurEffect.blurCenterPos = new Vector2(0.5f + 0.5f * bottleSelectController.bottle.transform.position.x / (width / 2.0f), 
                                                            0.5f + 0.5f * bottleSelectController.bottle.transform.position.y / (height / 2.0f));
        double angle;
        if (SPPController.getDragDirection().x == 0)
        {
            angle = 0;
        }
        else
        {
            angle = Mathf.Atan2(SPPController.getDragDirection().x, SPPController.getDragDirection().y)*(180.0/Mathf.PI);
        }
        GameObject membrane = Instantiate(Resources.Load("Membrane"), bottleSelectController.bottle.gameObject.transform.position, Quaternion.Euler(0, 0, -(float)angle)) as GameObject; //탄성막 이미지 생성
        screenEffectNum = 2;
    }
}