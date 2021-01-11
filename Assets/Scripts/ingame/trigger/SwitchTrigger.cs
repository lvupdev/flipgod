using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTrigger : TriggerFunction
{
    public Sprite pushedImage;

    private new void Start()
    {
        base.Start();

        isActTrigger = true; //해당 트리거가 처음에 활성화 상태인지 비활성화 상태인지 start함수에서 반드시 명시해줘야 한다.
        isFreezable = true; //해당 트리거가 얼릴 수 있는 트리거인지여 여부를 반드시 명시해줘야 한다.


    }

    void Update()
    {
        if (actBool && isFreezable && structure.isFreezed)
        {
            shouldBeFreezed = isActTrigger ? true : false; //빙결되기 전에 isActTrigger 상태였는지 shouldBeFreezed에 저장
            isActTrigger = false;
            actBool = false;
        }

        if (!actBool && !structure.isFreezed)
        {
            isActTrigger = shouldBeFreezed ? true : false; //방결되기 전에 isActTrigger상태였으면 isActTrigger은 true
            actBool = true;
        }

        if (isActTrigger)
        {
            // 트리거 발동 조건 함수
            if (EnoughCrash(1)) //스위치에 한 번 부딛히면 발동
            {
                conditionFullfilled = true; //트리거 작동 권한 부여
            }

            if (conditionFullfilled)
            {
                //트리거 발동 효과 함수
                Unactivate();

                structure.GetComponent<SpriteRenderer>().sprite = pushedImage;
            }

            
            if(TimeOut(1)) //1초가 지나면 비활성화
            {
                conditionFullfilled = false;
            }
        }
    }
}
