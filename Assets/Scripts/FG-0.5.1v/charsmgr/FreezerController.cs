using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FreezerController : SuperPowerController
{
    private int freezeNum = 1; //빙결 능력을 사용할 수 있는 횟수
    private float freezeRad; //빙결 가능 범위 반지름

    // Start is called before the first frame update
    void Start()
    {
        bottle = GameObject.Find("BottlePrefab");
        bottleController = bottle.GetComponent<BottleController>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        SPPController = GameObject.Find("SuperPowerPanel").GetComponent<SuperPowerPanelController>();
        blurEffect = GameObject.Find("Main Camera").GetComponent<RadialBlurImageEffect>();
        height = 2 * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        blurTime = 1;
        freezeRad = superPowerLV * 3; //빙결자의 초능력 강화 레벨 수치의 두 배 만큼이 빙결 가능 범위의 반지름이 된다.
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (bottleController.isSuperPowerAvailabe && (playerImageController.playingChr == 2)) Activate();
    }

    protected void Activate()
    {
        //SuperPowePanelController 값 가져오기
        initPos = SPPController.GetInitPos();
        endPos = SPPController.GetEndPos();
        isTouch = SPPController.GetIsTouch();

        if (isTouch && (freezeNum == 1))
        {
            GameObject dynamicStructures = GameObject.Find("Dynamic Structure");
            for (int i = 0; i < dynamicStructures.transform.childCount; i++)
            {
                float distance = (dynamicStructures.transform.GetChild(i).position - bottle.transform.position).magnitude;
                if (distance <= freezeRad)
                {
                    dynamicStructures.transform.GetChild(i).GetComponent<DSController>().isFreezed = true;
                    Debug.Log("얼어라!!");
                }
            }
            freezeNum = 0;
        }
    }

    public void ReselectBottle()
    {
        bottle = GameObject.FindWithTag("isActBottle");
        bottleController = bottle.GetComponent<BottleController>();//힘을 적용할 물병을 태그에 따라 재설정
        freezeNum = 1;
    }
}
