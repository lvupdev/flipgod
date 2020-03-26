using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SuperPowerController : MonoBehaviour
{
    private BottleSelectController bottleSelectController;
    private Psychokinesis psychokinesis;
    private MembraneCreator membraneCreator;
    private Freezer freezer;
    private PlayerImageController playerImageController;
    private SuperPowerPanelController SPPController;

    private void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        psychokinesis = GameObject.Find("Player").GetComponent<Psychokinesis>();
        membraneCreator = GameObject.Find("Player").GetComponent<MembraneCreator>();
        freezer = GameObject.Find("Player").GetComponent<Freezer>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        SPPController = GameObject.Find("SuperPowerPanel").GetComponent<SuperPowerPanelController>();
    }

    private void FixedUpdate()
    {
        if (bottleSelectController.bottleController.isSuperPowerAvailabe) //초능력 실행
        {
            switch (playerImageController.playingChr)
            {
                case 0:
                    if(SPPController.GetIsTouch()) psychokinesis.Activate();
                    break;
                case 1:
                    if ((membraneCreator.membraneNum > 0) && membraneCreator.membraneAvailable)
                    {
                        membraneCreator.Activate();
                    }
                    break;
                case 2:
                    if(SPPController.GetIsTouch()) freezer.Activate();
                    break;
            }
        }
    }
}
