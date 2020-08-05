using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Psychokinesis : MonoBehaviour
{
    private BottleSelectController bottleSelectController;
    private PlayerImageController playerImageController;
    private SuperPowerPanelController panel_SuperPower;
    private ScreenEffectController screenEffectController;
    private ResourceManager gameResourceValue;
    private GameObject bottles;

    private int superPowerLV; //초능력 강화 레벨
    private Vector2 initPos;//화면을 눌렀을 때의 위치
   


    void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        panel_SuperPower = GameObject.Find("Panel_SuperPower").GetComponent<SuperPowerPanelController>();
        screenEffectController = GameObject.Find("Main Camera").GetComponent<ScreenEffectController>();
        gameResourceValue = GameObject.Find("GameResource").GetComponent<ResourceManager>();
        bottles = GameObject.Find("Bottles");

        superPowerLV = gameResourceValue.GetSuperPowerLV(0);
    }


    public void Activate() //염력 초능력 사용
    {
        //SuperPowePanelController 값 가져오기
        initPos = panel_SuperPower.GetInitPos();

        screenEffectController.KinesisEffect();

        if (initPos.x > Screen.width / 2.0f) //화면 터치 위치가 스크린 오른편이면 시계방향으로 회전 힘을 가한다.
        {
            bottleSelectController.bottleController.rb.AddTorque(-superPowerLV / 60.0f, ForceMode2D.Impulse); //가하는 힘은 초능력 강화 레벨을 60으로 나눈 수치
        }
        if (initPos.x <= Screen.width / 2.0f)//화면 터치 위치가 스크린 왼편이면 시계반대방향으로 회전 힘을 가한다.
        {
            bottleSelectController.bottleController.rb.AddTorque(superPowerLV / 60.0f, ForceMode2D.Impulse);
        }
    }

    public void SkillActivate()
    {
        for(int i = 0; i < bottles.transform.childCount; i++)
        {
            if (bottles.transform.GetChild(i).transform.GetChild(0).gameObject.activeSelf)
            {
                bottles.transform.GetChild(i).gameObject.GetComponent<BottleController>().standingBySkill = true;
            }
        }
    }
}