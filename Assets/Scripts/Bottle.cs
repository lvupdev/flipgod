using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
물병에 작용되는 함수
*/
public class Bottle : MonoBehaviour
{
    public PadStrength padStrength;
    public PadDirection padDirection;

    private Rigidbody2D rb; //물병의 rigidbody 속성

    public float rotateSpeed; //회전속도
    private float strengthFactor = 10; //던지는 힘. 초기값이 있어야 포물선이 있어야


    //포물선
    private Vector2 initPos;
    private Vector2 endPos;
    public GameObject trajectoryDot;
    private GameObject[] trajectoryDots;
    public int trajectoryNumber = 14; //포물선 점 개수


    //어딘가에 부딪혔을때
    void OnCollisionEnter2D(Collision2D col)
    {
    }

    void Start()
    {
        //오브젝트 받아오기
        rb = GetComponent<Rigidbody2D>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        padDirection = GameObject.Find("Joystick").GetComponent<PadDirection>();

        //값 초기화
        rb.gravityScale = 0;
        rotateSpeed = 5.0f;

        //포물선
        initPos = gameObject.transform.position;
        trajectoryDots = new GameObject[trajectoryNumber];
        for (int i = 0; i < trajectoryNumber; i++)
        {
            trajectoryDots[i] = Instantiate(trajectoryDot, gameObject.transform);
        }
    }

    void Update()
    {
        if (padDirection.isTouch) //방향패드가 눌려있을 동안
        {
            // 포물선 그리기
            endPos = initPos + padDirection.direction;
            for (int i = 0; i < trajectoryNumber; i++)
            {
                trajectoryDots[i].transform.position = calculatePosition(i * 0.1f);
            }
        }
        
    }

    public void Jump() 
    {
        strengthFactor = padStrength.strength_time / 5; //5는 힘이 너무 세서 나누어주는 값
        Debug.Log(strengthFactor); //힘 출력

        rb.gravityScale = 1;

        //뛰면서 회전
        rb.AddForce(padDirection.direction * strengthFactor, ForceMode2D.Impulse);
        rb.AddTorque(rotateSpeed, ForceMode2D.Impulse);

        //포물선 삭제
        for (int i = 0; i < trajectoryNumber; i++)
        {
            Destroy(trajectoryDots[i]);
        }
    }


    //포물선 그리기
    private Vector2 calculatePosition(float elapsedTime)
    {
        return endPos + //X0
                new Vector2(padDirection.direction.x * strengthFactor, padDirection.direction.y * strengthFactor) * elapsedTime + //ut
                0.5f * Physics2D.gravity * elapsedTime * elapsedTime;
    }

}
