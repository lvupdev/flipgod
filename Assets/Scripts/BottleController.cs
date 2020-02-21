using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
물병에 작용되는 함수
*/
public class BottleController : MonoBehaviour
{
    public PadStrength padStrength;
    public PadDirection padDirection;
    public int addStrength = 7; //시간별로 더해지는 힘 값, 조정 가능
    
    //{get;set;}을 하면 코드 내에서 수정은 가능하나, 유니티에서 보여지지 않음
    public float totalStrength { get; set; }

    private float rotateSpeed; //회전속도
    private float delayTime = 1f; //힘 조절 버튼에서 손가락을 때고 물병이 던져지기까지의 딜레이 타임

    private Rigidbody2D rb; //물병의 rigidbody 속성
    private GameObject strengthGauge; //힘 게이지 스프라이트

    private bool padStrengthTouched; //힘 버튼이 한 번이라도 눌렸는가
    private bool padDirectionTouched; //방향 패드가 한 번이라도 눌렸는가
    private bool isThrowing = false; //캐릭터가 물병을 던지는 동작을 진행중인가의 여부
    private bool isSuperPowerAvailable; // 초능력의 영향을 받는지의 여부
    private bool isActive; //플레이어가 필살기를 쓰지 않고 조작할 수 있는지의 여부

    //포물선
    public GameObject trajectoryDotPrefab;
    private GameObject[] trajectoryDots;
    public int trajectoryNumber = 10; //포물선 점 개수
    

    //어딘가에 부딪혔을때
    void OnCollisionEnter2D(Collision2D col)
    {
        this.isActive = false;
        this.isSuperPowerAvailable = false;
    }

    void Start()
    {
        //오브젝트 받아오기
        this.rb = GetComponent<Rigidbody2D>();
        this.strengthGauge = GameObject.Find("StrengthGauge");
        this.padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        this.padDirection = GameObject.Find("Joystick").GetComponent<PadDirection>();

        //값 초기화
        rb.gravityScale = 0;
        isSuperPowerAvailable = false; //물병을 초능력으로 조작할 수 있는지의 여부
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
        if (padStrength.isTouch && (!isSuperPowerAvailable) && (!isThrowing) && isActive && (totalStrength < 2 * addStrength))//패드가 눌려있고 물병을 던지는 도중이 아니며 물병이 날아가는 도중이 아니면
        {

            totalStrength += addStrength * Time.fixedDeltaTime; // 매 초마다 일정한 힘을 더한다.
            this.strengthGauge.GetComponent<Image>().fillAmount += 1.0f / 2 * Time.fixedDeltaTime; // 매 초마다 힘 게이지가 1/2 씩 차오른다.
        }

        if (isThrowing || (totalStrength >= 2 * addStrength)) //패드에서 마우스를 뗐거나 힘 버튼을 2초 이상 눌렀을 때
        {
            delayTime -= Time.fixedDeltaTime; //딜레이 타임만큼 던지는 동작이 지연된다.

            if (delayTime <= 0)
            {
                strengthGauge.gameObject.SetActive(false); // 힘 게이지를 화면에서 제거한다.
                Jump();
                isThrowing = false;
                totalStrength = 0;
            }
        }

        if (padStrength.isTouch) padStrengthTouched = true;
        if (padStrength.isTouch) padDirectionTouched = true;
        if (padDirectionTouched && padStrengthTouched && (!isSuperPowerAvailable) && isActive) //방향패드와 힘 버튼이 한 번 이상 눌렸을 떄
        {
            // 포물선 그리기
            for (int i = 0; i < trajectoryNumber; i++)
            {
                trajectoryDots[i].transform.position = CalculatePosition(i * 0.1f);
            }
        }

    }

    public void StrengthButtonDown()
    {
        if ((!isSuperPowerAvailable) && (!isThrowing) && isActive)
        {
            delayTime = 1f; //딜레이 타임 초기화
            padStrength.isTouch = true;
            strengthGauge.gameObject.SetActive(true); //힘 게이지를 화면에 표시한다.
        }
    }

    public void StrengthButtonUp()
    {
        if (isActive)
        {
            padStrength.isTouch = false;
            if (isSuperPowerAvailable) isThrowing = false;
            else isThrowing = true;
        }
    }

    public void Jump() 
    {
        if (isActive)
        {
            isSuperPowerAvailable = true;
            Debug.Log(totalStrength); //힘 출력

            rb.gravityScale = 1;

            //뛰면서 회전
            rb.velocity = padDirection.direction * totalStrength;
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
                   padDirection.direction * totalStrength * elapsedTime + new Vector2(transform.position.x, transform.position.y);
    }
}
