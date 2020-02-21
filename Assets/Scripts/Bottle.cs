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
    public bool isLaunched; //물병이 던져져서 날아가고 있는지의 여부, 어딘가에 부딪히기 전으로 한하여 공중에 있는 지의 여부
    public bool isActive; //플레이어가 필살기를 쓰지 않고 조작할 수 있는지의 여부

    private float rotateSpeed; //회전속도
    private Rigidbody2D rb; //물병의 rigidbody 속성
    private bool padStrengthTouched; //힘 버튼이 한 번이라도 눌렸는가
    private bool padDirectionTouched; //힘 버튼이 한 번이라도 눌렸는가

    //포물선
    public GameObject trajectoryDotPrefab;
    private GameObject[] trajectoryDots;
    public int trajectoryNumber = 8; //포물선 점 개수
    

    //어딘가에 부딪혔을때
    void OnCollisionEnter2D(Collision2D col)
    {
        this.isActive = false;
        this.isLaunched = false;
    }

    void Start()
    {
        //오브젝트 받아오기
        rb = GetComponent<Rigidbody2D>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        padDirection = GameObject.Find("Joystick").GetComponent<PadDirection>();

        //값 초기화
        rb.gravityScale = 0;
        isLaunched = false; //물병이 던져졌는지의 여부
        isActive = true; //플레이어가 필살기를 쓰지 않고 조작할 수 있는지의 여부
        rotateSpeed = 2.0f; //회전속도
        padStrengthTouched = false;
        padDirectionTouched = false;

       //포물선
        trajectoryDots = new GameObject[trajectoryNumber];
        for (int i = 0; i < trajectoryNumber; i++)
        {
            trajectoryDots[i] = Instantiate(trajectoryDotPrefab, gameObject.transform);
        }
    }

    void FixedUpdate()
    {
        if (padStrength.isTouch) padStrengthTouched = true;
        if (padStrength.isTouch) padDirectionTouched = true;
        if (padDirectionTouched && padStrengthTouched && (!isLaunched) && isActive) //방향패드와 힘 버튼이 한 번 이상 눌렸을 떄
        {
            // 포물선 그리기
            for (int i = 0; i < trajectoryNumber; i++)
            {
                trajectoryDots[i].transform.position = CalculatePosition(i * 0.1f);
            }
        }

    }

    public void Jump() 
    {
        if (isActive)
        {
            isLaunched = true;
            Debug.Log(padStrength.totalStrength); //힘 출력

            rb.gravityScale = 1;

            //뛰면서 회전
            rb.velocity = padDirection.direction * padStrength.totalStrength;
            rb.AddTorque(rotateSpeed, ForceMode2D.Impulse);

            //포물선 삭제
            for (int i = 0; i < trajectoryNumber; i++)
            {
                Destroy(trajectoryDots[i]);
            }
        }
    }

    private Vector2 CalculatePosition(float elapsedTime)
    {
        return Physics2D.gravity * elapsedTime * elapsedTime * 0.5f +
                   padDirection.direction * padStrength.totalStrength * elapsedTime + new Vector2(transform.position.x, transform.position.y);
    }


}
