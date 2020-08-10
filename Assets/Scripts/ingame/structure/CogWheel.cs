using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogWheel : Structure
{
    public GameObject IndustrialLine; //톱니바퀴가 놓일 레인
    
    private RectTransform rect_Line;  //톱니바퀴 레인의 rect transform
    private float moveRange; //톱니바퀴가 이동할수 있는 범위
    private float distance; //톱니바퀴와 레인 중심 사이의 거리
    private int key; //이동 방향을 정하는 키값

    private new void Start()
    {
        base.Start(); //부모 클래스의 Start도 실행

        rect_Line = IndustrialLine.transform.GetComponent<RectTransform>();
        moveRange = rect_Line.rect.width / 2;
        distance = transform.position.x - IndustrialLine.transform.position.x;
        key = (distance > 0) ? 1 :-1; //초기 위치에 따라 키값 결정
    }
    void Update()
    {
        ThawDynamicStructure(isFreezed);
        if (!isFreezed) MoveDynamicStructure();
    }

    public void MoveDynamicStructure()
    {
        transform.Rotate(new Vector3(0, 0, -360 * Time.deltaTime)); //1초에 360도씩 회전

        transform.position += new Vector3(2* key * Time.deltaTime, 0, 0); //1초에 2만큼 이동

        distance = transform.position.x - IndustrialLine.transform.position.x;

        //이동 범위에서 벗어났을 경우, 두번째 조건은 한 번 key값을 바꾸고 이동 범위 내로 들어오기 전에 다시 키값이 바뀌는 걸 방지
        if ((Math.Abs(distance) > moveRange) && (distance * key > 0))
        {
            key = -key; //키값 변경
        }
    }
}
