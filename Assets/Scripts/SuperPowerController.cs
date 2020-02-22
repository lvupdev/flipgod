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


    void Start()
    {
        this.bottleController = GameObject.Find("BottlePrefab").GetComponent<BottleController>(); ;
        this.playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        isTouch = false;
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
                    Membranecreator();
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
            if (initPos.x > Screen.width / 2.0f) bottleController.rb.AddTorque(-superPowerLV[0]/20.0f,ForceMode2D.Impulse);
            if (initPos.x <= Screen.width / 2.0f) bottleController.rb.AddTorque(superPowerLV[0]/20.0f, ForceMode2D.Impulse);
        }
    }

    private void Membranecreator()
    {

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
    }

    public void ReselectBottle()
    {
        bottleController = GameObject.FindWithTag("isActive").GetComponent<BottleController>();//힘을 적용할 물병을 태그에 따라 재설정
    }
}
