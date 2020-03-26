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

    private Vector2 initPos;//화면을 눌렀을 때의 위치
    private Vector2 endPos;//화면에서 손을 땠을 떄의 위치

    public int freezeNum = 1; //빙결 능력을 사용할 수 있는 횟수
    private float freezeRad; //빙결 가능 범위 반지름

    // Start is called before the first frame update
    void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        SPPController = GameObject.Find("SuperPowerPanel").GetComponent<SuperPowerPanelController>();
        superPowerLV = 1;
        freezeRad = superPowerLV * 3; //빙결자의 초능력 강화 레벨 수치의 세 배 만큼이 빙결 가능 범위의 반지름이 된다.
    }

    // Update is called once per frame

    public void Activate()
    {
        if (freezeNum == 1)
        {
            GameObject dynamicStructures = GameObject.Find("Dynamic Structure");
            for (int i = 0; i < dynamicStructures.transform.childCount; i++)
            {
                float distance = (dynamicStructures.transform.GetChild(i).position - bottleSelectController.bottle.transform.position).magnitude;
                if (distance <= freezeRad)
                {
                    dynamicStructures.transform.GetChild(i).GetComponent<DSController>().isFreezed = true;
                    Debug.Log("얼어라!!");
                }
            }
            freezeNum = 0;
        }
    }
}
