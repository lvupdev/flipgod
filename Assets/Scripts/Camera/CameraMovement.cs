using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{

    public GameObject backgroud; //스테이지의 배경 오브젝트 (rect Transform을 가지고 있어야 함)

    private GameObject membranes;
    private BottleSelectController bottleSelectController;
    private SuperPowerPanelController superPowerPanelController;
    private RectTransform rect_background; //배경 오브젝트 rect transform
    private float backgroundWidth; // 배경 오브젝트 너비
    private float backgroundHeight; //배경 오브젝트 높이

    private Vector3 startPos, curPos; //마우스의 월드 좌표
    private Vector3 expectPosition; //카메라의 예상 위치
    private bool hold; //화면을 누르고 있는지의 여부
    private bool membraneHold; //탄성막을 누르고 있는지의 여부

    private float Max_X; //카메라가 이동할 수 있는 x위치의 최대 절댓값
    private float Max_Y; //카메라가 이동할 수 있는 y위치의 최대 절댓값
    private int key_X; //x좌표 부호
    private int key_Y; //y좌표 부호


    //zoom in out
    private Camera presentCamera; // 현재 카메라

    float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier; // 차례대로 이전 터치 간격, 현재 터치 간격, 줌 정도

    Vector2 firstTouchPrevPos, secondTouchPrevPos; // 두 번의 터치

    [SerializeField]
    float zoomModifierspeed = 0.1f;// 속도

    [SerializeField]
    Text text;

    



    void Start()
    {
        membranes = GameObject.Find("Membranes");
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        superPowerPanelController = GameObject.Find("Panel_SuperPower").GetComponent<SuperPowerPanelController>();
        rect_background = backgroud.GetComponent<RectTransform>();
        backgroundWidth = rect_background.rect.width * rect_background.localScale.x;
        backgroundHeight = rect_background.rect.height * rect_background.localScale.y;
        presentCamera = GetComponent<Camera>();
        hold = false;
        membraneHold = false;
        SetCameraSize();
    }

    private void SetCameraSize() //처음 시작할 때나 카메라 사이즈를 조절할 떄 실행
    {
        Max_X = ((backgroundWidth / 2) - presentCamera.aspect * presentCamera.orthographicSize);
        if (Max_X < 0) Max_X = 0;
        Max_Y = (backgroundHeight/2)- presentCamera.orthographicSize;
    }

    void Update()
    {
        if ((!bottleSelectController.bottleController.isSuperPowerAvailabe) && superPowerPanelController.GetIsTouch()) //초능력 사용 중(물병이 날아가는 도중)에는 스와이프나 줌인/아웃 사용 불가, superpowe패널을 터치해야 스와이프 가능
        {
            //swipe
            if (!membraneHold)//Input.touchCount == 1)
            {
                if (!hold)
                {
                    startPos = Input.mousePosition;
                    Vector2 pos = presentCamera.ScreenToWorldPoint(startPos);
                    Ray2D ray = new Ray2D(pos, Vector2.zero);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);

                    foreach (var hit in hits) //처음 터치한 위치에 탄성막이 존재하면 스와이프를 진행하지 않음
                    {
                        if (hit.collider.transform.parent == membranes.transform)
                            membraneHold = true;
                    }

                    hold = membraneHold ? false : true;

                }

                curPos = Input.mousePosition;

                if (hold)
                {
                    switch (CheckBoundary())
                    {
                        case 0: //x좌표 y좌표 모두 최대 상태일 때
                            transform.position = new Vector3(key_X * Max_X, key_Y * Max_Y, transform.position.z);
                            break;
                        case 1: //y좌표만 최대 상태일 때. 즉 x좌표는 이동할 수 있을 때
                            transform.position = new Vector3(expectPosition.x, key_Y * Max_Y, transform.position.z);;
                            break;
                        case 2: //x좌표만 최대 상태일 때. 즉 y좌표는 이동할 수 있을 때
                            transform.position = new Vector3(key_X * Max_X, expectPosition.y, transform.position.z);
                            break;
                        case 3: //둘 다 이동할 수 있을 때
                            transform.position = expectPosition;
                            break;
                    }
                    startPos = curPos;
                }
            }

            //zoom in out
            if (Input.touchCount >= 2) // 2개의 입력 감지
            {
                Touch firstTouch = Input.GetTouch(0);// 입력   
                Touch secondTouch = Input.GetTouch(1); // 입력

                firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition; //변위값의 차
                secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;//변위값의 차

                touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude; // 이전 변위값들의 차의 크기
                touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;// 현재 변위값들의 차의 크기

                zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierspeed; // 줌을 얼마나 땡길지

                if (touchesPrevPosDifference > touchesCurPosDifference)
                    presentCamera.orthographicSize += zoomModifier;
                if (touchesPrevPosDifference < touchesCurPosDifference)
                    presentCamera.orthographicSize -= zoomModifier;

            }

            //presemtCamera.orthographicSize = Mathf.Clamp(presemtCamera.orthographicSize, 2f, 10f);
        }
        else
        {
            hold = false;
            membraneHold = false;
        }
    }

    // check if outside the boundary
    public int CheckBoundary()  // 카메라의 시점을 제한해서 게임화면만 보여주기 위함.
    {
        expectPosition = transform.position + presentCamera.ScreenToWorldPoint(startPos) - presentCamera.ScreenToWorldPoint(curPos) + backgroud.transform.position; //이동할 것으로 예측되는 position. 뒤에 배경화면의 포지션을 더한 것은 배경화면이 원점에 위치하지 않을 경우
        
        if ((Math.Abs(expectPosition.x) > Max_X) && (Math.Abs(expectPosition.y) > Max_Y)) //x좌표 y좌표 모두 최대 상태일 때
        {
            key_X = expectPosition.x > 0 ? 1 : -1;
            key_Y = expectPosition.y > 0 ? 1 : -1;
            return 0;
        }
        else if ((Math.Abs(expectPosition.x) <= Max_X) && (Math.Abs(expectPosition.y) > Max_Y)) //y좌표만 최대 상태일 때. 즉 x좌표는 이동할 수 있을 때
        {
            key_X = expectPosition.x > 0 ? 1 : -1;
            key_Y = expectPosition.y > 0 ? 1 : -1;
            return 1;
        }
        else if ((Math.Abs(expectPosition.x) > Max_X) && (Math.Abs(expectPosition.y) <= Max_Y))//x좌표만 최대 상태일 때. 즉 y좌표는 이동할 수 있을 때
        {
            key_X = expectPosition.x > 0 ? 1 : -1;
            key_Y = expectPosition.y > 0 ? 1 : -1;
            return 2;
        }
        else //둘 다 이동할 수 있을 때
            return 3;
    }

}
