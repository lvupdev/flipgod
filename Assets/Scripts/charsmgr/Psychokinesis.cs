using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Psychokinesis : MonoBehaviour
{
    private BottleSelectController bottleSelectController;
    private PlayerImageController playerImageController;
    private SuperPowerPanelController SPPController;
    private ScreenEffectController screenEffectController;

    private int superPowerLV; //초능력 강화 레벨
    private int skillLV; //필살기 강화 레벨
    private Vector2 initPos;//화면을 눌렀을 때의 위치
    private Vector2 endPos;//화면에서 손을 땠을 떄의 위치
   


    void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        SPPController = GameObject.Find("SuperPowerPanel").GetComponent<SuperPowerPanelController>();
        screenEffectController = GameObject.Find("Main Camera").GetComponent<ScreenEffectController>();

        superPowerLV = 1;
        skillLV = 1;
    }


    public void Activate() //염력 초능력 사용
    {
        //SuperPowePanelController 값 가져오기
        initPos = SPPController.GetInitPos();
        endPos = SPPController.GetEndPos();

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
}