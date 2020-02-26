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
    private SuperPowerController superPowerController;
    private PlayerController playerController;
    private bool padStrengthTouched; //힘 버튼이 한 번이라도 눌렸는가
    private bool padDirectionTouched; //힘 버튼이 한 번이라도 눌렸는가

    //포물선
    public GameObject trajectoryDotPrefab;
    private GameObject[] trajectoryDots;
    private GameObject[] directionDots;
    private int trajectoryNumber = 13; //포물선 점 개수
    private int directionNumber = 5;
    private float normalStrength = 10.0f; //NEW: 포물선에 적용되는 기본 힘
    

    //어딘가에 부딪혔을때
    void OnCollisionEnter2D(Collision2D col)
    {
        this.isSuperPowerAvailabe = false; //더 이상 초능력을 적용할 수 없음
        if (gameObject.CompareTag("isActive")) 
        {
            gameObject.tag = "Untagged";//태그가 사라짐
            bottleGenerator.GenerateBottle();//물병 생성
            padStrength.ReselectBottle(); //물병 재선택
            superPowerController.ReselectBottle(); //물병 재선택
            playerController.ReselectBottle(); //물병 재선택
        }
    }

    void Start()
    {
        //오브젝트 받아오기
        rb = GetComponent<Rigidbody2D>();
        bottleGenerator = GameObject.Find("BottleGenerator").GetComponent<BottleGenerator>();
        superPowerController = GameObject.Find("SuperPower").GetComponent<SuperPowerController>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        padDirection = GameObject.Find("Joystick").GetComponent<PadDirection>();

        //값 초기화
        rb.gravityScale = 0;
        bottleGenerator.startPosition = gameObject.transform.position;// 처음 배치한 물병의 transform을 기준으로 새로운 물병 생성
        isSuperPowerAvailabe = false; //물병에 초능력을 적용할 수 있는지의 여부
        rotateSpeed = 0.5f; //회전속도
        padStrengthTouched = false;
        padDirectionTouched = false;

       //포물선
        trajectoryDots = new GameObject[trajectoryNumber];
        for (int i = 0; i < trajectoryNumber; i++)
        {
            trajectoryDots[i] = Instantiate(trajectoryDotPrefab, gameObject.transform);
        }


        //NEW: 방향포물선
        directionDots = new GameObject[directionNumber];
        for (int i = 0; i < directionNumber; i++)
        {
            directionDots[i] = Instantiate(trajectoryDotPrefab, gameObject.transform);
        }
    }

    void Update()
    {
        if (padStrength.isTouch) padStrengthTouched = true;
        if (padDirection.isTouch) padDirectionTouched = true; //NEW: 오타 수정
        if ((padStrength.isTouch || padDirectionTouched) && (!isSuperPowerAvailabe) && gameObject.CompareTag("isActive"))
        // NEW: 방향 패드만 눌렸을 때 기본 힘으로 포물선 그리기, 후에 힘버튼으로 포물선 조정
        {
            // 포물선 그리기
            if (!padStrengthTouched)
            {
                for (int i = 0; i < directionNumber; i++)
                {
                    directionDots[i].transform.position = CalculatePosition(i * 0.1f);
                }
            }
            else //NEW: 방향 포물선 그리기
            {
                for (int i = 0; i < trajectoryNumber; i++)
                {
                    trajectoryDots[i].transform.position = CalculatePosition(i * 0.1f);
                }
            }
            
        }

        if (gameObject.CompareTag("Untagged")) //던져진 물병이 물병 생성 위치와 너무 가까이 있으면 비활성화
        {
            Vector2 distance = gameObject.transform.position - bottleGenerator.startPosition;
            if (distance.magnitude < 2) gameObject.SetActive(false);

        }

        if(gameObject.transform.position.y<-8) // 물병이 화면 밖으로 날아갔을 때
        {
            if (gameObject.CompareTag("Untagged")) Destroy(gameObject); // 어딘가 부딪히고 화면 밖으로 튕겨나갔을 때
            else
            {
                gameObject.tag = "Untagged";//태그가 사라짐
                bottleGenerator.GenerateBottle();//물병 생성
                padStrength.ReselectBottle(); //물병 재선택
                superPowerController.ReselectBottle(); //물병 재선택
                playerController.ReselectBottle(); //물병 재선택
                Destroy(gameObject); //해당 물병 파괴
            }
        }
    }

    public void Jump() 
    {
        if (padDirection.direction.x >= 0) key = 1;
        if (padDirection.direction.x < 0) key = -1;

        isSuperPowerAvailabe = true;
        Debug.Log(padStrength.totalStrength); //힘 출력

        rb.gravityScale = 1;

        superPowerController.presentStrength = padStrength.totalStrength; //물병에 현재 가해진 힘 전달

        //뛰면서 회전
        rb.velocity = padDirection.direction * padStrength.totalStrength;
        rb.AddTorque(key*rotateSpeed, ForceMode2D.Impulse);

        //포물선 삭제
        for (int i = 0; i < trajectoryNumber; i++)
        {
                Destroy(trajectoryDots[i]);
        }
        for (int i = 0; i < directionNumber; i++)
        {
            Destroy(directionDots[i]);
        }

    }

    private Vector2 CalculatePosition(float elapsedTime)
    {
        float strengthFactor;
        if (!padStrengthTouched) strengthFactor = normalStrength; 
        //NEW: 힘패드 터치가 안되어있을때. 방향 포물선에 적용
        else strengthFactor = padStrength.totalStrength;
        return Physics2D.gravity * elapsedTime * elapsedTime * 0.5f +
                   padDirection.direction * strengthFactor * elapsedTime + new Vector2(transform.position.x, transform.position.y);
    }
}
