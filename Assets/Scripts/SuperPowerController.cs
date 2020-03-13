using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SuperPowerController : MonoBehaviour
{
    public int[] superPowerLV; //초능력 강화 레벨
    public int[] skillLV; //필살기 강화 레벨
    public float presentStrength; //현재 물병에 가해진 힘

    private BottleController bottleController;
    private GameObject bottle;
    private GameObject redAura;
    private Color redAuraColor;
    private PlayerImageController playerImageController;
    private SuperPowerPanelController SPPController;
    private ShadowThresholdCustomEffect effect;
    private bool membraneAvailable; //탄성막을 생성해도 되는지의 여부
    private int membraneNum; //생성할 수 있는 탄성막의 개수
    private int kinesisNum = 1; //염력 모드
    private int freezeNum = 1; //빙결 능력을 사용할 수 있는 횟수
    private float freezeRad; //빙결 가능 범위 반지름
    private Vector2 initPos;//화면을 눌렀을 때의 위치
    private Vector2 endPos;//화면에서 손을 땠을 떄의 위치
    private bool isTouch;

    //값 수정 함수
    public void SetMembraneAvailable(bool x) { membraneAvailable = x; }


    void Start()
    {
        bottle = GameObject.Find("BottlePrefab");
        bottleController = bottle.GetComponent<BottleController>();
        redAura = bottle.transform.Find("RedAura").gameObject;
        redAuraColor = redAura.GetComponent<SpriteRenderer>().color;
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        SPPController = GameObject.Find("SuperPowerPanel").GetComponent<SuperPowerPanelController>();
        effect = GameObject.Find("Main Camera").GetComponent<ShadowThresholdCustomEffect>();


        membraneAvailable = false;
        membraneNum = superPowerLV[1]; //탄성막 생성자의 초능력 강화 레벨의 수치만큼 탄성막을 생성할 수 있다.
        freezeRad = superPowerLV[2] * 3; //빙결자의 초능력 강화 레벨 수치의 두 배 만큼이 빙결 가능 범위의 반지름이 된다.

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //SuperPowePanelController 값 가져오기
        initPos = SPPController.GetInitPos();
        endPos = SPPController.GetEndPos();
        isTouch = SPPController.GetIsTouch();

        if (bottleController.isSuperPowerAvailabe)
        {
            switch (playerImageController.playingChr)//플레이어에 따라 실행되는 스킬이 달라진다.
            {
                case 0:
                    Psychokinesis();
                    break;
                case 1:
                    if ((membraneNum > 0) && membraneAvailable) MembraneCreator(); //생성할 수 있는 탄성막의 개수가 0보다 크면
                    break;
                case 2:
                    Freezer();
                    break;
            }
        }
    }

    private void Psychokinesis()
    {
        if (isTouch)
        {
            if (kinesisNum == 1)//염력 특수효과 발동
            {
                effect.enabled = true;
                Time.timeScale = 0.6f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                kinesisNum = 0;
                redAura.SetActive(true);//빨간 오러 켜기
            }

            if (initPos.x > Screen.width / 2.0f) //화면 터치 위치가 스크린 오른편이면 시계방향으로 회전 힘을 가한다.
            {
                bottleController.rb.AddTorque(-superPowerLV[0] / 60.0f, ForceMode2D.Impulse); //가하는 힘은 초능력 강화 레벨을 60으로 나눈 수치
            }
            if (initPos.x <= Screen.width / 2.0f)//화면 터치 위치가 스크린 왼편이면 시계반대방향으로 회전 힘을 가한다.
            {
                bottleController.rb.AddTorque(superPowerLV[0] / 60.0f, ForceMode2D.Impulse);
            }
        }
    }

    private void MembraneCreator()
    {
        Vector2 direction = endPos - initPos; //화면 드래그 방향
        bottleController.rb.velocity = direction.normalized * presentStrength; // 물병을 던졌을 때의 힘만큼 속도를 가한다.
        membraneNum -= 1; //생성할 수 있는 탄성막의 개수 감소
        membraneAvailable = false; //다시 탄성막을 생성하려면 반드시 한 번 더 화면을 터치해야 함.

    }

    private void Freezer()
    {
        if (isTouch && (freezeNum == 1))
        {
            GameObject dynamicStructures = GameObject.Find("Dynamic Structure");
            for (int i = 0; i < dynamicStructures.transform.childCount; i++)
            {
                float distance = (dynamicStructures.transform.GetChild(i).position - bottle.transform.position).magnitude;
                if (distance <= freezeRad)
                {
                    dynamicStructures.transform.GetChild(i).GetComponent<DSController>().isFreezed = true;
                    Debug.Log("얼어라!!");
                }
            }
            freezeNum = 0;
        }
    }

    public void ReselectBottle()
    {
        bottle = GameObject.FindWithTag("isActBottle");
        bottleController = bottle.GetComponent<BottleController>();//힘을 적용할 물병을 태그에 따라 재설정
        redAura = bottle.transform.Find("RedAura").gameObject;
        membraneNum = superPowerLV[1]; //생성할 수 있는 탄성막의 개수 초기화
        membraneAvailable = false;
        freezeNum = 1;
        kinesisNum = 1;

        if (Time.timeScale == 0.6f) //화면 원상복귀
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            effect.enabled = false;
        }
    }
}
