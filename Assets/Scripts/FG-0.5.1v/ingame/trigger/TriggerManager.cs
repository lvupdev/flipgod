using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ingame;

public class TriggerManager : MonoBehaviour
{
    /* 트리거: 개
     * 효과: 물병 회수
     * 발동 조건: 상시 발동
     * 발동 중단 조건: 중단 트리거(개 간식) 발동
     * 설명: 플레이어가 던진 물병의 태그가 unActBottle이 되면 해당 물병을 물어서 회수한다.
     */
    
    private GameObject parentTrigger;
    List<Trigger> triggers;

    // Start is called before the first frame update
    void Start()
    {
        parentTrigger = GameObject.Find("Trigger");
        triggers = new List<Trigger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Trigger SetTrigger(string triggerName)
    {
        Trigger trigger;

        trigger = new Trigger();
        
        switch(triggerName)
        {
            case "Dog":
                trigger = new Trigger();
                trigger.actCond = new TrgAct("상시 발동");
                trigger.effect = new TrgEff("물병 회수");
                trigger.deactCond = new TrgDeact("개 간식");
                break;

        }

        triggers.Add(trigger);
        return trigger;
    }

    public GameObject GenerateTriggerObject(string triggerName)
    {
        // Rigidbody Component는 넣어놓고 리소스화
        GameObject childTrigger = Instantiate(Resources.Load(triggerName)) as GameObject;
        Trigger trigger = SetTrigger(triggerName);
        // 이 trigger 컴포넌트를 childTrigger에 Add할 수 없을까?

        return childTrigger;
    }
}
