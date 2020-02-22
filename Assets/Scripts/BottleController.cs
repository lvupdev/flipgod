using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
물병에 작용되는 함수
*/
public class BottleController : MonoBehaviour
{
    public PadStrength padStrength;
    public PadDirection padDirection;
    public Rigidbody2D rb; //물병의 rigidbody 속성
    public bool isSuperPowerAvailabe; //물병에 기본 초능력을 사용할 수 있는지의 여부
    public bool isStanding; //서 있는지의 여부

    private float rotateSpeed; //회전속도
    private int key; //물병의 회전 방향 결정 요소
    private BottleGenerator bottleGenerator;
    private GameDirector gameDirector;
    private bool padStrengthTouched; //힘 버튼이 한 번이라도 눌렸는가
    private bool padDirectionTouched; //힘 버튼이 한 번이라도 눌렸는가

    //포물선
    public GameObject trajectoryDotPrefab;
    private GameObject[] trajectoryDots;
    public int trajectoryNumber = 10; //포물선 점 개수
    

    //어딘가에 부딪혔을때
    void OnCollisionEnter2D(Collision2D col)
    {
        this.isSuperPowerAvailabe = false; //더 이상 초능력을 적용할 수 없음
        if (gameObject.CompareTag("isActive")) 
        {
            gameObject.tag = "Untagged";//태그가 사라짐
            bottleGenerator.GenerateBottle();//물병 생성
            padStrength.ReselectBottle(); //물병 재선택
            gameDirector.PlayerReselectBottle();
        }
    }

    void Start()
    {
        //오브젝트 받아오기
        rb = GetComponent<Rigidbody2D>();
        bottleGenerator = GameObject.Find("BottleGenerator").GetComponent<BottleGenerator>();
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        padDirection = GameObject.Find("Joystick").GetComponent<PadDirection>();
        bottleGenerator = GameObject.Find("BottleGenerator").GetComponent<BottleGenerator>();

        //값 초기화
        rb.gravityScale = 0;
        bottleGenerator.startPosition = gameObject.transform.position;// 처음 배치한 물병의 transform을 기준으로 새로운 물병 생성
        isSuperPowerAvailabe = false; //물병에 초능력을 적용할 수 있는지의 여부
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

    void Update()
    {
        if (padStrength.isTouch) padStrengthTouched = true;
        if (padStrength.isTouch) padDirectionTouched = true;
        if (padDirectionTouched && padStrengthTouched && (!isSuperPowerAvailabe) && gameObject.CompareTag("isActive")) //방향패드와 힘 버튼이 한 번 이상 눌렸을 떄
        {
            // 포물선 그리기
            for (int i = 0; i < trajectoryNumber; i++)
            {
                trajectoryDots[i].transform.position = CalculatePosition(i * 0.1f);
            }
        }

        if (gameObject.CompareTag("Untagged")) //던져진 물병이 물병 생성 위치와 너무 가까이 있으면 비활성화
        {
            Vector2 distance = gameObject.transform.position - bottleGenerator.startPosition;
            if (distance.magnitude < 2) gameObject.SetActive(false);

        }
    }

    public void Jump() 
    {
        if (padDirection.direction.x >= 0) key = 1;
        if (padDirection.direction.x < 0) key = -1;

        isSuperPowerAvailabe = true;
        Debug.Log(padStrength.totalStrength); //힘 출력

        rb.gravityScale = 1;

        //뛰면서 회전
        rb.velocity = padDirection.direction * padStrength.totalStrength;
        rb.AddTorque(key*rotateSpeed, ForceMode2D.Impulse);

        //포물선 삭제
        for (int i = 0; i < trajectoryNumber; i++)
        {
                Destroy(trajectoryDots[i]);
        }
    
    }

    private Vector2 CalculatePosition(float elapsedTime)
    {
        return Physics2D.gravity * elapsedTime * elapsedTime * 0.5f +
                   padDirection.direction * padStrength.totalStrength * elapsedTime + new Vector2(transform.position.x, transform.position.y);
    }


}
