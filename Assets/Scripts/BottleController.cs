using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
물병을 점프시키는 스크립트
*/
public class BottleController : MonoBehaviour
{
    public PadStrength padStrength;
    public PadDirection padDirection;
    public Rigidbody2D rb; //물병의 rigidbody 속성
    public bool isSuperPowerAvailabe; //물병에 기본 초능력을 사용할 수 있는지의 여부
    public bool isStanding; //서 있는지의 여부
    public bool onFloor; //바닥에 있는지의 여부

    private float rotateSpeed; //회전속도
    private float zRotation; //NEW: 물병의 z회전축 값
    private int key; //물병의 회전 방향 결정 요소
    private float delta; //NEW: 물병이 구조물에 부딪히고 지난 시간
    private float destroyDelay; //NEW: 물병이 땅에 닿고 파괴되기까지의 딜레이 시간
    private BottleGenerator bottleGenerator;
    private SuperPowerController superPowerController;
    private SuperPowerPanelController SPPController;
    private PlayerImageController playerImageController;
    private GameObject player;
    private bool padStrengthTouched; //힘 버튼이 한 번이라도 눌렸는가
    private bool padDirectionTouched; //힘 버튼이 한 번이라도 눌렸는가


    private TrajectoryLine trajectoryLine; //포물선 스크립트 분리

    void Start()
    {
        //오브젝트 받아오기
        rb = GetComponent<Rigidbody2D>();
        bottleGenerator = GameObject.Find("BottleGenerator").GetComponent<BottleGenerator>();
        superPowerController = GameObject.Find("Player").GetComponent<SuperPowerController>();
        SPPController = GameObject.Find("SuperPowerPanel").GetComponent<SuperPowerPanelController>();
        player = GameObject.Find("Player");
        playerImageController = player.GetComponent<PlayerImageController>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        padDirection = GameObject.Find("Joystick").GetComponent<PadDirection>();
        trajectoryLine = GameObject.Find("Trajectory").GetComponent<TrajectoryLine>();


        //값 초기화
        rb.gravityScale = 0;
        transform.position = playerImageController.GetBottlePosition();
        isSuperPowerAvailabe = false; //물병에 초능력을 적용할 수 있는지의 여부
        isStanding = false;
        onFloor = false;
        rotateSpeed = 0.5f; //회전속도
        delta = 0;
        destroyDelay = 1;
        padStrengthTouched = false;
        padDirectionTouched = false;
        trajectoryLine.Start();

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

            if ((delta < 0.11f) && ((zRotation > 340) || (zRotation < 20))) //NEW: 처음 충돌했을 때 각도가 30도 이하 또는 330도 이상이면 1초동안
            {
                rb.centerOfMass = new Vector3(0, -0.7f, 0); //물병의 무게 중심
                rb.drag = 10f;
                rb.angularDrag = 30f;
            }

            else
            {
                rb.centerOfMass = new Vector3(0, (-0.4f / (180f * 180f)) * (zRotation - 180) * (zRotation - 180) + 0.2f, 0); //NEW: 1초 후에 물병의 무게 중심이 각도에 따라 변함
                rb.drag = 0;
                rb.angularDrag = 0.05f;
            }


            // 세워져 있는지의 여부 수정
            if ((delta > 2f) && !((zRotation > 330) || (zRotation < 30)))
                isStanding = false;
            else if ((delta > 2f) && ((zRotation > 330) || (zRotation < 30)))
                isStanding = true;
        }

        if(onFloor) //NEW: 땅바닥에 닿았을 때 물병 파괴
        {
            destroyDelay -= Time.deltaTime;
            if (destroyDelay < 0) Destroy(gameObject);
        }


        if(gameObject.transform.position.y<-8) // 물병이 화면 밖으로 날아갔을 때
        {
            if (gameObject.CompareTag("unActBottle")) Destroy(gameObject); // 어딘가 부딪히고 화면 밖으로 튕겨나갔을 때
            else
            {
                gameObject.tag = "unActBottle";//태그가 사라짐
                bottleGenerator.GenerateBottle();//물병 생성
                padStrength.ReselectBottle(); //물병 재선택
                playerImageController.ReselectBottle(); //물병 재선택
                SPPController.ReselectBottle(); //물병 재선택
                switch (playerImageController.playingChr)
                {
                    case 0:
                        player.transform.GetChild(0).GetComponent<PsychokinesisController>().ReselectBottle();
                        break;
                    case 1:
                        player.transform.GetChild(1).GetComponent<MembraneCreatorController>().ReselectBottle();
                        break;
                    case 2:
                        player.transform.GetChild(2).GetComponent<FreezerController>().ReselectBottle();
                        break;
                }
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

        player.transform.GetChild(1).GetComponent<MembraneCreatorController>().presentStrength = padStrength.totalStrength; //물병에 현재 가해진 힘 전달

        //뛰면서 회전
        rb.velocity = padDirection.direction * padStrength.totalStrength;
        rb.AddTorque(key*rotateSpeed, ForceMode2D.Impulse);

        trajectoryLine.Delete();
    }

    
}
