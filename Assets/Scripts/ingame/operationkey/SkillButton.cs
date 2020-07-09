using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    private GameObject tensionGaugeBar; //텐션게이지 이미지
    private Button button; //Button_Skill
    private BottleSelectController bottleSelectController;
    private PadStrength padStrength;
    private bool usingSkill; //Sill 버튼을 사용 중인지의 여부

// Start is called before the first frame update
    void Start()
    {
        tensionGaugeBar = GameObject.Find("Image_TensionGaugeBar");
        button = GetComponent<Button>();
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        usingSkill = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((!bottleSelectController.bottleController.isSuperPowerAvailabe) && (!padStrength.isThrowing) && 
            (!padStrength.isTouch) && (tensionGaugeBar.GetComponent<Image>().fillAmount == 1)) button.interactable = true;
    }

    public void PushSkillButton()
    {
        if (usingSkill)
        {
            usingSkill = false;
        }
        else
        {
            usingSkill = true;
        }
    }
}
