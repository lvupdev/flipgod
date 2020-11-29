using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//움직이는 타일 위에 올라오는 물병들을 카운트하는 스크립트
public class AboveRange : MonoBehaviour
{
	PlaneTile planeTile;
	GameObject bottles;


	void Start()
	{
		planeTile = gameObject.GetComponentInParent<PlaneTile>();
		bottles = GameObject.Find("Bottles");
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		BottleCollision bottle = col.gameObject.GetComponent<BottleCollision>();

		if (bottle == null) return; //범위 콜라이더에서 떨어진 물체가 물병이 아니면 리턴
		else
		{
			bottle.rangePlaneTile.Add(planeTile);
		}
	}


	void OnTriggerExit2D(Collider2D col)
	{
		BottleCollision bottle = col.gameObject.GetComponent<BottleCollision>();

		if (bottle == null) return; //범위 콜라이더에서 떨어진 물체가 물병이 아니면 리턴
		else
		{
			bottle.rangePlaneTile.Remove(planeTile);

			bottle.transform.SetParent(bottles.transform);
		}
	}
}
