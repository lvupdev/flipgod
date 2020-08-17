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
    public GameObject IndustrialLine; //톱니바퀴가 놓일 레인
    public float speed; //이동 속도 
    public bool horizontal; //수평 이동하는지의 여부
    private float rotation; // industrial thin line의 각도(라디안)
    private float angle; // tan값



    public bool vertical; //수직 이동하는지의 여부
    private RectTransform rect_Line;  //톱니바퀴 레인의 rect transform
    private Vector3 direction; //톱니바퀴가 이동하는 방향
    private float angle; //톱니바퀴 레인의 각도
    private float moveRange; //톱니바퀴가 이동할수 있는 범위
    private float distance; //톱니바퀴와 레인 중심 사이의 수평 거리
    private int key; //이동 방향을 정하는 키값

    private new void Start()
    {
        base.Start(); //부모 클래스의 Start도 실행

        rect_Line = IndustrialLine.transform.GetComponent<RectTransform>();

        moveRange = rect_Line.rect.width / 2;
        rotation = rect_Line.eulerAngles.z;
        if (rotation >= 45) // -45~ 45 각도를 위함 
        {
            rotation = rotation - 90;
        }
        angle = Mathf.Deg2Rad * rotation;//각도에 따른 기울기값 + 쿼너티언 각도를 오일러각도로 
        angle = Mathf.Tan(rotation);


        if (vertical) //수직으로 이동하는 경우
        {
            moveRange = rect_Line.rect.width / 2;
            distance = transform.position.y - IndustrialLine.transform.position.y;
            key = (distance > 0) ? 1 : -1; //초기 위치에 따라 키값 결정
        }
        else //이외의 경우
        {
            distance = transform.position.x - IndustrialLine.transform.position.x;
            key = (distance > 0) ? 1 : -1; //초기 위치에 따라 키값 결정
            angle = rect_Line.rotation.eulerAngles.z;

            angle *= (Mathf.PI / 180.0f);
            moveRange = (rect_Line.rect.width / 2) * Mathf.Cos(angle);
            direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        }
    }
    void Update()
    {
        ThawDynamicStructure(isFreezed);
        if (!isFreezed) MoveDynamicStructure();
    }

    public void MoveDynamicStructure()
    {
        transform.Rotate(new Vector3(0, 0, -360 * Time.deltaTime)); //1초에 360도씩 회전


        transform.position += new Vector3(speed * key * Time.deltaTime, speed * key * Time.deltaTime * angle, 0); //1초에 speed만큼 이동
        distance = transform.position.x - IndustrialLine.transform.position.x;


        /*
        if (horizontal)
=======
        if (vertical)
>>>>>>> 2b407904943bcc5767f47f9d1764ab1211d5290e
=======
        if (vertical)
>>>>>>> 2b407904943bcc5767f47f9d1764ab1211d5290e
        {
            transform.position += new Vector3(0, speed * key * Time.deltaTime, 0); //1초에 speed만큼 이동

            distance = transform.position.y - IndustrialLine.transform.position.y;
        }
        else
        {
            transform.position += direction * speed * key * Time.deltaTime; //1초에 speed만큼 이동

            distance = transform.position.x - IndustrialLine.transform.position.x;
        }
        */

        //이동 범위에서 벗어났을 경우, 두번째 조건은 한 번 key값을 바꾸고 이동 범위 내로 들어오기 전에 다시 키값이 바뀌는 걸 방지
        if ((Math.Abs(distance) > moveRange) && (distance * key > 0))
        {
            key = -key; //키값 변경
        }
    }
}
