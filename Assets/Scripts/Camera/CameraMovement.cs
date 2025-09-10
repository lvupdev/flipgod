using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{

    public GameObject backgroud; //스테이지의 배경 오브젝트 (rect Transform을 가지고 있어야 함)
    public bool autoMoving; //카메라가 자동으로 이동하고 있는 상태인지의 여부

    private GameObject membranes;
    private BottleSelectController bottleSelectController;
    private SuperPowerPanelController superPowerPanelController;
    private ScreenEffectController screenEffectController;
    private RectTransform rect_background; //배경 오브젝트 rect transform
    private float backgroundWidth; // 배경 오브젝트 너비
    private float backgroundHeight; //배경 오브젝트 높이

    private Vector3 startPos, curPos; //마우스의 월드 좌표
    private Vector3 expectPosition; //카메라의 예상 위치
    private bool hold; //화면을 누르고 있는지의 여부
    private bool membraneHold; //탄성막을 누르고 있는지의 여부

    private float defaultCameraSize; //카메라 기본 사이즈
    private float bottleMoveGap = 4; //물병을 따라 카메라가 이동하게 되는 차이 값 
    private float Max_X; //카메라가 이동할 수 있는 x위치의 최대 절댓값
    private float Max_Y; //카메라가 이동할 수 있는 y위치의 최대값
    private float Min_Y; //카메라가 이동할 수 있는 y위치의 최소값
    private int key_X; //x좌표 부호
    private int key_Y; //y좌표 부호

    private Camera presentCamera; // 현재 카메라

    float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier; // 차례대로 이전 터치 간격, 현재 터치 간격, 줌 정도

    Vector2 firstTouchPrevPos, secondTouchPrevPos; // 두 번의 터치

    float zoomModifierspeed = 0.01f;// 속도

    private Vector2 distance; //물병과 카메라 포지션 사이의 거리



    void Start()
    {
        membranes = GameObject.Find("Membranes");
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        superPowerPanelController = GameObject.Find("Panel_SuperPower").GetComponent<SuperPowerPanelController>();
        rect_background = backgroud.GetComponent<RectTransform>();
        screenEffectController = Camera.main.transform.GetComponent<ScreenEffectController>();
        backgroundWidth = rect_background.rect.width * rect_background.localScale.x;
        backgroundHeight = rect_background.rect.height * rect_background.localScale.y;
        presentCamera = GetComponent<Camera>();
        defaultCameraSize = presentCamera.orthographicSize;
        hold = false;
        membraneHold = false;
        autoMoving = false;
        SetCameraSize();
    }

    private void SetCameraSize() //처음 시작할 때나 카메라 사이즈를 조절할 떄 실행
    {
        Max_X = ((backgroundWidth / 2) - presentCamera.aspect * presentCamera.orthographicSize);
        if (Max_X < 0) Max_X = 0;
        Min_Y = -defaultCameraSize + presentCamera.orthographicSize;
        Max_Y = (backgroundHeight / 2) - presentCamera.orthographicSize;
    }

    void Update()
    {
        //초능력 사용 중(물병이 날아가는 도중)에는 스와이프나 줌인/아웃 사용 불가, superpower패널을 터치해야 스와이프 가능, 카메라가 autoMoving상태이면 안 됨
        if ((!bottleSelectController.bottleController.isSuperPowerAvailabe) && superPowerPanelController.GetIsTouch() && !autoMoving)
        {
            //swipe
            if (!membraneHold && Input.touchCount == 1)
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
                        case 0: //x좌표가 최대이고 y좌표 최소 또는 최대 상태일 때
                            if (key_Y > 0)
                                transform.position = new Vector3(key_X * Max_X, Max_Y, transform.position.z);
                            else
                                transform.position = new Vector3(key_X * Max_X, Min_Y, transform.position.z);
                            break;
                        case 1: //y좌표가 최대 혹은 최소 상태일 때. 즉 x좌표는 이동할 수 있을 때
                            if(key_Y > 0)
                                transform.position = new Vector3(expectPosition.x, Max_Y, transform.position.z);
                            else
                                transform.position = new Vector3(expectPosition.x, Min_Y, transform.position.z);
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
                {
                    if(((backgroundHeight / 2 + defaultCameraSize) / 2) < (backgroundWidth / (2 * presentCamera.aspect))) //세로 길이가 먼저 찰 때
                    {
                        float tempSize = presentCamera.orthographicSize + zoomModifier;

                        if (tempSize > (backgroundHeight / 2 + defaultCameraSize) / 2)
                            presentCamera.orthographicSize = (backgroundHeight / 2 + defaultCameraSize) / 2;
                        else
                            presentCamera.orthographicSize = tempSize;
                    }
                    else //가로 길이가 먼저 찰 때
                    {
                        float tempSize = presentCamera.orthographicSize + zoomModifier;

                        if (tempSize > backgroundWidth / (2 * presentCamera.aspect))
                            presentCamera.orthographicSize = backgroundWidth / (2 * presentCamera.aspect);
                        else
                            presentCamera.orthographicSize = tempSize;
                    }
                }
                if (touchesPrevPosDifference < touchesCurPosDifference)
                {
                    float tempSize = presentCamera.orthographicSize - zoomModifier;
                    if (tempSize >= defaultCameraSize)
                        presentCamera.orthographicSize = tempSize;
                    else
                        presentCamera.orthographicSize = defaultCameraSize;
                }

                SetCameraSize();

                switch (CheckBoundary()) //카메라 중심 좌표를 이동
                {
                    case 0: //x좌표가 최대이고 y좌표 최소 또는 최대 상태일 때
                        if (key_Y > 0)
                            transform.position = new Vector3(key_X * Max_X, Max_Y, transform.position.z);
                        else
                            transform.position = new Vector3(key_X * Max_X, Min_Y, transform.position.z);
                        break;
                    case 1: //y좌표가 최대 혹은 최소 상태일 때. 즉 x좌표는 이동할 수 있을 때
                        if (key_Y > 0)
                            transform.position = new Vector3(expectPosition.x, Max_Y, transform.position.z);
                        else
                            transform.position = new Vector3(expectPosition.x, Min_Y, transform.position.z);
                        break;
                    case 2: //x좌표만 최대 상태일 때. 즉 y좌표는 이동할 수 있을 때
                        transform.position = new Vector3(key_X * Max_X, expectPosition.y, transform.position.z);
                        break;
                    case 3: //둘 다 이동할 수 있을 때
                        break;
                }
                if(((backgroundHeight / 2 + 6) / 2) < backgroundWidth)
                presentCamera.orthographicSize = Mathf.Clamp(presentCamera.orthographicSize, defaultCameraSize, (backgroundHeight/2 + defaultCameraSize)/ 2);
                screenEffectController.height = 2 * presentCamera.orthographicSize;
                screenEffectController.width = 2 * presentCamera.orthographicSize * presentCamera.aspect;
            }
        }
        else
        {
            hold = false;
            membraneHold = false;
        }

        if (bottleSelectController.bottleSelected && bottleSelectController.bottleController.isSuperPowerAvailabe &&bottleSelectController.bottle != null) //물병의 이동에 따라 카메라가 함께 이동함
        {
            distance = bottleSelectController.bottle.transform.position - transform.position;
            if ((Math.Abs(distance.x) > presentCamera.orthographicSize * presentCamera.aspect - bottleMoveGap) || (Math.Abs(distance.y) > presentCamera.orthographicSize - bottleMoveGap))
            {
                switch (CheckBoundary())
                {
                    case 0: //x좌표가 최대이고 y좌표 최소 또는 최대 상태일 때
                        if (key_Y > 0)
                            transform.position = new Vector3(key_X * Max_X, Max_Y, transform.position.z);
                        else
                            transform.position = new Vector3(key_X * Max_X, Min_Y, transform.position.z);
                        break;
                    case 1: //y좌표가 최대 혹은 최소 상태일 때. 즉 x좌표는 이동할 수 있을 때
                        if (key_Y > 0)
                            transform.position = new Vector3(expectPosition.x, Max_Y, transform.position.z);
                        else
                            transform.position = new Vector3(expectPosition.x, Min_Y, transform.position.z);
                        break;
                    case 2: //x좌표만 최대 상태일 때. 즉 y좌표는 이동할 수 있을 때
                        transform.position = new Vector3(key_X * Max_X, expectPosition.y, transform.position.z);
                        break;
                    case 3: //둘 다 이동할 수 있을 때
                        transform.position = expectPosition;
                        break;
                }
            }
        }

		if (autoMoving) //힘 버튼을 눌렀을 때 물병이 이동하는 것
		{
            distance = bottleSelectController.bottle.transform.position - transform.position;
            if ((Math.Abs(distance.x) > presentCamera.orthographicSize * presentCamera.aspect - bottleMoveGap) || (Math.Abs(distance.y) > presentCamera.orthographicSize - bottleMoveGap))
            {
                switch (CheckBoundary())
                {
                    case 0: //x좌표가 최대이고 y좌표 최소 또는 최대 상태일 때
                        if (key_Y > 0)
						{
                            transform.position = Vector3.Lerp(transform.position, new Vector3(key_X * Max_X, Max_Y, transform.position.z), 8 * Time.deltaTime);
                            if ((transform.position - new Vector3(key_X * Max_X, Max_Y, transform.position.z)).magnitude < 0.1f)
                            {
                                transform.position = new Vector3(key_X * Max_X, Max_Y, transform.position.z);
                                autoMoving = false;
                            }
                        }
						else
						{
                            transform.position = Vector3.Lerp(transform.position, new Vector3(key_X * Max_X, Min_Y, transform.position.z), 8 * Time.deltaTime);
                            if ((transform.position - new Vector3(key_X * Max_X, Min_Y, transform.position.z)).magnitude < 0.1f)
                            {
                                transform.position = new Vector3(key_X * Max_X, Min_Y, transform.position.z);
                                autoMoving = false;
                            }
                        }
                        break;
                    case 1: //y좌표가 최대 혹은 최소 상태일 때. 즉 x좌표는 이동할 수 있을 때
                        if (key_Y > 0)
						{
                            transform.position = Vector3.Lerp(transform.position, new Vector3(expectPosition.x, Max_Y, transform.position.z), 8 * Time.deltaTime);
                            if ((transform.position - new Vector3(expectPosition.x, Max_Y, transform.position.z)).magnitude < 0.1f)
                            {
                                transform.position = new Vector3(expectPosition.x, Max_Y, transform.position.z);
                                autoMoving = false;
                            }
                        }
						else
						{
                            transform.position = Vector3.Lerp(transform.position, new Vector3(expectPosition.x, Min_Y, transform.position.z), 8 * Time.deltaTime);
                            if ((transform.position - new Vector3(expectPosition.x, Min_Y, transform.position.z)).magnitude < 0.1f)
                            {
                                transform.position = new Vector3(expectPosition.x, Min_Y, transform.position.z);
                                autoMoving = false;
                            }
                        }
                        break;
                    case 2: //x좌표만 최대 상태일 때. 즉 y좌표는 이동할 수 있을 때
                        transform.position = Vector3.Lerp(transform.position, new Vector3(key_X * Max_X, expectPosition.y, transform.position.z), 8 * Time.deltaTime);
                        if ((transform.position - new Vector3(key_X * Max_X, expectPosition.y, transform.position.z)).magnitude < 0.1f)
                        {
                            transform.position = new Vector3(key_X * Max_X, expectPosition.y, transform.position.z);
                            autoMoving = false;
                        }
                        break;
                    case 3: //둘 다 이동할 수 있을 때
                        transform.position = Vector3.Lerp(transform.position, expectPosition, 8 * Time.deltaTime);
                        if ((transform.position - expectPosition).magnitude < 0.1f)
                        {
                            transform.position = expectPosition;
                            autoMoving = false;
                        }
                        break;
                }
            }
        }
    }

    // check if outside the boundary
    public int CheckBoundary()  // 카메라의 시점을 제한해서 게임화면만 보여주기 위함.
    {
        if (bottleSelectController.bottleController.isSuperPowerAvailabe || autoMoving) //물병을 따라 이동하는 경우
        {
            expectPosition = transform.position;
            if(Math.Abs(distance.x) > presentCamera.orthographicSize * presentCamera.aspect - bottleMoveGap)
            {
                int key = distance.x > 0 ? 1 : -1;
                expectPosition += new Vector3((Math.Abs(distance.x) - (presentCamera.orthographicSize * presentCamera.aspect - bottleMoveGap)) * key,0,0);
            }
            if(Math.Abs(distance.y) > presentCamera.orthographicSize - bottleMoveGap)
            {
                int key = distance.y > 0 ? 1 : -1;
                expectPosition += new Vector3(0, (Math.Abs(distance.y) - (presentCamera.orthographicSize - bottleMoveGap)) * key, 0);
            }
        }
        else if (hold) //스와이프의 경우
            expectPosition = transform.position + presentCamera.ScreenToWorldPoint(startPos) - presentCamera.ScreenToWorldPoint(curPos); //이동할 것으로 예측되는 position
        else //줌인/아웃의 경우
            expectPosition = transform.position;

        if ((Math.Abs(expectPosition.x) > Max_X) && ((expectPosition.y > Max_Y) || expectPosition.y < Min_Y)) //x좌표가 최대이고 y좌표가 최대 또는 최소 상태일 때
        {
            key_X = expectPosition.x > 0 ? 1 : -1;
            key_Y = expectPosition.y > Max_Y ? 1 : -1;
            return 0;
        }
        else if ((Math.Abs(expectPosition.x) <= Max_X) && ((expectPosition.y > Max_Y) || expectPosition.y < Min_Y)) //y좌표만 최대 상태일 때. 즉 x좌표는 이동할 수 있을 때
        {
            key_X = expectPosition.x > 0 ? 1 : -1;
            key_Y = expectPosition.y > Max_Y ? 1 : -1;
            return 1;
        }
        else if ((Math.Abs(expectPosition.x) > Max_X) && (expectPosition.y <= Max_Y) && (expectPosition.y>=Min_Y))//x좌표만 최대 상태일 때. 즉 y좌표는 이동할 수 있을 때
        {
            key_X = expectPosition.x > 0 ? 1 : -1;
            key_Y = expectPosition.y > 0 ? 1 : -1;
            return 2;
        }
        else //둘 다 이동할 수 있을 때
            return 3;
    }

}
