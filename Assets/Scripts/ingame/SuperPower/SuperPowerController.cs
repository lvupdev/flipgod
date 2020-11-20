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
    private SuperPowerPanelController panel_SuperPower;

    private void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        psychokinesis = GameObject.Find("Player").GetComponent<Psychokinesis>();
        membraneCreator = GameObject.Find("Player").GetComponent<MembraneCreator>();
        freezer = GameObject.Find("Player").GetComponent<Freezer>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        panel_SuperPower = GameObject.Find("Panel_SuperPower").GetComponent<SuperPowerPanelController>();
    }

    private void FixedUpdate()
    {
        if (bottleSelectController.bottleController.isSuperPowerAvailabe) //초능력 실행
        {
            switch (playerImageController.getPlayingChr())
            {
                case 0:
                    if (panel_SuperPower.GetIsTouch() && psychokinesis.psychoAvailable)
                    {
                        psychokinesis.Activate();
                        playerImageController.ChangePlayerImage(2); //초능력 사용 이미지로 변경
                    }
                    break;
                case 1:
                    if ((membraneCreator.membraneNum > 0) && membraneCreator.membraneAvailable)
                    {
                        membraneCreator.Activate();
                        playerImageController.ChangePlayerImage(2); //초능력 사용 이미지로 변경
                    }
                    break;
                case 2:
                    if(panel_SuperPower.GetIsTouch() && freezer.freezeAvailable)
                    {
                        freezer.Activate();
                        playerImageController.ChangePlayerImage(2); //초능력 사용 이미지로 변경
                    }
                    break;
            }
        }
    }
}
