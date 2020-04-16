using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zoom_in_out : MonoBehaviour
{

    Camera mainCamera; // 메인카메라

    float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier; // 차례대로 이전 터치 간격, 현재 터치 간격, 줌 정도

    Vector2 firstTouchPrevPos, secondTouchPrevPos; // 두 번의 터치

    [SerializeField]
    float zoomModifierspeed = 0.1f;// 속도

    [SerializeField]
    Text text;

    



    void Start()
    {
        mainCamera = GetComponent<Camera>();

        
    }

    void Update()
    {
        if (Input.touchCount == 2) // 2개의 입력 감지
        {
            Touch firstTouch = Input.GetTouch(0);// 입력   
            Touch secondTouch = Input.GetTouch(1); // 입력

            firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition; //변위값의 차
            secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;//변위값의 차

            touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude; // 이전 변위값들의 차의 크기
            touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;// 현재 변위값들의 차의 크기

            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierspeed; // 줌을 얼마나 땡길지

            if (touchesPrevPosDifference > touchesCurPosDifference)
                mainCamera.orthographicSize += zoomModifier;
            if (touchesPrevPosDifference < touchesCurPosDifference)
                mainCamera.orthographicSize -= zoomModifier;
                     
        }

        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2f, 10f);
        text.text = "Camera size" + mainCamera.orthographicSize;
    }
}
