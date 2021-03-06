﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*
힘 조절 패드
*/
public class PadStrength : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private BottleSelectController bottleSelectController;
    private CameraMovement mainCamera;
    private CameraMovement colorCamera;
    public bool isTouch = false;
    public bool isThrowing = false; //캐릭터가 물병을 던지는 동작을 진행중인가의 여부

    //{get;set;}을 하면 코드 내에서 수정은 가능하나, 유니티에서 보여지지 않음
    public float totalStrength { get; set; }
    private int addStrength = 24; //시간별로 더해지는 힘 값, 조정 가능

    private GameObject strengthGauge; //힘 게이지 스프라이트
    private float delayTime = 1f; //힘 조절 버튼에서 손가락을 때고 물병이 던져지기까지의 딜레이 타임
    public int count; // 물병던진 횟수 카운트 변수

    void OnGUI() //NEW: 유니티 GUI 함수 오버라이딩
    {
        GUILayout.BeginArea(new Rect(100,0,100,90));

        GUILayout.Label("Strength: " + (int)totalStrength);

        GUILayout.EndArea();

        
    }

    void Start()
    {
        strengthGauge = GameObject.Find("StrengthGauge");
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        mainCamera = Camera.main.gameObject.GetComponent<CameraMovement>();
        colorCamera = GameObject.Find("Color Camera").GetComponent<CameraMovement>();
        totalStrength = 0;
        count = 0;
        
    }


    void FixedUpdate()
    {

        if (isTouch && (!bottleSelectController.bottleController.isSuperPowerAvailabe) && (!isThrowing) && (totalStrength < addStrength))//패드가 눌려있고 물병을 던지는 도중이 아니며 물병이 날아가는 도중이 아니면
        {

            totalStrength += addStrength * Time.fixedDeltaTime; // 매 초마다 일정한 힘을 더한다.
            this.strengthGauge.GetComponent<Image>().fillAmount += 1.0f * Time.fixedDeltaTime; // 힘 게이지가 꽉 채워지는 데 1초가 걸린다.
        }

        if (isThrowing || (totalStrength >= addStrength)) //패드에서 마우스를 뗐거나 힘 버튼을 1초 이상 눌렀을 때
        {
            delayTime -= Time.fixedDeltaTime; //딜레이 타임만큼 던지는 동작이 지연된다.

            if (delayTime <= 0)
            {
                strengthGauge.gameObject.SetActive(false); // 힘 게이지를 화면에서 제거한다.
                this.strengthGauge.GetComponent<Image>().fillAmount = 0;
                bottleSelectController.bottleController.Jump();
                isThrowing = false;
                totalStrength = 0;
                count++;
                print(count);

            }
        }
    }

   
    public void OnPointerDown(PointerEventData eventData) //패드를 누르는 순간
    {
        if ((!bottleSelectController.bottleController.isSuperPowerAvailabe) && (!isThrowing))
        {
            //물병의 위치에 따라 카메라 이동
            mainCamera.autoMoving = true; 
            colorCamera.autoMoving = true;

            delayTime = 1f; //딜레이 타임 초기화
            isTouch = true;
            strengthGauge.gameObject.SetActive(true); //힘 게이지를 화면에 표시한다.
        }
    }

    public void OnPointerUp(PointerEventData eventData) //패드에서 마우스를 떼는 순간
    {
        isTouch = false;
        if (bottleSelectController.bottleController.isSuperPowerAvailabe || totalStrength == 0) isThrowing = false;
        else isThrowing = true;
    }
}
