using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bird : MonoBehaviour
{
    public Transform trans; //초기위치
    public float moveSpeed = 2;  //움직이는 속도
    private bool damaged = false;//부딪혔는지 확인할 변수

    // 땅에 닿았을 때 파괴
    private Rigidbody2D rb;// 리지드바디2d
    private GameObject bird;// 새
    private Vector2 Vecx1; // 왔다 갔다 왼쪽 //움직일 좌표
    private Vector2 Vecx2; //왔다 갔다 오른쪽 //움직일 좌표
    private int direction = 1;

    void Start()
    {
        this.bird = GameObject.FindGameObjectWithTag("Bird"); //게임오브젝트를 찾는다.
        trans = this.bird.transform; // 위치를 받아온다
        rb = bird.GetComponent<Rigidbody2D>(); // 리지드바디를 받아온다
       Vecx1.x = (trans.position.x - 4f); // 왼쪽으로 이동할 범위
       Vecx2.x = (trans.position.x + 4f); // 오른쪽으로 이동할 범위
    }

    void Update()
    {

        if(damaged == false) //안부딪혔으면 계속 날라다녀야지
        {
            Birdmove(Vecx1, Vecx2, moveSpeed);
        }
        
    }

    void Birdcrush()// 부딪혔으면 떨어져야지
    {
        if (damaged)
        {
            rb.gravityScale = 1;
        }

    }

    void Birdmove(Vector2 Vecx1, Vector2 Vecx2, float moveSpeed) // 새의 이동함수
    {
        Vector2 currPos = gameObject.transform.position; // 현재의 위치를 계속 받아온다.
        gameObject.transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, 0); // 시간에 따른 위치 이동

        if (currPos.x <= Vecx1.x)   //오른쪽 가면 돌아와야지
        {
            direction = 1;
        }
        if (currPos.x >= Vecx2.x) // 왼쪽가면 돌아와야지
        {
            direction = -1;
        }
    }

    void OnTriggerEnter2D(Collider2D other) //충돌시 불리는 함수
    {
        if (other.tag == "isActBottle") //부딪히면 떨어져야지
        {
            damaged = true;
            Birdcrush();
        }



    }


}
