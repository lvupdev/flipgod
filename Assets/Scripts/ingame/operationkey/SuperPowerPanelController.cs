using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SuperPowerPanelController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private MembraneCreator membraneCreator;
    private BottleSelectController bottleSelectController;
    private PlayerImageController playerImageController;
    private Vector2 dragDirection; //드래그 방향 백터
    private Vector2 initPos;//화면을 눌렀을 때의 위치
    private Vector2 endPos;//화면에서 손을 땠을 떄의 위치
    public  bool isTouch;

    //값 반환 함수
    public Vector2 GetInitPos() { return initPos; }
    public Vector2 GetEndPos() { return endPos; }
    public bool GetIsTouch() { return isTouch; }
    public Vector2 getDragDirection() { return dragDirection; }

    void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        membraneCreator = GameObject.Find("Player").GetComponent<MembraneCreator>();

        isTouch = false;
        initPos = Vector2.zero;
        endPos = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        initPos = Input.mousePosition;
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        endPos = Input.mousePosition;
        dragDirection = (endPos - initPos).normalized; //화면 드래그 방향
        isTouch = false;
        if ((playerImageController.getPlayingChr() == 1) && (bottleSelectController.bottleController.isSuperPowerAvailabe))
        {
            membraneCreator.SetMembraneAvailable(true); //탄성막을 한 개 생성하기 위해서는 탄성막 생성자를 조작하는 도중 한 번의 터치를 해야한다.
        } 
    }
}
