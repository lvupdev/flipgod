using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PadDirection : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    private RectTransform rect_Background;
    private RectTransform rect_Joystick;

    private float radius;

    private Vector2 position;

    public Vector2 direction;

    public bool isTouch = false;



    void Start()
    {
        rect_Background = GameObject.Find("Background").GetComponent<RectTransform>();
        rect_Joystick = GameObject.Find("Joystick").GetComponent<RectTransform>();

        radius = rect_Background.rect.width * 0.5f;
        // 너비의 반이 반지름이 된다
    }

    void Update()
    {
        if (isTouch)
        {
            HandleOut();
        }
    }

    private Vector2 HandleOut()
    {
        direction = position;

        print(direction);

        return direction;
    }

    public void OnDrag(PointerEventData eventData)
    {


        Vector2 value = eventData.position - (Vector2)rect_Background.position;

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

        rect_Joystick.localPosition = Vector3.zero;


    }



}
