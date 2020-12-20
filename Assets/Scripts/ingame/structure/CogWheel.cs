using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/*
 * 톱니 바퀴 움직임을 관리하는 스크립트. 수직으로 이동하는 경우에 vertical을 체크해야 한다.
 * 배경에 놓이는 industrialLine은 반드시 -90도 ~ 90도 사이로 회전값을 가져야한다.
 */
public class CogWheel : Structure
{
    public float speed; //이동 속도 

    private GameObject industrialLine;
    private RectTransform rect_Line;  //톱니바퀴 레인의 rect transform
    private float moveRange; //톱니바퀴가 이동할수 있는 범위
    private float distance; //톱니바퀴와 레인 중심 사이의 수평 거리
    private int key; //이동 방향을 정하는 키값

    private new void Start()
    {
        base.Start(); //부모 클래스의 Start도 실행

        industrialLine = transform.parent.gameObject;
        rect_Line = industrialLine.GetComponent<RectTransform>();
        distance = transform.localPosition.x;
        key = (distance > 0) ? 1 : -1; //초기 위치에 따라 키값 결정
        moveRange = rect_Line.rect.width / 2;
    }
    void Update()
    {
        ThawDynamicStructure(isFreezed);
        if (!isFreezed) MoveDynamicStructure();
    }

    public void MoveDynamicStructure()
    {
        transform.Rotate(new Vector3(0, 0, -360 * Time.deltaTime)); //1초에 360도씩 회전

        transform.localPosition += new Vector3(speed * key * Time.deltaTime, 0, 0); //1초에 speed만큼 이동

        distance = transform.localPosition.x;

        //이동 범위에서 벗어났을 경우, 두번째 조건은 한 번 key값을 바꾸고 이동 범위 내로 들어오기 전에 다시 키값이 바뀌는 걸 방지
        if ((Math.Abs(distance) > moveRange) && (distance * key > 0))
        {
            key = -key; //키값 변경
        }
    }
}
