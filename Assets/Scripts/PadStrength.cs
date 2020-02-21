using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
힘 조절 패드
*/
public class PadStrength : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public BottleController bottleController;
    public bool isTouch = false;

    void Start()
    {
        bottleController = GameObject.Find("Bottle").GetComponent<BottleController>();
    }

    public void OnPointerDown(PointerEventData eventData) //패드를 누르는 순간
    {
        bottleController.StrengthButtonDown();
    }

    public void OnPointerUp(PointerEventData eventData) //패드에서 마우스를 떼는 순간
    {
        bottleController.StrengthButtonUp();
    }

}
