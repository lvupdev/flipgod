using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SuperPowerPanelController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private MembraneCreatorController membraneCreatorController;
    private BottleController bottleController;
    private PlayerImageController playerImageController;
    private Vector2 initPos;//화면을 눌렀을 때의 위치
    private Vector2 endPos;//화면에서 손을 땠을 떄의 위치
    private bool isTouch;

    //값 반환 함수
    public Vector2 GetInitPos() { return initPos; }
    public Vector2 GetEndPos() { return endPos; }
    public bool GetIsTouch() { return isTouch; }

    void Start()
    {
        bottleController = GameObject.Find("BottlePrefab").GetComponent<BottleController>(); ;
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        membraneCreatorController = GameObject.Find("MembraneCreator").GetComponent<MembraneCreatorController>();

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
        isTouch = false;
        if ((playerImageController.playingChr == 1) && (bottleController.isSuperPowerAvailabe))
        {
            membraneCreatorController.SetMembraneAvailable(true); //탄성막을 한 개 생성하기 위해서는 탄성막 생성자를 조작하는 도중 한 번의 터치를 해야한다.
        } 
    }

    public void ReselectBottle()
    {
        bottleController = GameObject.FindWithTag("isActBottle").GetComponent<BottleController>();
    }
}
