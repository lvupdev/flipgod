using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
/*
물병을 점프시키는 스크립트
*/
public class BottleController : MonoBehaviour
{
    public PadStrength padStrength;
    public PadDirection padDirection;
    public Rigidbody2D rb { get; set; }                  // rigidbody component of bottle
    public static BottleController ControllingBottle { get; private set; }
    public static List<BottleController> BottleControllerList { get { return bottleControllerList; } }

    /*==========<variable about state of bottle>================================*/
    public bool isSuperPowerAvailabe { get; set; }     // 물병에 기본 초능력을 사용할 수 있는가
    public bool IsStanding { get; private set; }                // 물병이 현재 서 있는가

    public bool isAct;                      // 물병이 콜라이더에 충돌하기 전인가
    // (New) public bool isStandingAtTheMoment
    public bool tensionGaugeUp;             // 콤보로 인한 텐션게이지 상승이 발생하여야 하는가
    public bool onFloor;                    // 물병이 바닥 위에 있는가
    public bool standingBySkill;            // 필살기에 의해 세워지는 중인가;
    public float Timeout;
    public bool standBottle; //물병을 세울건지의 여부

    private static List<BottleController> bottleControllerList = new List<BottleController>();
    private static int combo = 0;                // 물병이 몇번째 콤보를 달성하였는가

    private float rotateSpeed; //회전속도
    private float zRotation; //NEW: 물병의 z회전축 값
    private int key; //물병의 회전 방향 결정 요소
    private float delta; //NEW: 물병이 구조물에 부딪히고 지난 시간
    private float destroyDelay; //NEW: 물병이 땅에 닿고 파괴되기까지의 딜레이 시간
    private float standingDelay; // 물병이 필살기에 의해 세워지기까지의 시간
    private BottleGenerator bottleGenerator;
    private PlayerImageController playerImageController;
    private GameObject player;
    private BottleSelectController bottleSelectController;
    private TensionGaugeManager tensionGaugeManager;
    private ControllButtonsUIManager controllButtonsUIManager;
    private UsefullOperation usefullOperation;
    private ScreenEffectController screenEffectController;
    private bool padStrengthTouched; //힘 버튼이 한 번이라도 눌렸는가
    private bool padDirectionTouched; //힘 버튼이 한 번이라도 눌렸는가
    private bool isDestroying; //물병이 현재 파괴되고있는 중인지의 여부


    private TrajectoryLine trajectoryLine; //포물선 스크립트 분리
    private SpriteRenderer transparent;

    private LeftBottom leftBottom;
    private RightBottom rightBottom;

    private void Awake()
    {
        Assert.IsTrue(null == ControllingBottle, "이미 조작 중인 물병이 있는데 새로운 물병이 생성되면 안 됩니다.");
        
        ControllingBottle = this;
        bottleControllerList.Add(this);
    }

    private void Start()
    {
        //오브젝트 받아오기
        rb = GetComponent<Rigidbody2D>();
        bottleGenerator = GameObject.Find("BottleManager").GetComponent<BottleGenerator>();
        player = GameObject.Find("Player");
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        playerImageController = player.GetComponent<PlayerImageController>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        padDirection = GameObject.Find("Joystick").GetComponent<PadDirection>();
        trajectoryLine = GameObject.Find("Trajectory").GetComponent<TrajectoryLine>();
        transparent = GetComponent<SpriteRenderer>(); // 물병의 스프라이트 렌더러(투명도)
        tensionGaugeManager = GameObject.Find("Image_TensionGaugeBar").GetComponent<TensionGaugeManager>();
        controllButtonsUIManager = GameObject.Find("UIManager").GetComponent<ControllButtonsUIManager>();
        usefullOperation = GameObject.Find("GameResource").GetComponent<UsefullOperation>();
        screenEffectController = GameObject.Find("Main Camera").GetComponent<ScreenEffectController>();
        leftBottom = transform.Find("BottleBottom").Find("LeftBottom").GetComponent<LeftBottom>();
        rightBottom = transform.Find("BottleBottom").Find("RightBottom").GetComponent<RightBottom>();

        //값 초기화
        rb.gravityScale = 0;
        transform.position = playerImageController.getBottlePosition();
        isSuperPowerAvailabe = false; //물병에 초능력을 적용할 수 있는지의 여부
        IsStanding = false;
        onFloor = false;
        standingBySkill = false;
        rotateSpeed = 0.8f; //회전속도
        delta = 0;
        destroyDelay = 1;
        standingDelay = 2;
        padStrengthTouched = false;
        padDirectionTouched = false;
        tensionGaugeUp = true;
        isDestroying = false;
        standBottle = false;
    }
    private void Update()
    {
        if (padStrength.isTouch) padStrengthTouched = true;
        if (padDirection.getIsTouch()) padDirectionTouched = true; //오타 수정
        if ((padStrength.isTouch || padDirectionTouched) && (!isSuperPowerAvailabe) && (this == ControllingBottle))
        // 방향 패드만 눌렸을 때 기본 힘으로 포물선 그리기, 후에 힘버튼으로 포물선 조정
        {
            trajectoryLine.Draw(padStrengthTouched, padDirection.getDirection(), padStrength.totalStrength);
            transform.position = playerImageController.getBottlePosition(); // 물병 위치 갱신
        }

        if ( this != ControllingBottle )
        {
            Vector2 distance = gameObject.transform.position - playerImageController.getBottlePosition();
            zRotation = gameObject.transform.eulerAngles.z;
            delta += Time.deltaTime;

            if (distance.magnitude < 2) gameObject.SetActive(false); //던져진 물병이 물병 생성 위치와 너무 가까이 있으면 비활성화
            
            if (standingBySkill) //필살기 발동에 의해 물병이 세워짐
            {
                standingDelay -= Time.deltaTime;
                rb.WakeUp();
                rb.centerOfMass = new Vector3(0, -0.6f, 0);
                
                if (standingDelay < 0)
                {
                    standingDelay = 2;
                    standingBySkill = false;
                    rb.centerOfMass = Vector3.zero;
                    if(!isDestroying) usefullOperation.FadeOut(1, this.transform.GetChild(0).GetComponent<SpriteRenderer>()); //파괴 도중에 실행되면 오류 발생
                }
            }

            if (standBottle) //물병이 세워지는 경우
            {
                if (delta < 0.6f && transform.eulerAngles.z < 350 && transform.eulerAngles.z > 10 && leftBottom.isLeftBottomTouched && rightBottom.isRightBottomTouched)
                {
                    if (zRotation > 350) transform.Rotate(new Vector3(0, 0, 180 * Time.smoothDeltaTime), Space.World); //물병의 z축 값을 360으로 수렴
                    else transform.Rotate(new Vector3(0, 0, -180 * Time.smoothDeltaTime), Space.World); //물병의 z축 값을 0으로 수렴
                }
                else if (delta < 0.9f && leftBottom.isLeftBottomTouched && rightBottom.isRightBottomTouched)
                {
                    if (transform.eulerAngles != Vector3.zero)
                    {
                        transform.rotation = Quaternion.Euler(Vector3.zero); //물병의 z축 값을 0으로 설정
                        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    }
                    rb.AddForce(new Vector2(0, -1000 * Time.smoothDeltaTime)); // 물병이 튀어오르지 않도록 아래로 힘 작용
                }
                else
                {
                    standBottle = false;
                    rb.constraints = RigidbodyConstraints2D.None; //z축 회전고정 해제
                }
            }

            // 세워져 있는지의 여부 수정 및 텐션게이지 상승
            if (((delta > 1.49f) && !((zRotation > 340) || (zRotation < 20))) || onFloor)
            {
                IsStanding = false;
                tensionGaugeUp = false;
                if (delta < 1.5f)
                {
                    combo = 0;
                    tensionGaugeManager.comboText = "";
                }
            }
            else if ((delta > 1f) && (Mathf.Abs(rb.angularVelocity) < 0.1f) && ((zRotation > 340) || (zRotation < 20)))
            {
                IsStanding = true;
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
            destroyDelay -= Time.deltaTime;
            if (destroyDelay < 0)
            {
                usefullOperation.FadeOut(1, this.transform.GetChild(0).GetComponent<SpriteRenderer>());
                usefullOperation.FadeOut(2, transparent);
            }
            isDestroying = true;
        }


        if (gameObject.transform.position.y < -10 && !isDestroying) // 물병이 화면 밖으로 날아갔을 때
        {
            if (this != ControllingBottle) Destroy(gameObject); // 어딘가 부딪히고 화면 밖으로 튕겨나갔을 때
            else
            {
                if (Time.timeScale != 1) //염력 사용하다가 화면 밖으로 날아간 경우
                {
                    Time.timeScale = 1;
                    Time.fixedDeltaTime = 0.02f * Time.timeScale;
                    screenEffectController.shadowEffect.enabled = false;
                    screenEffectController.screenEffectNum = 1;
                }
                ControllingBottle = null;
                bottleSelectController.bottleSelected = false;
                bottleGenerator.GenerateBottleWithDelay(0.75f);//물병 생성
                bottleSelectController.ReselectBottleWithDelay(0.75f); //물병 재선택
                Destroy(gameObject); //해당 물병 파괴
            }
        }
    }

    public void Jump()
    {
        if (padDirection.getDirection().x >= 0) key = 1;
        if (padDirection.getDirection().x < 0) key = -1;

        isSuperPowerAvailabe = true;

        rb.gravityScale = 2f;

        player.GetComponent<MembraneCreator>().presentStrength = padStrength.totalStrength; //물병에 현재 가해진 힘 전달
        MembraneUsingSkillEffect.presentStrength = padStrength.totalStrength; //물병에 현재 가해진 힘 전달

        //뛰면서 회전
        rb.linearVelocity = padDirection.getDirection() * padStrength.totalStrength;
        rb.AddTorque(key * rotateSpeed, ForceMode2D.Impulse);

        playerImageController.ChangePlayerImage(1); //던지는 동작으로 스프라이트 교체

        controllButtonsUIManager.setHideButtons(true, 0); // 화면을 깔끔하게 하기 위해 컨트롤 UI 버튼들을 일시적으로 전부 숨김

        trajectoryLine.Delete();
        if (playerImageController.getPlayingChr() == 2)
        {
            usefullOperation.FadeIn(transform.Find("FreezeRange").gameObject.GetComponent<SpriteRenderer>());
        }
        tensionGaugeManager.IncreaseTensionGauge(1, 1); //텐션 게이지 10% 상승
    }

    public static void ClearControllingBottle(GameObject thisBottle)
    {
        Assert.IsTrue(thisBottle == ControllingBottle.gameObject, "같은 물병 오브젝트 안에 있는 컴포넌트만 Controlling Bottle을 초기화할 수 있습니다.");

        ControllingBottle = null;
    }

    public static void ClearBottleControllerList()
    {
        bottleControllerList.Clear();
    }

	public static int CountStandingBottle()
	{
		int count = 0;
		foreach (var bottleController in bottleControllerList)
		{
            if ( bottleController == ControllingBottle )
                continue;

			if (bottleController.IsStanding == true)
				count++;
		}
		return count;
	}

	public bool DestroyBottle()
	{
		bool isDestroyed = false;

		if (bottleControllerList.Contains(this))
		{
			bottleControllerList.Remove(this);
		}
        Destroy(gameObject);

		return isDestroyed;
	}
}