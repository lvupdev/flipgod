using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffectController : MonoBehaviour
{
    private BottleSelectController bottleSelectController;
    private PlayerImageController playerImageController;
    private RadialBlurImageEffect blurEffect;
    private SuperPowerPanelController Panel_SuperPower;
    private UsefullOperation usefullOperation;
    private Camera mainCamera;
    private GameObject colorCamera;
    private GameObject redAura;

    public ShadowThresholdCustomEffect shadowEffect;
    public GameObject membranePrefab;

    private float blurTime; //블러가 적용되는 시간

    public float height { get; set; } //게임화면 높이
    public float width { get; set; } //게임화면 넓이

    public int screenEffectNum { get; set; } //화면 특수효과 계수
    public float psychoTime { get; set; } //염력을 사용할 수 있는 시간

    void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        mainCamera = Camera.main;
        colorCamera = GameObject.Find("Color Camera").gameObject;
        blurEffect = GameObject.Find("Main Camera").GetComponent<RadialBlurImageEffect>();
        shadowEffect = GameObject.Find("Main Camera").GetComponent<ShadowThresholdCustomEffect>();
        Panel_SuperPower = GameObject.Find("Panel_SuperPower").GetComponent<SuperPowerPanelController>();
        usefullOperation = GameObject.Find("GameResource").GetComponent<UsefullOperation>();
        height = 2 * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        blurTime = 1;
        screenEffectNum = 1;
        psychoTime = 0.4f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerImageController.getPlayingChr())
        {
            case 0:
                if (shadowEffect.enabled)
                {
                    if (blurTime < 10)
                    {
                        blurTime += 20 * Time.deltaTime;
                        blurEffect.samples = (int)blurTime;
                    }
                    blurEffect.blurCenterPos = new Vector2(0.5f + 0.5f * bottleSelectController.bottle.transform.position.x / (width / 2.0f), 
                                                                            0.5f + 0.5f * bottleSelectController.bottle.transform.position.y / (height / 2.0f));
                }
                else if (blurTime > 1)
                {
                    blurTime -= 20.0f * Time.deltaTime;
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
                        if (blurTime > 10) screenEffectNum = 3;
                        blurTime += 50 * Time.deltaTime;
                        blurEffect.samples = (int)blurTime;
                        Camera.allCameras[0].orthographicSize -= 2 * Time.deltaTime;
                        Camera.allCameras[1].orthographicSize -= 2 * Time.deltaTime;
                        break;
                    case 3: // 화면효과 2단계 = 화면 정상화
                        blurTime -= 50 * Time.deltaTime;
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
                            Camera.allCameras[0].orthographicSize += 2 * Time.deltaTime;
                            Camera.allCameras[1].orthographicSize += 2 * Time.deltaTime;
                        }
                        break;

                }
                break;
            case 2:
                break;
        }

        if(screenEffectNum == 0) //염력 사용 중일 때만 실행
		{
            psychoTime -= Time.deltaTime;
            if(psychoTime < 0) //지정된 염력 사용 시간이 지나면 염력 사용 종료
			{
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                shadowEffect.enabled = false;
                screenEffectNum = 1;
                psychoTime = 0.4f;
                usefullOperation.FadeOut(false, redAura.GetComponent<SpriteRenderer>());
            }

        }
    }

    public void KinesisEffect() //염력 카메라 특수효과 발동
    {
        if(screenEffectNum == 1)
        {
            shadowEffect.enabled = true;
            Time.timeScale = 0.6f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            redAura = bottleSelectController.bottle.transform.Find("RedAura").gameObject;
            usefullOperation.FadeIn(redAura.GetComponent<SpriteRenderer>());//빨간 오러 켜기
            screenEffectNum = 0;
        }
    }

    public void MembraneEffect() // 탄성막 카메라 특수효과 발동
    {
        blurEffect.blurSize = 20;
        blurEffect.blurCenterPos = new Vector2(0.5f + 0.5f * bottleSelectController.bottle.transform.position.x / (width / 2.0f), 
                                                            0.5f + 0.5f * bottleSelectController.bottle.transform.position.y / (height / 2.0f));
        double angle;
        if (Panel_SuperPower.getDragDirection().x == 0)
        {
            angle = 0;
        }
        else
        {
            angle = Mathf.Atan2(Panel_SuperPower.getDragDirection().x, Panel_SuperPower.getDragDirection().y)*(180.0/Mathf.PI);
        }

        GameObject membrane = Instantiate(membranePrefab, bottleSelectController.bottle.gameObject.transform.position, Quaternion.Euler(0, 0, -(float)angle)); //탄성막 이미지 생성
        screenEffectNum = 2;
    }

    public void FreezeEffect() //빙결 카메라 특수효과 발동
    {
        usefullOperation.ShakeObject(mainCamera.transform, 0.2f, 0.25f);
        usefullOperation.ShakeObject(colorCamera.transform, 0.2f, 0.25f);
    }
}