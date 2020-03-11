using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SuperPowerPanelController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private SuperPowerController superPowerController;
    private BottleController bottleController;
    private PlayerChange playerChange;
    private Vector2 initPos;//화면을 눌렀을 때의 위치
    private Vector2 endPos;//화면에서 손을 땠을 떄의 위치
    private bool isTouch;

    //값 전달 함수
    public Vector2 GetInitPos() { return initPos; }
    public Vector2 GetEndPos() { return endPos; }
    public bool GetIsTouch() { return isTouch; }

    void Start()
    {
        bottleController = GameObject.Find("BottlePrefab").GetComponent<BottleController>(); ;
        playerChange = GameObject.Find("Player").GetComponent<PlayerChange>();
        superPowerController = GameObject.Find("Player").GetComponent<SuperPowerController>();

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
        if((playerChange.playingChr == 1) && (bottleController.isSuperPowerAvailabe))
        {
            superPowerController.SetMembraneAvailable(true); //탄성막을 한 개 생성하기 위해서는 탄성막 생성자를 조작하는 도중 한 번의 터치를 해야한다.
        } 
    }
}
