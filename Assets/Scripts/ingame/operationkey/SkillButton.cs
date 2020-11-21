using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    private GameObject tensionGaugeBar; //텐션게이지 이미지
    private GameObject panel_SuperPower; //초능력 입력 패널
    private GameObject membranes;
    private GameObject trajectory;
    private Button button; //Button_Skill
    private PlayerImageController playerImageController;
    private BottleSelectController bottleSelectController;
    private PadStrength padStrength;
    private PadDirection padDirection;
    private TensionGaugeManager tensionGaugeManager;
    private ControllButtonsUIManager controllButtonsUIManager;
    private Psychokinesis psychokinesis;
    private Freezer freezer;
    private bool usingSkill; //Skill 버튼을 사용 중인지의 여부
    private bool skillUsed; //스킬이 유의미하게 사용되었는지의 여부
    private Vector2 previousDirection; //스킬 버튼을 누르기 전 방향키의 벡터

    public Sprite[] skillButtonSprite; // skill버튼 스프라이트 배열

// Start is called before the first frame update
    void Start()
    {
        tensionGaugeBar = GameObject.Find("Image_TensionGaugeBar");
        panel_SuperPower = GameObject.Find("Panel_SuperPower");
        membranes = GameObject.Find("Membranes");
        trajectory = GameObject.Find("Trajectory");
        button = GetComponent<Button>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        padDirection = GameObject.Find("Joystick").GetComponent<PadDirection>();
        tensionGaugeManager = GameObject.Find("Image_TensionGaugeBar").GetComponent<TensionGaugeManager>();
        controllButtonsUIManager = GameObject.Find("UIManager").GetComponent<ControllButtonsUIManager>();
        psychokinesis = GameObject.Find("Player").GetComponent<Psychokinesis>();
        freezer = GameObject.Find("Player").GetComponent<Freezer>();
        usingSkill = false;
        skillUsed = false;
    }

    public bool getUsingSkill()
    {
        return usingSkill;
    }

    // Update is called once per frame
    void Update()
    {
        if ((!bottleSelectController.bottleController.isSuperPowerAvailabe) && (!padStrength.isThrowing) &&
            (!padStrength.isTouch) && (tensionGaugeBar.GetComponent<Image>().fillAmount == 1) && (!controllButtonsUIManager.isOperating)) button.interactable = true;
        else
            button.interactable = false;
    }

    public void PushSkillButton()
    {
        if (usingSkill) //필살기 사용 완료
        {
            GetComponent<Image>().sprite = skillButtonSprite[0];

            //panel_SuperPower.SetActive(true); //초능력 입력 패널 활성화

            if (playerImageController.getPlayingChr() == 0) //염동력자가 필살기를 사용 완료했을 때
            {
                if (bottleSelectController.bottleSkillOperation.getUsingSkillNum() != 0) //필살기를 적용한 물병이 존재하는 경우
                {
                    playerImageController.ChangePlayerImageWithDelay(2, 0); //딜레이 주고 던지는 자세로 교체
                    bottleSelectController.SetActiveBottleWithDelay(2); //딜레이 주고 물통 활성화
                    controllButtonsUIManager.setShowButtonsWithDelay(2, 1); //스킬 버튼과 membrame 추가/제거 버튼을 제외하고 모든 컨트롤 버튼 활성화
                    psychokinesis.SkillActivate();
                    skillUsed = true;
                }
                else //필살기를 적용한 물병이 없을 경우
                {
                    playerImageController.ChangePlayerImage(0); //던지는 자세로 교체
                    bottleSelectController.bottle.SetActive(true); //물통 활성화
                    controllButtonsUIManager.setShowButtons(true, 1); //스킬 버튼과 membrame 추가/제거 버튼을 제외하고 모든 컨트롤 버튼 활성화
                }
            }
            else if (playerImageController.getPlayingChr() == 1) //탄성막 생성자가 필살기를 사용 완료했을 때
            {
                playerImageController.ChangePlayerImage(0); //던지는 자세로 교체
                bottleSelectController.bottle.SetActive(true); //물통 활성화
                padDirection.setDirection(previousDirection); //조이스틱 방향을 원래 방향으로 갱신
                controllButtonsUIManager.setShowButtons(true, 2); // 탄성막 추가 버튼을 숨기고 모든 컨트롤 버튼을 보여줌
                trajectory.SetActive(true); //포물선 활성화
                MembraneUsingSkillEffect.selectedMembrane = null; //탄성막 선택 초기화
                for(int i = 0; i < membranes.transform.childCount; i++)
                {
                    membranes.transform.GetChild(i).GetComponent<MembraneUsingSkillEffect>().setStartDelta(true); //탄성막 파괴 카운트다운 시작
                    membranes.transform.GetChild(i).GetComponent<MembraneUsingSkillEffect>().Activate(); //콜라이더 활성화
                }

                if (membranes.transform.childCount > 0) skillUsed = true;
            }
            else //빙결자가 필살기를 사용 완료했을 때
            {
                if(bottleSelectController.bottleSkillOperation.getUsingSkillNum() != 0) //필살기를 젹용하는 물병이 존재하는 경우
                {
                    playerImageController.ChangePlayerImageWithDelay(1, 0); //딜레이 주고 던지는 자세로 교체
                    bottleSelectController.SetActiveBottleWithDelay(1); //물통 활성화
                    controllButtonsUIManager.setShowButtonsWithDelay(1, 1); //스킬 버튼과 membrame 추가/제거 버튼을 제외하고 모든 컨트롤 버튼 활성화
                    freezer.SkillActivate();
                    skillUsed = true;
                }
                else
                {
                    playerImageController.ChangePlayerImage(0); //던지는 자세로 교체
                    bottleSelectController.bottle.SetActive(true); //물통 활성화
                    controllButtonsUIManager.setShowButtons(true, 1); //스킬 버튼과 membrame 추가/제거 버튼을 제외하고 모든 컨트롤 버튼 활성화
                }
            }

            if (skillUsed) //유의미하게 스킬을 사용했을 때
            {
                tensionGaugeManager.DecreaseTensionGauge(50); //텐션게이지 감소
                skillUsed = false;
            }
            bottleSelectController.bottleSkillOperation.setUsingSkillNum(0);
            usingSkill = false;
        }
        else // 필살기 사용 시작
        {
            GetComponent<Image>().sprite = skillButtonSprite[1]; //버튼 이미지 교체

            playerImageController.ChangePlayerImage(2); //플레이어 초능력 사용 자세로 교체

            bottleSelectController.bottle.SetActive(false); //물통 비활성화

            if (playerImageController.getPlayingChr() == 1) //탄성막 생성자가 필살기를 사용했을 때
            {
                previousDirection = padDirection.getDirection(); //스킬버튼을 누르기 전의 방향 저장
                controllButtonsUIManager.setHideButtons(true, 2); //탄성막 추가 버튼 보이고 조이스틱을 제외한 컨트롤 버튼 숨김
                trajectory.SetActive(false); //포물선 비활성화
            }
            else //염동력자, 빙결자가 필살기를 사용했을 때
            {
                controllButtonsUIManager.setHideButtons(true, 1); //스킬 버튼을 제외하고 모든 컨트롤 버튼 숨김
            }

            usingSkill = true;
        }
    }
}
