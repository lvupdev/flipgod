using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SuperPowerController : MonoBehaviour
{
    private PsychokinesisController psychokinesisController;
    private MembraneCreatorController membraneCreatorController;
    private FreezerController freezerController;

    private void Start()
    {
        psychokinesisController = GameObject.Find("Player").GetComponent<PsychokinesisController>();
        membraneCreatorController = GameObject.Find("Player").GetComponent<MembraneCreatorController>();
        freezerController = GameObject.Find("Player").GetComponent<FreezerController>();
    }

    private void FixedUpdate()
    {
        
    }
}
