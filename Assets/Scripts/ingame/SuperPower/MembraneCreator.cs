using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MembraneCreator : MonoBehaviour
{
    private BottleSelectController bottleSelectController;
    private PlayerImageController playerImageController;
    private SuperPowerPanelController panel_SuperPower;
    private ScreenEffectController screenEffectController;

    private Vector2 initPos;//화면을 눌렀을 때의 위치
    private Vector2 endPos;//화면에서 손을 땠을 떄의 위치
    private int superPowerLV; //초능력 강화 레벨
    private int skillLV; //필살기 강화 레벨

    private Vector2 dragDirection; //드래그 방향 백터
    public bool membraneAvailable; //탄성막을 생성해도 되는지의 여부
    public int membraneNum; //생성할 수 있는 탄성막의 개수
    public float presentStrength; //현재 물병에 가해진 힘

    //값 전달 함수
    public int getSuperPowerLV() { return superPowerLV; }

    //값 수정 함수
    public void SetMembraneAvailable(bool x) { membraneAvailable = x; }

    // Start is called before the first frame update
    void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        panel_SuperPower = GameObject.Find("Panel_SuperPower").GetComponent<SuperPowerPanelController>();
        screenEffectController = GameObject.Find("Main Camera").GetComponent<ScreenEffectController>();

        superPowerLV = 3;
        membraneAvailable = false;
        membraneNum = superPowerLV; //탄성막 생성자의 초능력 강화 레벨의 수치만큼 탄성막을 생성할 수 있다.
    }

    public void Activate()
    {
        screenEffectController.MembraneEffect();
        bottleSelectController.bottleController.rb.velocity = panel_SuperPower.getDragDirection() * presentStrength; // 물병을 던졌을 때의 힘만큼 속도를 가한다.
        membraneNum -= 1; //생성할 수 있는 탄성막의 개수 감소
        membraneAvailable = false; //다시 탄성막을 생성하려면 반드시 한 번 더 화면을 터치해야 함.
    }
}


