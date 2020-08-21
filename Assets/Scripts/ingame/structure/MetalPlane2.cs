using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalPlane2 : Structure
{
    public bool isSpinning; //타일이 회전하는지의 여부
    public float spinningSpeed;  //타일이 회전하는 속도
    public bool isMoving; //타일이 이동하는지의 여부
    public Vector2 direction; //이동방향
    public float movingSpeed; //타일이 이동하는 속도
    public float movingRange; //이동 범위

    private Vector2 originPos; //처음 배치 위치
    private int key; //이동 방향 전환 키

    public new void Start()
    {
        base.Start();

        isSpinning = false;
        isMoving = false;
        key = 1;
        direction = Vector2.ClampMagnitude(direction, 1);
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

        }
    }
}
