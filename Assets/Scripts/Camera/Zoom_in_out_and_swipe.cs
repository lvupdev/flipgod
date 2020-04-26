using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zoom_in_out_and_swipe : MonoBehaviour
{

    //swipe
    const int MAX_X = 5; 
    const int MIN_X = -5; 
    const int MAX_Y = 5; 
    const int MIN_Y = -5;
    const float fscale = 0.01f;


    Vector2 vscale = new Vector2(fscale, fscale);
    Vector2 startPos, curPos;
    bool hold;
    
    //zoom in out
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

        //swipe
        if (Input.touchCount == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.hold = true;
                this.startPos = Input.mousePosition; // 처음 위치 (누르고 있는 동안은 값이 변하지않음)
            }

            else if (Input.GetMouseButtonUp(0))
            {
                this.hold = false;
            }

            if (this.hold)
            {
                this.curPos = Input.mousePosition; // 계속 터치 위치를 받아옴 (손가락이 움직이는 동안 값 바뀜)
                transform.Translate((mainCamera.orthographicSize / 5) * vscale * (startPos - curPos)); 
                
                if (outside()) 
                    transform.Translate((mainCamera.orthographicSize / 5) * vscale * (curPos - startPos)); 


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
                mainCamera.orthographicSize += zoomModifier;
            if (touchesPrevPosDifference < touchesCurPosDifference)
                mainCamera.orthographicSize -= zoomModifier;
                     
        }

        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2f, 10f);
    }

    // check if outside the boundary
    bool outside()  // 카메라의 시점을 제한해서 게임화면만 보여주기 위함.
    { 
        if (transform.position.x > MAX_X || transform.position.x < MIN_X || transform.position.y > MAX_Y || transform.position.y < MIN_Y)
            return true;
        else 
            return false;
    }

}
