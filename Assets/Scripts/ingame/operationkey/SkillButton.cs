using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    private GameObject tensionGaugeBar; //텐션게이지 이미지
    private GameObject panel_SuperPower; //초능력 입력 패널
    private GameObject controllButtons; // 컨트롤 UI 버튼 부모 오브젝트
    private GameObject trajectory;
    private Button button; //Button_Skill
    private PlayerImageController playerImageController;
    private BottleSelectController bottleSelectController;
    private PadStrength padStrength;
    private TensionGaugeManager tensionGaugeManager;
    private Psychokinesis psychokinesis;
    private MembraneCreator membraneCreator;
    private Freezer freezer;
    private bool usingSkill; //Skill 버튼을 사용 중인지의 여부

    public Sprite[] skillButtonSprite; // skill버튼 스프라이트 배열

// Start is called before the first frame update
    void Start()
    {
        tensionGaugeBar = GameObject.Find("Image_TensionGaugeBar");
        panel_SuperPower = GameObject.Find("Panel_SuperPower");
        controllButtons = GameObject.Find("ControllButtons");
        trajectory = GameObject.Find("Trajectory");
        button = GetComponent<Button>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        tensionGaugeManager = GameObject.Find("Image_TensionGaugeBar").GetComponent<TensionGaugeManager>();
        psychokinesis = GameObject.Find("Player").GetComponent<Psychokinesis>();
        membraneCreator = GameObject.Find("Player").GetComponent<MembraneCreator>();
        freezer = GameObject.Find("Player").GetComponent<Freezer>();
        usingSkill = false;
    }

    public bool getUsingSkill()
    {
        return usingSkill;
    }

    // Update is called once per frame
    void Update()
    {
        if ((!bottleSelectController.bottleController.isSuperPowerAvailabe) && (!padStrength.isThrowing) &&
            (!padStrength.isTouch) && (tensionGaugeBar.GetComponent<Image>().fillAmount == 1)) button.interactable = true;
        else
            button.interactable = false;
    }

    public void PushSkillButton()
    {
        if (usingSkill) //필살기 사용 완료
        {
            GetComponent<Image>().sprite = skillButtonSprite[0];

            panel_SuperPower.SetActive(true); //초능력 입력 패널 활성화

            bottleSelectController.bottle.SetActive(true); //물통 활성화

            if(playerImageController.GetPlayingChr() == 0) //염동력자가 필살기를 사용 완료했을 때
            {
                for (int i = 1; i < controllButtons.transform.childCount; i++) //스킬 버튼을 제외하고 모든 컨트롤 버튼 활성화
                {
                    controllButtons.transform.GetChild(i).gameObject.SetActive(true);
                }
                psychokinesis.SkillActivate();
            }
            else if (playerImageController.GetPlayingChr() == 1) //탄성막 생성자가 필살기를 사용 완료했을 때
            {
                for (int i = 1; i < controllButtons.transform.childCount-1; i++) //스킬 버튼을 제외하고 모든 컨트롤 버튼 활성화
                {
                    controllButtons.transform.GetChild(i).gameObject.SetActive(true);
                    trajectory.SetActive(true); //포물선 활성화
                }
            }
            else //빙결자가 필살기를 사용 완료했을 때
            {
                for (int i = 1; i < controllButtons.transform.childCount; i++) //스킬 버튼을 제외하고 모든 컨트롤 버튼 활성화
                {
                    controllButtons.transform.GetChild(i).gameObject.SetActive(true);
                }
                freezer.SkillActivate();
            }

            tensionGaugeManager.DecreaseTensionGauge(bottleSelectController.bottleSkillOperation.getUsingSkillNum()); //텐션게이지 감소
            bottleSelectController.bottleSkillOperation.setUsingSkillNum(0);
            usingSkill = false;
        }
        else // 필살기 사용 시작
        {
            GetComponent<Image>().sprite = skillButtonSprite[1]; //버튼 이미지 교체

            panel_SuperPower.SetActive(false); //초능력 입력 패널 비활성화

            bottleSelectController.bottle.SetActive(false); //물통 비활성화

            if (playerImageController.GetPlayingChr() == 1) //탄성막 생성자가 필살기를 사용했을 때
            {
                for (int i = 1; i < controllButtons.transform.childCount-1; i++) //스킬 버튼과 방향키를 제외하고 모든 컨트롤 버튼 제거
                {
                    controllButtons.transform.GetChild(i).gameObject.SetActive(false);
                }
                trajectory.SetActive(false); //포물선 비활성화
            }
            else //염동력자, 빙결자가 필살기를 사용했을 때
            {
                for(int i = 1; i < controllButtons.transform.childCount; i++) //스킬 버튼을 제외하고 모든 컨트롤 버튼 제거
                {
                    controllButtons.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            usingSkill = true;
        }
    }
}
