using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    [SerializeField]
    private StageData stageData = null;
    public StageData StageData { get { return stageData; } }


    // Start is called before the first frame update
    void Start()
    {
        stageData = Resources.Load<StageData>("StageDatas/StageData-"+StageGameManager.GetCurrentStageNumber());    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
