﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//트리거 관리 스크립트
public class TriggerFunction : MonoBehaviour
{
    public BottleSelectController bottleSelectController;
    public List<GameObject> TargetObject = new List<GameObject>(); //트리거와 상호작용 중인 오브젝트 배열
    public GameObject bottles;
    public Structure structure; //트리거가 적용될 구조물
    public GameObject targetTrigger; //효과를 적용할 목표 트리거(트리거 활성화/비활성화 효과 때 사용)
	public TensionGaugeManager tensionGaugeManager;
    public UsefullOperation usefullOperation;

    public PadStrength padStrength;

    public bool conditionFullfilled; //트리거 발동 조건이 충족되었는지의 여부
    public bool isActTrigger; //트리거가 활성화 상태인지의 여부
    public bool isFreezable; //얼릴 수 있는 트리거인지의 여부
    public bool shouldBeFreezed; //얼려져야 하는지의 여부 
    public bool actBool; //반복 방지 변수
    public bool enoughStackBool; // EnoughStack 텐션게이지 중복 증가 방지 변수
    public int inCollidernNum; //콜라이더 안에 있는 물병의 개수
    public float intervalTime; //주기 시간
    public float operatingTime; //트리거가 발동을 지속한 시간;

    public int count;



    public void Start()
    {
        count = GameObject.Find("Pad_Strength").GetComponent<PadStrength>().count;
        structure = transform.parent.GetComponent<Structure>();
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        bottles = GameObject.Find("Bottles");
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        tensionGaugeManager = GameObject.Find("Image_TensionGaugeBar").GetComponent<TensionGaugeManager>();
        usefullOperation = GameObject.Find("GameResource").GetComponent<UsefullOperation>();

        conditionFullfilled = false;
        shouldBeFreezed = false;
        inCollidernNum = 0;
        intervalTime = 0;
        operatingTime = 0;
        actBool = true;
        enoughStackBool = true;
    }

    // 트리거 발동 조건 함수


    /*
     * 상시 발동
     */
    public bool Always()
    {
        return true;
    }

    public bool parent()
    {

        if ((count%3) == 0 && count != 0)
        {

            return true;

        }

        else

            return false;
    }
    


    /*
     * n초 주기 발동
     * time(초)을 주기로 트리거 작동 권한을 부여한다.
     */
    public bool TimeInterval(float time)
    {
        intervalTime += Time.deltaTime;
        if (time < intervalTime)
        {
            intervalTime = 0;
            return true;
        }
        else
            return false;
    }

    /*
     * n번 충돌 발동
     * num 번만큼 충돌할 때마다 트리거 작동 권한을 부여한다.
     */
    public bool EnoughCrash(int num)
    {
        if (structure.collisionNum >= num)
        {
            structure.collisionNum = 0;
            tensionGaugeManager.IncreaseTensionGauge(4, 1);
            return true;
        }
        else
            return false;
    }

    /*
     * n개 누적 발동
     * num개 이상의 물병이 콜라이더 안에 있으면 트리거 작동 권한 부여
     */
    public bool EnoughStack(int num)
    {
        if (TargetObject.Count >= num)
        {
            if (enoughStackBool)
            {
                tensionGaugeManager.IncreaseTensionGauge(4, 1);
                enoughStackBool = false;
            }
            return true;
        }
        return false;
    }

    /*
     * n개 투척 발동
     * num 번만큼 던질 때마다 트리거 작동 권한을 부여한다.
     */
    public bool ThrowAct(int num)
    {

        if (padStrength.count >= num)
        {
            padStrength.count = 0;
            return true;

        }

        else

            return false;
    }





    //트리거 발동 효과 함수


    /*
     * 물병 자석
     * key값이 1이면 척력, -1이면 인력 작용
     * strength는 자석의 힘을 나타내는 값
     */
    public void BottleMagnet(int key, float strength)
    {
        for (int i = 0; i < TargetObject.Count; i++)
        {
            Vector2 directionToMagnet = (TargetObject[i].transform.position - gameObject.transform.position).normalized; // 자석으로 향하는 벡터설정 
            float distance = Vector2.Distance(gameObject.transform.position, TargetObject[i].transform.position);// Distance 로 거리를 a,b사이의 거리를 구함
            TargetObject[i].GetComponent<Rigidbody2D>().AddForce((key * 100 * Time.fixedDeltaTime * strength * directionToMagnet / distance), ForceMode2D.Force);// 힘의 크기와 방향이 있으니까 물리적 힘 구현 rigidbody가 있어야 가능
        }
    }


    /*
     * 트리거 비활성화
     */
    public void Unactivate()
    {
        targetTrigger.GetComponent<TriggerFunction>().isActTrigger = false;
    }


    /*
     * 트리거 활성화
     */
    public void Activate()
    {
        if(targetTrigger.GetComponent<TriggerFunction>().actBool) //해당 트리거가 얼어있는 상태가 아니어야 한다.
            targetTrigger.GetComponent<TriggerFunction>().isActTrigger = true;
    }

    /*
     * 물병 제거
     * 트리거 범위 안에 들어온 물병을 제거한다.
     */
    public void BottleDestroy()
	{
        var list = new List<GameObject>();
        list.AddRange(TargetObject);

        foreach (GameObject gameObject in list)
		{
            if (gameObject.GetComponent<BottleController>() == null) continue;
			else
			{
                TargetObject.Remove(gameObject);
                Destroy(gameObject);
            }
		}
	}


    //트리거 발동 중단 함수


    /*
     * n초 경과
     * time(초)만큼의 시간이 지나면 트리거의 발동을 중단시킨다.
     */
    public bool TimeOut(float time)
    {
        if (conditionFullfilled) operatingTime += Time.deltaTime;

        if (time < operatingTime)
        {
            return true;
        }
        else
            return false;
    }


    /*
     * n번 충돌 중단
     * 트리거 발동 이후 num 횟수 만큼 충돌하면 트리거의 발동을 중단시킨다.
     */
    public bool ToMuchCrash(int num)
    {
        if (conditionFullfilled && (structure.collisionNum == num))
        {
            structure.collisionNum = 0;
            return true;
        }
        else
            return false;
    }

    /*
     * n개 누적 미달
     * num개 미만의 오브젝트가 콜라이더 안에 있으면 트리거의 발동이 중단된다.
     */
    public bool LowStack(int num)
    {
        if (TargetObject.Count < num)
        {
            if (!enoughStackBool) enoughStackBool = true;
            return true;
        }
        return false;
    }

    /*
     * n개 투척 중단
     * 트리거 발동 이후 num 번만큼 던질 때마다 트리거의 발동을 중단시킨다.
     */
    public bool ThrowInAct(int num)
    {

        if (conditionFullfilled && (padStrength.count >= num))
        {
            padStrength.count = 0;
            return true;

        }

        else

            return false;
    }


    //충돌 판정 함수

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent == bottles.transform) TargetObject.Add(collision.gameObject);
        inCollidernNum++;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent == bottles.transform) TargetObject.Remove(collision.gameObject);
    }
}