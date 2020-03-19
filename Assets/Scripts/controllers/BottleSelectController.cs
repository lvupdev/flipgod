using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleSelectController : MonoBehaviour
{
    private Psychokinesis psychokinesis;
    private MembraneCreator membraneCreator;
    private Freezer freezer;
    private ScreenEffectController screenEffectController;
    public GameObject bottle;
    public BottleController bottleController;
    private GameObject redAura;
    private float delta; //시간 변수
    public bool reload;



    private void Start()
    {
        psychokinesis = GameObject.Find("Player").GetComponent<Psychokinesis>();
        membraneCreator = GameObject.Find("Player").GetComponent<MembraneCreator>();
        freezer = GameObject.Find("Player").GetComponent<Freezer>();
        screenEffectController = GameObject.Find("Main Camera").GetComponent<ScreenEffectController>();
        bottle = GameObject.Find("BottlePrefab");
        bottleController = bottle.GetComponent<BottleController>();
        redAura = bottle.transform.Find("RedAura").gameObject;
        reload = false;
        delta = 0;
    }

    public void ReselectBottle()
    {
        bottle = GameObject.FindWithTag("isActBottle");
        bottleController = bottle.GetComponent<BottleController>();//힘을 적용할 물병을 태그에 따라 재설정
        membraneCreator.membraneNum = membraneCreator.getSuperPowerLV();
        membraneCreator.membraneAvailable = false;
        reload = true;

        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            screenEffectController.shadowEffect.enabled = false;
            screenEffectController.screenEffectNum = 1;
        }
    }
}
