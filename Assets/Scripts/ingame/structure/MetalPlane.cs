using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalPlane : Structure
{
    public bool isSpinning; //타일이 회전하는지의 여부
    public float spinningSpeed;  //타일이 회전하는 속도
    public bool isMoving; //타일이 이동하는지의 여부
    public Vector3 direction; //이동방향
    public float movingSpeed; //타일이 이동하는 속도
    public float movingRange; //이동 범위

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
}
