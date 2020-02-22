using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SuperPowerController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int[] superPowerLV; //초능력 강화 레벨
    public int[] skillLV; //필살기 강화 레벨

    private BottleController bottleController;
    private PlayerController playerController;
    private Vector2 initPos;//화면을 눌렀을 때의 위치
    private Vector2 endPos;//화면에서 손을 땠을 떄의 위치
    private bool isTouch;
    private bool membraneAvailable; //탄성막을 생성해도 되는지의 여부
    private int membraneNum; //생성할 수 있는 탄성막의 개수


    void Start()
    {
        this.bottleController = GameObject.Find("BottlePrefab").GetComponent<BottleController>(); ;
        this.playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        isTouch = false;
        membraneAvailable = false;
        membraneNum = superPowerLV[1]; //탄성막 생성자의 초능력 강화 레벨의 수치만큼 탄성막을 생성할 수 있다.
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
            if (initPos.x > Screen.width / 2.0f) bottleController.rb.AddTorque(-superPowerLV[0]/60.0f,ForceMode2D.Impulse);
            if (initPos.x <= Screen.width / 2.0f) bottleController.rb.AddTorque(superPowerLV[0]/60.0f, ForceMode2D.Impulse);
        }
    }

    private void MembraneCreator()
    {
        Vector2 direction = endPos - initPos; //화면 드래그 방향
        bottleController.rb.velocity = direction.normalized * 8;
        membraneNum -= 1; //생성할 수 있는 탄성막의 개수 감소
        membraneAvailable = false; //다시 탄성막을 생성하려면 반드시 한 번 더 화면을 터치해야 함.

    }

    private void Freezer()
    {

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
        bottleController = GameObject.FindWithTag("isActive").GetComponent<BottleController>();//힘을 적용할 물병을 태그에 따라 재설정
        membraneNum = superPowerLV[1]; //생성할 수 있는 탄성막의 개수 초기화
        membraneAvailable = false;
    }
}
