using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SuperPowerController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int[] superPowerLV; //초능력 강화 레벨
    public int[] skillLV; //필살기 강화 레벨
    public float presentStrength; //현재 물병에 가해진 힘

    private BottleController bottleController;
    private GameObject bottle;
    private PlayerController playerController;
    private Vector2 initPos;//화면을 눌렀을 때의 위치
    private Vector2 endPos;//화면에서 손을 땠을 떄의 위치
    private bool isTouch;
    private bool membraneAvailable; //탄성막을 생성해도 되는지의 여부
    private int membraneNum; //생성할 수 있는 탄성막의 개수
    private int freezeNum = 1; //빙결 능력을 사용할 수 있는 횟수
    private float freezeRad; //빙결 가능 범위 반지름


    void Start()
    {
        this.bottle = GameObject.Find("BottlePrefab");
        this.bottleController = bottle.GetComponent<BottleController>(); ;
        this.playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        isTouch = false;
        membraneAvailable = false;
        membraneNum = superPowerLV[1]; //탄성막 생성자의 초능력 강화 레벨의 수치만큼 탄성막을 생성할 수 있다.
        freezeRad = superPowerLV[2] * 3; //빙결자의 초능력 강화 레벨 수치의 두 배 만큼이 빙결 가능 범위의 반지름이 된다.
        initPos = Vector2.zero;
        endPos = Vector2.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(bottleController.isSuperPowerAvailabe)
        {
            switch (playerController.playingChr)//플레이어에 따라 실행되는 스킬이 달라진다.
            {
                case 0:
                    Psychokinesis();
                    break;
                case 1:
                    if((membraneNum > 0) && (membraneAvailable)) MembraneCreator(); //생성할 수 있는 탄성막의 개수가 0보다 크면
                    break;
                case 2:
                    Freezer();
                    break;
            }
        }
    }

    private void Psychokinesis()
    {
        if(isTouch)
        {
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
        if(isTouch && (freezeNum==1))
        {
            GameObject dynamicStructures = GameObject.Find("DynamicStructures");
            for(int i=0; i< dynamicStructures.transform.childCount; i++)
            {
                float distance = (dynamicStructures.transform.GetChild(i).position - bottle.transform.position).magnitude;
                if (distance<=freezeRad)
                {
                    dynamicStructures.transform.GetChild(i).GetComponent<DSController>().isFreezed = true;
                    Debug.Log("얼어라!!");
                }
            }
            freezeNum = 0;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        initPos = Input.mousePosition;
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        endPos = Input.mousePosition;
        isTouch = false;
        if((playerController.playingChr == 1) && (bottleController.isSuperPowerAvailabe))
        {
            membraneAvailable = true; //탄성막을 한 개 생성하기 위해서는 탄성막 생성자를 조작하는 도중 한 번의 터치를 해야한다.
        } 
    }

    public void ReselectBottle()
    {
        bottle = GameObject.FindWithTag("isActBottle");
        bottleController = bottle.GetComponent<BottleController>();//힘을 적용할 물병을 태그에 따라 재설정
        membraneNum = superPowerLV[1]; //생성할 수 있는 탄성막의 개수 초기화
        membraneAvailable = false;
        freezeNum = 1;
    }
}
