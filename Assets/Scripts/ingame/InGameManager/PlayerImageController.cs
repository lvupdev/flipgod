using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * 플레이어 관리 스크립트입니다.
 * 염동력자, 탄성막 생성자, 빙결자에게 배정한 숫자는 순서대로 0, 1, 2 입니다.
 * 플레이어 이미지 관리는 물론 각 물병 스프라이트의 위치도 관리합니다.
*/
public class PlayerImageController : MonoBehaviour
{
    private int whichCase; //플레이어 스프라이트 교체 시 어떤 자세인지
    private SpriteRenderer spriteRenderer;
    private PadStrength padStrength;
    public PadDirection padDirection;
    private BottleSelectController bottleSelectController;
    private SkillButton skillButton;
    private Image charSlot1; //캐릭터 슬롯 1 아이콘
    private Image charSlot2; //캐릭터 슬롯 2 아이콘

    private Button characterSlot_1; //캐릭터 슬롯 1 버튼
    private Button characterSlot_2; //캐릭터 슬롯 2 버튼

    private int playingChr; //조작할 수 있는 캐릭터
    private int firstSlotChr; //교체 슬롯 1번에 있는 캐릭터
    private int secondSlotChr; //교체 슬롯 2번에 있는 캐릭터

    public Sprite[] standingSprites; // 스탠딩 이미지를 담아놓는 배열
    public Sprite[] iconSprites; //아이콘 이미지를 담아놓는 배열
    public Sprite[] throwingSprites; // 던지는 이미지를 담아놓는 배열
    public Sprite[] superpowerSprites; // 초능력 쓰는 이미지를 담아놓는 배열

    private Vector3 bottlePosition; //물병이 위치해야 하는 포지션
    private int key; //플레이어의 스프라이트 방향 결정 겂

    //값 반환 함수
    public Vector3 getBottlePosition() { return bottlePosition; }
    public int getPlayingChr() { return playingChr; }
  
    private void Awake()
    {
        bottlePosition = transform.GetChild(0).transform.position;
    }

    void Start()
    {
        playingChr = 0;
        firstSlotChr = 1;
        secondSlotChr = 2;

        spriteRenderer = GetComponent<SpriteRenderer>();
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        skillButton = GameObject.Find("Button_Skill").GetComponent<SkillButton>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        padDirection = GameObject.Find("Joystick").GetComponent<PadDirection>();
        charSlot1 = GameObject.Find("ControllButtons").transform.Find("Button_CharacterSlot_1").GetComponent<Image>();
        charSlot2 = GameObject.Find("ControllButtons").transform.Find("Button_CharacterSlot_2").GetComponent<Image>();
        characterSlot_1 = GameObject.Find("ControllButtons").transform.Find("Button_CharacterSlot_1").GetComponent<Button>();
        characterSlot_2 = GameObject.Find("ControllButtons").transform.Find("Button_CharacterSlot_2").GetComponent<Button>();

        characterSlot_1.onClick.AddListener(CharacterSlot1);
        characterSlot_2.onClick.AddListener(CharacterSlot2);


        spriteRenderer.sprite = standingSprites[playingChr];
        charSlot1.sprite = iconSprites[firstSlotChr];
        charSlot2.sprite = iconSprites[secondSlotChr];
    }

    private void Update()
    {
        if (padDirection.getDirection().x <= 0) key = 1;
        if (padDirection.getDirection().x > 0) key = -1;

        if(!skillButton.getUsingSkill()) transform.localScale = new Vector3(key * 0.4f, 0.4f, 1); //필살기 사용중이 아니면 패드 위치에 따라 캐릭터가 향하는 방향이 바뀜

        bottlePosition = transform.GetChild(playingChr).transform.position;
    }

    public void CharacterSlot1()
    {
        if((!bottleSelectController.bottleController.isSuperPowerAvailabe) && (!padStrength.isThrowing) &&(!padStrength.isTouch))
        {
            //빙결자 스프라이트 높이 문제로 캐릭터 이미지 변경 때마다 높이 재설정
            if (firstSlotChr == 2) 
			{
                gameObject.transform.position += new Vector3(0, 0.2f, 0);
			}
            else if(playingChr == 2)
			{
                gameObject.transform.position -= new Vector3(0, 0.2f, 0);
            }

            int temp;
            temp = playingChr;
            playingChr = firstSlotChr;
            firstSlotChr = temp;
            charSlot1.sprite = iconSprites[firstSlotChr];
            spriteRenderer.sprite = standingSprites[playingChr];

            bottleSelectController.bottle.transform.position = transform.GetChild(playingChr).transform.position;
            bottlePosition = transform.GetChild(playingChr).transform.position; //  물병 위치 결정
        }
    }

    public void CharacterSlot2()
    {
        if ((!bottleSelectController.bottleController.isSuperPowerAvailabe) && (!padStrength.isThrowing) && (!padStrength.isTouch))
        {
            //빙결자 스프라이트 높이 문제로 캐릭터 이미지 변경 때마다 높이 재설정
            if (secondSlotChr == 2)
            {
                gameObject.transform.position += new Vector3(0, 0.2f, 0);
            }
            else if (playingChr == 2)
            {
                gameObject.transform.position -= new Vector3(0, 0.2f, 0);
            }

            int temp;
            temp = playingChr;
            playingChr = secondSlotChr;
            secondSlotChr = temp;
            charSlot2.sprite = iconSprites[secondSlotChr];
            spriteRenderer.sprite = standingSprites[playingChr];

            bottleSelectController.bottle.transform.position = transform.GetChild(playingChr).transform.position;
            bottlePosition = transform.GetChild(playingChr).transform.position; //  물병 위치 결정
        }
    }

    public void ChangePlayerImageWithDelay(float delay, int whichCase)
    {
        this.whichCase = whichCase;
        Invoke("ChangePlayerImage", delay);
    }

    public void ChangePlayerImage(int whichCase) //인수가 0이면 던지기 전 자세, 1이면 던질때 자세, 2이면 필살기 사용 자세로 플레이어 이미지를 교체한다.
    {
        switch (whichCase)
        {
            case 0:
                spriteRenderer.sprite = standingSprites[playingChr];
                break;
            case 1:
                spriteRenderer.sprite = throwingSprites[playingChr];
                break;
            case 2:
                spriteRenderer.sprite = superpowerSprites[playingChr];
                break;
        }
    }
    public void ChangePlayerImage() //인수가 0이면 던지기 전 자세, 1이면 던질때 자세, 2이면 필살기 사용 자세로 플레이어 이미지를 교체한다.
    {
        switch (whichCase)
        {
            case 0:
                spriteRenderer.sprite = standingSprites[playingChr];
                break;
            case 1:
                spriteRenderer.sprite = throwingSprites[playingChr];
                break;
            case 2:
                spriteRenderer.sprite = superpowerSprites[playingChr];
                break;
        }
    }
}
