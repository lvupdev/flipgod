using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleSelectController : MonoBehaviour
{
    private MembraneCreator membraneCreator;
    private Freezer freezer;

    public GameObject bottle { get; set; }
    public BottleController bottleController { get; set; }
    public BottleSkillOperation bottleSkillOperation;

    public bool bottleSelected { get; set; } //물병이 선택되었음을 알리는 변수


    private void Start()
    {
        membraneCreator = GameObject.Find("Player").GetComponent<MembraneCreator>();
        freezer = GameObject.Find("Player").GetComponent<Freezer>();
        bottle = GameObject.FindWithTag("isActBottle");
        bottleController = bottle.GetComponent<BottleController>();
        bottleSkillOperation = bottle.GetComponent<BottleSkillOperation>();
        bottleSelected = true;
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
        bottleSelected = true;
    }
}

