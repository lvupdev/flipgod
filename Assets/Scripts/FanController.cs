﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanController : TriggerFunction
{
    private void Start()
    {
        isActTrigger = true; //해당 트리거가 처음에 활성화 상태인지 비활성화 상태인지 start함수에서 반드시 명시해줘야 한다.
    }

    void Update()
    {
        if (isActTrigger)
        {
            // 트리거 발동 조건 함수
            if (Always())
            {
                conditionFullfilled = true; //트리거 작동 권한 부여
            }

            if (conditionFullfilled)
            {
                //트리거 발동 효과 함수
                BottleMagnet(1, 10);
            }

            /*
            if(트리거 발동 중단 함수)
            {
                conditionFullfilled = false;
            }
            */
        }
    }
}
