using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
방향 조절 패드
*/
public class PadDirection : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    private RectTransform rect_Joypad; //패드 중 아래에 있는것 
    private RectTransform rect_Joystick; //패드 중 위에 있는것, 방향 조정.
    private double a, b, rotate;

    private float radius; //background 패드의 반지름

    private Vector2 position; //joystick 패드의 위치

    private RectTransform angle;

    private Vector2 direction; //Bottle 클래스로 넘겨지는 방향 벡터

    private bool isTouch = false;

    public Vector2 getDirection() { return direction; }
    public void setDirection(Vector2 direction) { this.direction = direction; }

    public bool getIsTouch() { return isTouch; }


    void Start()
    {
        rect_Joypad = GameObject.Find("Joypad").GetComponent<RectTransform>();
        rect_Joystick = GameObject.Find("Joystick").GetComponent<RectTransform>();
        

        radius = rect_Joypad.rect.width * 0.5f;
        // 너비의 반이 반지름이 된다
    }

    void Update()
    {
        a = rect_Joystick.localPosition.x;
        b = rect_Joystick.localPosition.y;
        rotate = Math.Atan2(a, b) * (180 / Math.PI);
        //위의 3줄은 화살표 rotation 각도를 위한 식
        
        if (isTouch)
        {
            HandleOut();
        }
    }

    private Vector2 HandleOut() //콘솔에 출력하고, 위치값 저장.
    {
        direction = position;

        //print(direction);

        return direction;
    }

    public void OnDrag(PointerEventData eventData)
    {


        Vector2 value = eventData.position - (Vector2)rect_Joypad.position;

        value = Vector2.ClampMagnitude(value, radius);

        rect_Joystick.localPosition = value;

        position = value.normalized;


    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;

        rect_Joystick.localPosition = Vector3.zero; //방향 초기화
    }



}
