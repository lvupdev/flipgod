using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ingame{
    public class TriggerManager {

        /*
        ●	강아지
            효과: 물병 회수
            발동 조건: 상시 발동
            발동 중단 조건: 중단 트리거(개 간식) 발동
            설명: 플레이어가 던진 물병의 태그가 unActBottle이 되면 해당 물병을 물어서 회수한다. 발동 중단 트리거는 ‘개 간식’이다.
        */

        //설정
        Trigger trg1 = new Trigger();
        trg1.setActCond = new TrgAct("상시발동");
        trg1.setEffect = new TrgEff("물병회수");
        trg1.setDeactCond = new TrgDeact("중단트리거");

        //실행
        trg1.activate(); //Trigger 클래스의 activate() 가, 
        //actCond 객체의 TrgAct 클래스의 activate() 를 실행한다.

    }
}