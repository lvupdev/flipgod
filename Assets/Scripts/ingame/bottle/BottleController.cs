using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
물병을 점프시키는 스크립트
*/
public class BottleController : MonoBehaviour
{
    public PadStrength padStrength;
    public PadDirection padDirection;
    public Rigidbody2D rb;                  // rigidbody component of bottle

    /*==========<variable about state of bottle>================================*/
    public bool isSuperPowerAvailabe;       // 물병에 기본 초능력을 사용할 수 있는가
    public bool isAct;                      // 물병이 콜라이더에 충돌하기 전인가
    public bool isStanding;                 // 물병이 현재 서 있는가
    // (New) public bool isStandingAtTheMoment
    public bool tensionGaugeUp;             // 콤보로 인한 텐션게이지 상승이 발생하여야 하는가
    public bool onFloor;                    // 물병이 바닥 위에 있는가
    public bool standingBySkill;            // 필살기에 의해 세워지는 중인가;
    public float Timeout;
    public static int combo = 0;                // 물병이 몇번째 콤보를 달성하였는가

    private float rotateSpeed; //회전속도
    private float zRotation; //NEW: 물병의 z회전축 값
    private int key; //물병의 회전 방향 결정 요소
    private float delta; //NEW: 물병이 구조물에 부딪히고 지난 시간
    private float destroyDelay; //NEW: 물병이 땅에 닿고 파괴되기까지의 딜레이 시간
    private float standingDelay; // 물병이 필살기에 의해 세워지기까지의 시간
    private BottleGenerator bottleGenerator;
    private SuperPowerController superPowerController;
    private PlayerImageController playerImageController;
    private GameObject player;
    private GameObject controllButtons;
    private BottleSelectController bottleSelectController;
    private TensionGaugeManager tensionGaugeManager;
    private bool padStrengthTouched; //힘 버튼이 한 번이라도 눌렸는가
    private bool padDirectionTouched; //힘 버튼이 한 번이라도 눌렸는가
    private Text comboText; //콤보 텍스트


    private TrajectoryLine trajectoryLine; //포물선 스크립트 분리

    SpriteRenderer transparent;

    void Start()
    {
        //오브젝트 받아오기
        rb = GetComponent<Rigidbody2D>();
        bottleGenerator = GameObject.Find("BottleManager").GetComponent<BottleGenerator>();
        superPowerController = GameObject.Find("Player").GetComponent<SuperPowerController>();
        player = GameObject.Find("Player");
        controllButtons = GameObject.Find("ControllButtons");
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        playerImageController = player.GetComponent<PlayerImageController>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        padDirection = GameObject.Find("Joystick").GetComponent<PadDirection>();
        trajectoryLine = GameObject.Find("Trajectory").GetComponent<TrajectoryLine>();
        transparent = GetComponent<SpriteRenderer>(); // 물병의 스프라이트 렌더러(투명도)
        tensionGaugeManager = GameObject.Find("Image_TensionGaugeBar").GetComponent<TensionGaugeManager>();
        comboText = GameObject.Find("Text_Combo").GetComponent<Text>();

        //값 초기화
        rb.gravityScale = 0;
        transform.position = playerImageController.GetBottlePosition();
        isSuperPowerAvailabe = false; //물병에 초능력을 적용할 수 있는지의 여부
        isStanding = false;
        onFloor = false;
        standingBySkill = false;
        rotateSpeed = 0.5f; //회전속도
        delta = 0;
        destroyDelay = 1;
        standingDelay = 2;
        padStrengthTouched = false;
        padDirectionTouched = false;
        trajectoryLine.Start();
        tensionGaugeUp = true;

    }
    void FixedUpdate()
    {
        if (padStrength.isTouch) padStrengthTouched = true;
        if (padDirection.isTouch) padDirectionTouched = true; //오타 수정
        if ((padStrength.isTouch || padDirectionTouched) && (!isSuperPowerAvailabe) && gameObject.CompareTag("isActBottle"))
        // 방향 패드만 눌렸을 때 기본 힘으로 포물선 그리기, 후에 힘버튼으로 포물선 조정
        {
            trajectoryLine.Draw(padStrengthTouched, padDirection.direction, padStrength.totalStrength);
            transform.position = playerImageController.GetBottlePosition(); // 물병 위치 갱신
        }

        if (gameObject.CompareTag("unActBottle"))
        {
            Vector2 distance = gameObject.transform.position - playerImageController.GetBottlePosition();
            zRotation = gameObject.transform.eulerAngles.z;
            delta += Time.fixedDeltaTime;
            if (distance.magnitude < 2) gameObject.SetActive(false); //던져진 물병이 물병 생성 위치와 너무 가까이 있으면 비활성화

            if ((delta < 0.11f) && ((zRotation > 340) || (zRotation < 20))) //NEW: 처음 충돌했을 때 각도가 30도 이하 또는 330도 이상이면 0.1초동안
            {
                rb.centerOfMass = new Vector3(0, -0.7f, 0); //물병의 무게 중심
                rb.drag = 10f;
                rb.angularDrag = 30f;
            }
            else if (standingBySkill) //필살기 발동에 의해 물병이 세워짐
            {
                standingDelay -= Time.fixedDeltaTime;
                rb.WakeUp();
                rb.centerOfMass = new Vector3(0, -1f,0); //물병의 무게 중심
                
                if (standingDelay < 0)
                {
                    standingDelay = 2;
                    standingBySkill = false;
                    this.transform.GetChild(0).gameObject.SetActive(false);
                }
                
                Debug.Log(rb.centerOfMass);
            }
            else
            {
                rb.centerOfMass = new Vector3(0, (-0.4f / (180f * 180f)) * (zRotation - 180) * (zRotation - 180) + 0.2f, 0); //NEW: 1초 후에 물병의 무게 중심이 각도에 따라 변함
                rb.drag = 0;
                rb.angularDrag = 0.05f;
            }


            // 세워져 있는지의 여부 수정 및 텐션게이지 상승
            if (((delta > 1f) && !((zRotation > 340) || (zRotation < 20))) || onFloor)
            {
                isStanding = false;
                tensionGaugeUp = false;
                if (delta < 1.5f)
                {
                    combo = 0;
                    comboText.text = "";
                }
            }
            else if ((delta > 1f) && (rb.angularVelocity == 0) && ((zRotation > 340) || (zRotation < 20)))
            {
                isStanding = true;
                if (tensionGaugeUp)
                {
                    combo++;
                    tensionGaugeManager.IncreaseTensionGauge(2, combo); //10% * 콤보수 만큼 상승
                    tensionGaugeUp = false;
                }
            }
        }

        if (onFloor) //NEW: 땅바닥에 닿았을 때 물병 파괴
        {
            Color c = transparent.material.color; // RGBA 중 A 가 투명도

            destroyDelay -= Time.fixedDeltaTime;
            if (destroyDelay < 0)
            {
                c.a -= 0.06f; // 투명도 0.01씩 낮추기
                transparent.material.color = c;
                if (c.a < 0) Destroy(gameObject);

            }
        }


        if (gameObject.transform.position.y < -8) // 물병이 화면 밖으로 날아갔을 때
        {
            if (gameObject.CompareTag("unActBottle")) Destroy(gameObject); // 어딘가 부딪히고 화면 밖으로 튕겨나갔을 때
            else
            {
                gameObject.tag = "unActBottle";//태그가 사라짐
                bottleGenerator.GenerateBottle();//물병 생성
                bottleSelectController.ReselectBottle(); //물병 재선택
                Destroy(gameObject); //해당 물병 파괴
            }
        }
    }

    public void Jump()
    {
        if (padDirection.direction.x >= 0) key = 1;
        if (padDirection.direction.x < 0) key = -1;

        isSuperPowerAvailabe = true;

        rb.gravityScale = 1;

        player.GetComponent<MembraneCreator>().presentStrength = padStrength.totalStrength; //물병에 현재 가해진 힘 전달

        //뛰면서 회전
        rb.velocity = padDirection.direction * padStrength.totalStrength;
        rb.AddTorque(key * rotateSpeed, ForceMode2D.Impulse);

        controllButtons.SetActive(false); // 화면을 깔끔하게 하기 위해 컨트롤 UI 버튼들을 일시적으로 전부 제거

        trajectoryLine.Delete();
        if (playerImageController.GetPlayingChr() == 2)
        {
            transform.Find("FreezeRange").gameObject.SetActive(true);
        }
        tensionGaugeManager.IncreaseTensionGauge(1, 1); //텐션 게이지 10% 상승
    }
}