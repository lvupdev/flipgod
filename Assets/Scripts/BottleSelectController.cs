using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleSelectController : MonoBehaviour
{
    private PsychokinesisController psychokinesisController;
    private MembraneCreatorController membraneCreatorController;
    private FreezerController freezerController;
    public GameObject bottle;
    private BottleController bottleController;
    private GameObject redAura;
    private float delta; //시간 변수
    public bool reload;



    private void Start()
    {
        psychokinesisController = GameObject.Find("Player").GetComponent<PsychokinesisController>();
        membraneCreatorController = GameObject.Find("Player").GetComponent<MembraneCreatorController>();
        freezerController = GameObject.Find("Player").GetComponent<FreezerController>();
        bottle = GameObject.Find("BottlePrefab");
        bottleController = bottle.GetComponent<BottleController>();
        redAura = bottle.transform.Find("RedAura").gameObject;
        reload = false;
        delta = 0;
    }

    private void FixedUpdate()
    {
        if (reload) delta += Time.fixedDeltaTime;
        if (delta > 1)
        {
            reload = false;
            delta = 0;
        }
    }

    public void ReselectBottle()
    {
        bottle = GameObject.FindWithTag("isActBottle");
        bottleController = bottle.GetComponent<BottleController>();//힘을 적용할 물병을 태그에 따라 재설정
        redAura = bottle.transform.Find("RedAura").gameObject;
        membraneCreatorController.membraneNum = membraneCreatorController.getSuperPowerLV(); //생성할 수 있는 탄성막의 개수 초기화
        membraneCreatorController.membraneAvailable = false;
        reload = true;

        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            psychokinesisController.kinesisNum = 1;
            psychokinesisController.shadowEffect.enabled = false;
        }
    }
}
