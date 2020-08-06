using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleSelectController : MonoBehaviour
{
    private MembraneCreator membraneCreator;
    private Freezer freezer;
    private ScreenEffectController screenEffectController;

    public GameObject bottle;
    public BottleController bottleController;
    public BottleSkillOperation bottleSkillOperation;


    private void Start()
    {
        membraneCreator = GameObject.Find("Player").GetComponent<MembraneCreator>();
        freezer = GameObject.Find("Player").GetComponent<Freezer>();
        screenEffectController = GameObject.Find("Main Camera").GetComponent<ScreenEffectController>();
        bottle = GameObject.FindWithTag("isActBottle");
        bottleController = bottle.GetComponent<BottleController>();
        bottleSkillOperation = bottle.GetComponent<BottleSkillOperation>();
    }

    public void ReselectBottleWithDelay(float delay)
    {
        Invoke("ReselectBottle", delay);
    }
    public void SetActiveBottleWithDelay(float delay)
    {
        Invoke("SetActiveBottle", delay);
    }
    public void SetActiveBottle()
    {
        bottle.SetActive(true);
    }

    public void ReselectBottle()
    {
        bottle = GameObject.FindWithTag("isActBottle");
        bottleController = bottle.GetComponent<BottleController>();//힘을 적용할 물병을 태그에 따라 재설정
        bottleSkillOperation = bottle.GetComponent<BottleSkillOperation>();
        membraneCreator.membraneNum = membraneCreator.getSuperPowerLV();
        membraneCreator.membraneAvailable = false;
        freezer.freezeAvailable = true;
    }
}

