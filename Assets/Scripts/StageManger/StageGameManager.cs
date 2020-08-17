﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * This script handles every methods and variables contained in each StageManagers.
 */
public class StageGameManager : MonoBehaviour
{
    //================[Static Field]=====================================
    // Static instance of StageGameManager
    private static StageGameManager instance = null;
    public static StageGameManager Instance { get { return instance; } }
    //===================================================================

    // Container of stage data (initial value)
    [SerializeField]
    private StageData stageData; 
    public StageData StageData { get { return stageData; } }

    //========values of used resource in current Stage=======================
    private int usedBottleNumber = 0;
    public int UsedBottleNumber { get { return usedBottleNumber; } }

    private float usedTime = 0;
    public float UsedTime { get { return usedTime; } }
    //=========================================================================

    // the number of complete mission
    private int completeMissionNumber = 0;
    public int CompleteMissionNumber { get { return completeMissionNumber; } }

    private void Awake()
    {
        // itself
        instance = this;


        // (TO DO) 이게 왜 안 되는지 모르겠네
        //======Init static instance of StageGameManager=========
        // If instance already exists, then destroy this
        // if not, set instance to this
        /*
        if (instance = null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }       
         */


        // Init
        InitializeStage();
    }

    public void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    // Set all value about stage to initial value
    public void InitializeStage()
    {
        InitializeCurrentStageData(GetCurrentStageNumber());

        usedBottleNumber = 0;
        usedTime = 0;
        completeMissionNumber = 0;
    }

    // Get number of current stage
    // and Set current stage data using that value
    public void InitializeCurrentStageData(int currentStageNumber)
    {
        /*=======Set Stage Data about stage========
         * name of Stage Data: StageData-*
         * * is the number of stage
         */
        stageData = Resources.Load<StageData>("StageDatas/StageData-" + currentStageNumber);
    }

    // Get number of current stage
    public int GetCurrentStageNumber()
    {
        // scene name (stage name) : "Stage-<Stage num>"
        // Get current scene name
        string sceneName = SceneManager.GetActiveScene().name;
        string[] parsed = sceneName.Split('-');
        int stageNum = int.Parse(parsed[1]);

        // Return current stage number
        return stageNum;
    }

    // Increase a timer value every second
    public void Timer()
    {
        // Add 1 to used Time
        usedTime += Time.deltaTime;

    }

    // Increase number of used bottle 
    public void CountUsedBottle()
    {
        //Add 1 to used bottle number
        usedBottleNumber += 1;

    }

    // Increase number of complete mission
    public void CountCompleteMission()
    {
        // If complete mission number is smaller than tartget number
        if (CompleteMissionNumber < StageData.TargetNumber)
        {
            // then Add 1 to complete mission number
            completeMissionNumber += 1;
        }
    }

    // Judge whether is the mission(game) complete
    public void JudgeGame()
    {
        // if user complete the mission,
        if (CompleteMissionNumber == StageData.TargetNumber)
        {
            // then Save current stage data and user record to show result in stage clear scene
            // and Go to stage clear scene
            SaveStageDataAndUserRecord();
            GoToStageClearScene();
        }
    }

    // Save current stage data and user record
    public void SaveStageDataAndUserRecord()
    {
        // Save current stage data at Instance of Stage Clear UI Manager
        StageClearUIManager.Instance.stageData = StageData;
        // Save current user record at Instance of Stage Clear UI Manager
        StageClearUIManager.Instance.userRecord
            = UserRecord.AddUserRecord(StageData.StageIndexNumber, UsedBottleNumber, UsedTime, CompleteMissionNumber);
    }

    // Go to stage clear scene
    public void GoToStageClearScene()
    {

    }


}
