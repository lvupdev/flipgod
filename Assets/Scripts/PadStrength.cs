using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
힘 조절 패드
*/
public class PadStrength : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{


    public Bottle bottle;
    private bool isTouch = false;
    public int addStrength = 3; //시간별로 더해지는 힘 값, 조정 가능

    //{get;set;}을 하면 코드 내에서 수정은 가능하나, 유니티에서 보여지지 않음
    public int strength_time { get; set; }



    void Start()
    {
    }


    void Update()
    {

        if (isTouch) //패드가 눌려있으면
        {
            strength_time += addStrength; // 매 초마다 일정한 힘을 더한다.
        }
    }

   
    public void OnPointerDown(PointerEventData eventData) //패드를 누르는 순간
    {
        strength_time = 0;
        isTouch = true;
        
    }

    public void OnPointerUp(PointerEventData eventData) //패드에서 마우스를 떼는 순간
    {
        isTouch = false;
        bottle.Jump();
    }
}
