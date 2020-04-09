using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Freezer : MonoBehaviour
{
    private int superPowerLV; //초능력 강화 레벨
    private int skillLV; //필살기 강화 레벨
    private BottleSelectController bottleSelectController;
    private PlayerImageController playerImageController;
    private SuperPowerPanelController SPPController;
    private ScreenEffectController screenEffectController;

    private Vector2 initPos;//화면을 눌렀을 때의 위치
    private Vector2 endPos;//화면에서 손을 땠을 떄의 위치

    public bool freezeAvailable;
    private float freezeRad; //빙결 가능 범위 반지름

    // Start is called before the first frame update
    void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        SPPController = GameObject.Find("SuperPowerPanel").GetComponent<SuperPowerPanelController>();
        screenEffectController = GameObject.Find("Main Camera").GetComponent<ScreenEffectController>();

        superPowerLV = 1;
        freezeAvailable = true;
        freezeRad = superPowerLV * 3; //빙결자의 초능력 강화 레벨 수치의 세 배 만큼이 빙결 가능 범위의 반지름이 된다.
    }

    // Update is called once per frame

    public void Activate()
    {
        screenEffectController.FreezeEffect();
        bottleSelectController.bottle.transform.Find("FreezeRange").GetComponent<FreezeEffect>().freeze();
        bottleSelectController.bottle.transform.Find("FreezeRange").gameObject.SetActive(false);
        freezeAvailable = false;
    }
}
