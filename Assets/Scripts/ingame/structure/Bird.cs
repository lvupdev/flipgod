using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ingame;


public class Bird : Structure
{

    private Vector2 minVec2; // 왔다 갔다 왼쪽 //움직일 좌표
    private Vector2 maxVec2; //왔다 갔다 오른쪽 //움직일 좌표
    public float moveSpeed;  //움직이는 속도
    Vector2 currPos;  // 현재의 위치를 계속 받아온다.
    
    private bool damaged = false;//부딪혔는지 확인할 변수
    private Rigidbody2D rb;

    // 땅에 닿았을 때 파괴
    private int direction = 1;

    void Awake()
    {
       currPos = gameObject.transform.position;
       rb = gameObject.GetComponent<Rigidbody2D>(); // 리지드바디를 받아온다
       minVec2.x = (currPos.x - 4f); // 왼쪽으로 이동할 범위
       maxVec2.x = (currPos.x + 4f); // 오른쪽으로 이동할 범위
    }

    void Update()
    {
        currPos = gameObject.transform.position;
        ThawDynamicStructure(isFreezed);
        if ((damaged == false) || isFreezed) //안부딪혔으면 계속 날라다녀야지
        {
            MoveDynamicStructure();
        }
    }

    public void setValue(float moveSpeed) //스크립트 필드의 값을 설정하는 메소드.
    {
        this.moveSpeed = moveSpeed;
    }

    public void MoveDynamicStructure() // 새의 이동함수
    {
        gameObject.transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, 0); // 시간에 따른 위치 이동

        if (currPos.x <= minVec2.x)   //오른쪽 가면 돌아와야지
        {
            direction = 1;
        }
        if (currPos.x >= maxVec2.x) // 왼쪽가면 돌아와야지
        {
            direction = -1;
        }
    }

    void OnTriggerEnter2D(Collider2D other) //충돌시 불리는 함수
    {
        if (other.tag == "isActBottle") //부딪히면 떨어져야지
        {
            damaged = true;
            rb.gravityScale = 1;
        }
    }


}
