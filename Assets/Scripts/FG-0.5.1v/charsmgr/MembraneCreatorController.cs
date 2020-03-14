using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MembraneCreatorController : SuperPowerController
{
    private bool membraneAvailable; //탄성막을 생성해도 되는지의 여부
    private int membraneNum; //생성할 수 있는 탄성막의 개수
    public float presentStrength { get; set; } //현재 물병에 가해진 힘

    //값 수정 함수
    public void SetMembraneAvailable(bool x) { membraneAvailable = x; }

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
        superPowerLV = 1;
        membraneAvailable = false;
        membraneNum = superPowerLV; //탄성막 생성자의 초능력 강화 레벨의 수치만큼 탄성막을 생성할 수 있다.
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bottleController.isSuperPowerAvailabe && (playerImageController.playingChr == 1) 
            && (membraneNum > 0) && membraneAvailable) Activate();
    }

    private void Activate()
    {
        //SuperPowePanelController 값 가져오기
        initPos = SPPController.GetInitPos();
        endPos = SPPController.GetEndPos();

        Vector2 direction = endPos - initPos; //화면 드래그 방향
        bottleController.rb.velocity = direction.normalized * presentStrength; // 물병을 던졌을 때의 힘만큼 속도를 가한다.
        membraneNum -= 1; //생성할 수 있는 탄성막의 개수 감소
        membraneAvailable = false; //다시 탄성막을 생성하려면 반드시 한 번 더 화면을 터치해야 함.
    }

    public void ReselectBottle() 
    {
        bottle = GameObject.FindWithTag("isActBottle");
        bottleController = bottle.GetComponent<BottleController>();//힘을 적용할 물병을 태그에 따라 재설정
        membraneNum = superPowerLV; //생성할 수 있는 탄성막의 개수 초기화
        membraneAvailable = false;
    }
}


