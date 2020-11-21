using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FreezeEffect : MonoBehaviour
{
    public List<GameObject> TargetObject = new List<GameObject>(); //범위 내에 있는 구조물들의 배열
    private TensionGaugeManager tensionGaugeManager;
    private ResourceManager gameResourceValue;
    private int superPowerLV; //초능력 강화 레벨
    private int newFreezedCount; //새롭게 빙결된 구조물의 갯수 

    private void Awake()
    {
        tensionGaugeManager = GameObject.Find("Image_TensionGaugeBar").GetComponent<TensionGaugeManager>();
        gameResourceValue = GameObject.Find("GameResource").GetComponent<ResourceManager>();
        superPowerLV = gameResourceValue.GetSuperPowerLV(2);
        newFreezedCount = 0;

        //초기 범위 이미지 반지름 설정
        transform.localScale = new Vector3(1.075f * superPowerLV, superPowerLV, 1);

    }

    public void Freeze(int whichCase) //0이면 기본 초능력 사용시, 1이면 필살기 사용시
    {   
        for (int i = 0; i < TargetObject.Count; i++)
        {
            Structure structure = TargetObject[i].GetComponent<Structure>();

            if (!structure.isFreezed) //구조물이 이미 얼어있는 상태가 아닐 떄
			{
                structure.isFreezed = true;
				if (structure.getFreezeBonus)
				{
                    newFreezedCount++;
                }
            }
            structure.delta = 0;
        }

        if(whichCase == 0) tensionGaugeManager.IncreaseTensionGauge(3, newFreezedCount); //기본 초능력으로 구조물을 얼렸을 때만 텐션게이지 상승

        newFreezedCount = 0;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
		try
		{
            if (collision.GetComponent<Structure>().isFreezable == true) TargetObject.Add(collision.gameObject);
        }
        catch (NullReferenceException e) //콜라이더 안에 들어온 오브젝트가 Structure 타입이 아니면 return한다.
		{
            return;
		}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        try
        {
            if (collision.GetComponent<Structure>().isFreezable == true) TargetObject.Remove(collision.gameObject);
        }
        catch (NullReferenceException e) //콜라이더 안에 들어온 오브젝트가 Structure 타입이 아니면 return한다.
        {
            return;
        }
    }
}