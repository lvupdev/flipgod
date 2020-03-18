using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PsychokinesisController : MonoBehaviour
{
    private int superPowerLV; //초능력 강화 레벨
    private int skillLV; //필살기 강화 레벨
    private BottleController bottleController;
    private GameObject bottle;
    private PlayerImageController playerImageController;
    private SuperPowerPanelController SPPController;
    private RadialBlurImageEffect blurEffect;
    private BottleSelectController bottleSelectController;
    private float blurTime; //블러가 적용되는 시간
    private float height; //게임화면 높이
    private float width; //게임화면 넓이
    private Vector2 initPos;//화면을 눌렀을 때의 위치
    private Vector2 endPos;//화면에서 손을 땠을 떄의 위치
    private bool isTouch;
    private bool isScreenEffect; //화면 특수 효과가 적용되는지의 여부

    public GameObject redAura;
    public ShadowThresholdCustomEffect shadowEffect;
    public int kinesisNum = 1; //염력 모드

    void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        bottle = bottleSelectController.bottle;
        bottleController = bottle.GetComponent<BottleController>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        SPPController = GameObject.Find("SuperPowerPanel").GetComponent<SuperPowerPanelController>();
        blurEffect = GameObject.Find("Main Camera").GetComponent<RadialBlurImageEffect>();
        height = 2 * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        isScreenEffect = false;
        blurTime = 1;
        superPowerLV = 1;
        redAura = bottle.transform.Find("RedAura").gameObject;
        shadowEffect = GameObject.Find("Main Camera").GetComponent<ShadowThresholdCustomEffect>();
    }

    void FixedUpdate()
    {
        if (bottleSelectController.reload)
        {
            bottle = bottleSelectController.bottle;
            bottleController = bottle.GetComponent<BottleController>();
            redAura = bottle.transform.Find("RedAura").gameObject;
        }

        if (playerImageController.playingChr == 0)
        {
            if(bottleController.isSuperPowerAvailabe) Activate();

            /*
            if((!shadowEffect.enabled) && (blurTime > 1))
            {
                blurTime -= 20.0f * Time.fixedDeltaTime;
                if (blurTime > 1) blurEffect.samples = (int)blurTime;
                else blurTime = 1;
            }

            if (isScreenEffect) ScreenEffectOn();
            */
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
                Time.timeScale = 0.6f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                redAura.SetActive(true);//빨간 오러 켜기
                Debug.Log(blurTime);
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
}