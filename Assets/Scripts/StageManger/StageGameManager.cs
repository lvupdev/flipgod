using System;
using System.Collections;
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

	public event Action OnStageCleared;

	private void Awake()
	{
		// itself
		instance = this;

		// Init
		InitializeStage();
	}

	// Update is called once per frame
	void Update()
	{
		Timer();
		CountCompleteMission();
		JudgeGame();
	}

	// Set all value about stage to initial value
	public void InitializeStage()
	{
		InitializeCurrentStageData(GetCurrentStageNumber());
		
		BottleController.InitializeBottleController();

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
		if (CompleteMissionNumber < StageData.GoalNumber)
		{
			switch (StageData.MissionType)
			{
				case StageData.Mission.StandBottle:
					completeMissionNumber = BottleController.CountStandingBottle();
					break;
				case StageData.Mission.StandBottleOnTheTarget:
					completeMissionNumber = PlaneTile.CountStandingBottleOnPlaneTile();
					break;
			}

		}
	}

	// Judge whether is the mission(game) complete
	public void JudgeGame()
	{
		// TODO: add condition for complete
		// if user complete the mission,

		if (CompleteMissionNumber == StageData.GoalNumber)
		{
			UserRecordManager.JudgeNewRecord(stageData.StageIndexNumber, UsedBottleNumber, UsedTime);
			OnStageCleared?.Invoke();
			StartCoroutine(GoToStageClearSceneAsync());
		}
	}

	// Go to stage clear scene
	public IEnumerator GoToStageClearSceneAsync()
	{
		yield return new WaitForSeconds(2.0f);

		SceneManager.LoadScene("StageClear");
	}


}
