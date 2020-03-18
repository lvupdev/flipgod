using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MembraneCreatorController : MonoBehaviour
{
    private int superPowerLV; //초능력 강화 레벨
    private int skillLV; //필살기 강화 레벨
    private BottleSelectController bottleSelectController;
    private BottleController bottleController;
    private GameObject bottle;
    private PlayerImageController playerImageController;
    private SuperPowerPanelController SPPController;
    private RadialBlurImageEffect blurEffect;
    private float blurTime; //블러가 적용되는 시간
    private float height; //게임화면 높이
    private float width; //게임화면 넓이
    private Vector2 initPos;//화면을 눌렀을 때의 위치
    private Vector2 endPos;//화면에서 손을 땠을 떄의 위치
    private bool isTouch;
    private bool isScreenEffect; //화면 특수 효과가 적용되는지의 여부

    public bool membraneAvailable; //탄성막을 생성해도 되는지의 여부
    public int membraneNum; //생성할 수 있는 탄성막의 개수
    public float presentStrength { get; set; } //현재 물병에 가해진 힘

    //값 전달 함수
    public int getSuperPowerLV() { return superPowerLV; }

    //값 수정 함수
    public void SetMembraneAvailable(bool x) { membraneAvailable = x; }

    // Start is called before the first frame update
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
        blurTime = 1;
        superPowerLV = 1;
        membraneAvailable = false;
        membraneNum = superPowerLV; //탄성막 생성자의 초능력 강화 레벨의 수치만큼 탄성막을 생성할 수 있다.
        isScreenEffect = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bottleSelectController.reload)
        {
            bottle = bottleSelectController.bottle;
            bottleController = bottle.GetComponent<BottleController>();
        }

        if (bottleController.isSuperPowerAvailabe && (playerImageController.playingChr == 1) 
            && (membraneNum > 0) && membraneAvailable) Activate();
        /*

        switch (screenEffect)
        {
            case 0:
                break;
            case 1:   // 화면 효과 1단계 = 화면 줌
                if (blurTime > 20) screenEffect = 2;
                blurTime += 120 * Time.fixedDeltaTime;
                blurEffect.samples = (int)blurTime;
                Camera.allCameras[0].orthographicSize -= 3 * Time.fixedDeltaTime;
                Camera.allCameras[1].orthographicSize -= 3 * Time.fixedDeltaTime;
                break;
            case 2: // 화면효과 2단계 = 화면 정상화
                blurTime -= 120 * Time.fixedDeltaTime;
                if (blurTime < 1)
                {
                    screenEffect = 0;
                    blurTime = 1;
                    blurEffect.blurSize = 10;
                    Camera.allCameras[0].orthographicSize = height / 2;
                    Camera.allCameras[1].orthographicSize = height / 2;
                }
                else
                {
                    blurEffect.samples = (int)blurTime;
                    Camera.allCameras[0].orthographicSize += 3 * Time.fixedDeltaTime;
                    Camera.allCameras[1].orthographicSize += 3 * Time.fixedDeltaTime;
                }
                break;
            
        }
        */
    }

    private void Activate()
    {
        //SuperPowePanelController 값 가져오기
        initPos = SPPController.GetInitPos();
        endPos = SPPController.GetEndPos();

        Vector2 direction = endPos - initPos; //화면 드래그 방향
        bottleController.rb.velocity = direction.normalized * presentStrength; // 물병을 던졌을 때의 힘만큼 속도를 가한다.
        membraneNum -= 1; //생성할 수 있는 탄성막의 개수 감소
        membraneAvailable = false; //다시 탄성막을 생성하려면 반드시 한 번 더 화면을 터치해야 함.
        //screenEffect = 1;
        blurEffect.blurSize = 20;
        blurEffect.blurCenterPos = new Vector2(0.5f + 0.5f * bottle.transform.position.x / (width / 2.0f), 0.5f + 0.5f * bottle.transform.position.y / (height / 2.0f));
    }
}


