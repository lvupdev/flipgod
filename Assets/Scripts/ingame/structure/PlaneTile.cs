using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//모든 평평한 타일, 즉 물병을 주로 세우게 될 타일을 관리하는 스크립트

public class PlaneTile : Structure
{
    public bool isSpinning; //타일이 회전하는지의 여부
    public float spinningSpeed;  //타일이 회전하는 속도
    public bool isMoving; //타일이 이동하는지의 여부
    public Vector3 direction; //이동방향
    public float movingSpeed; //타일이 이동하는 속도
    public float movingRange; //이동 범위
    public List<BottleCollision> bottleAbove = new List<BottleCollision>(); //위에 올려져 있는 물병들의 리스트
    public int bottleAboveCount;

    private Vector3 originPos; //처음 배치 위치
    private int key; //이동 방향 전환 키
    private bool turnSucees; //성공적으로 방향을 전환했는지의 여부

    public new void Start()
    {
        base.Start();

        key = 1;
        direction = Vector3.ClampMagnitude(direction, 1);
        originPos = transform.position;
        turnSucees = false;
    }


    void Update()
    {
        ThawDynamicStructure(isFreezed);

        if (!isFreezed)
            MoveDynamicStructure();

        bottleAboveCount = bottleAbove.Count;
    }

    public void MoveDynamicStructure()
    {
        if(isSpinning)
            transform.Rotate(new Vector3(0, 0, -360 * spinningSpeed * Time.deltaTime));

        if (isMoving)
        {
            transform.Translate(key * direction * movingSpeed * Time.deltaTime);

            if((originPos - transform.position).magnitude > movingRange)
			{
				if (!turnSucees)
				{
                    key = -key;
                    turnSucees = true;
                }
			}
			else
			{
                turnSucees = false;
			}


        }
    }

    public void OnCollisionStay2D(Collision2D col)
	{
        BottleCollision bottle = col.gameObject.GetComponent<BottleCollision>();

        if (bottle == null) return; //부딛힌 물체가 물병이 아니면 리턴
        else
        {
            bottleAbove.AddRange(bottle.bottleChain); //접한 물병의 리스트 추가
            bottleAbove = bottleAbove.Distinct().ToList(); //중복 요소 제거
            bottle.contactPlaneTile.Add(GetComponent<PlaneTile>());
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        BottleCollision bottleCollision = col.gameObject.GetComponent<BottleCollision>();

        if (bottleCollision == null) return; //떨어진 물체가 물병이 아니면 리턴
        else
        {
            bottleAbove = bottleAbove.Except(bottleCollision.bottleChain).ToList(); //떨어져 나가는 물병의 bottleChain 리스트 요소들을 제거
            bottleCollision.contactPlaneTile.Remove(GetComponent<PlaneTile>());
        }
    }
}
