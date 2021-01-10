using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Stage의 UI를 관리하는 스크립트
 * 게임을 진행하며 바뀌는 값을 수시로 update하는 UI의 경우,
 * 그 값을 StageManger1...<num> 에서 받아와 코루틴으로 update하도록 한다.
 * (To Do) 현재는 StageManager1으로 특정하였으나 다른 stage를 다루려면 수정해야 함.
 * 이 스크립트는 단순히 UI와 관련한 변수와 메서드만을 다룬다.
 */

public class StageUIManager : MonoBehaviour
{
	//================[Static Field]=====================================
	// Static instance of StageUIManager
	private static StageUIManager instance;
	public static StageUIManager Instance { get { return instance; } }
	//===================================================================

	// Canvas of stage UI
	public Transform canvas;
	// Transform of canvas element
	private static Dictionary<string, Transform> uis = new Dictionary<string, Transform>();

	// Elements of UI
	private Transform pausePanel;
	private Transform missionPanel;

	// Elements of score panel
	private Transform scorePanel;
	private Text completeMissionCountText;
	private Text timeText;
	private Text bottleCountText;

	// Elements of tension gague UI
	private Image tensionValueImg;
	private float lerpSpeed = 0.5f;

	private int completeMissionNumber, totalMissionNumber = 0;
	private int usedBottle, totalBottle = 0;
	private float usedTime, totalTime = .0f;

	private void Awake()
	{
		// itself
		instance = this;
	}

	// Start is called before the first frame update
	void Start()
	{
		if (Time.timeScale == 0.0f)
		{
			Time.timeScale = 1.0f;
		}
		
		pausePanel = GameObject.Find("Panel_Pause").transform;
		pausePanel.gameObject.SetActive(false);

		missionPanel = GameObject.Find("Panel_Mission").transform;

		scorePanel = GameObject.Find("Panel_Score").transform;
		completeMissionCountText = scorePanel.GetChild(0).GetChild(1).GetComponent<Text>();
		timeText = scorePanel.GetChild(1).GetChild(1).GetComponent<Text>();
		bottleCountText = scorePanel.GetChild(2).GetChild(1).GetComponent<Text>();

		tensionValueImg = GameObject.Find("Image_TensionGaugeBar").GetComponent<Image>();
		tensionValueImg.fillAmount = 0.0f;

		InitStageData();
	}

	public void InitStageData()
	{
		if (StageGameManager.Instance.StageData != null)
		{
			totalMissionNumber = StageGameManager.Instance.StageData.GoalNumber;
			totalBottle = StageGameManager.Instance.StageData.LimitedBottleNumber;
			totalTime = StageGameManager.Instance.StageData.LimitedTime;
		}
		else
		{
			Debug.Log("==============Stage Data is not referenced==============");
		}
	}

	private void Update()
	{
		UpdateScoreTexts();
	}

	/*=================<Update texts of score panel>================================*/
	private void UpdateScoreTexts()
	{
		completeMissionNumber = StageGameManager.Instance.CompleteMissionNumber;
		usedBottle = StageGameManager.Instance.UsedBottleNumber;
		usedTime = StageGameManager.Instance.UsedTime;

		UpdateCompleteMissionNumberText(completeMissionNumber, totalMissionNumber);
		UpdateBottleText(usedBottle, totalBottle);
		UpdateTimeText(usedTime, totalTime);
	}

	// Update mission target text
	private void UpdateCompleteMissionNumberText(int complete, int total)
	{
		completeMissionCountText.text = complete + "/" + total;
	}

	// Update time text
	private void UpdateTimeText(float used, float total)
	{
		string temp;
		int minute, second;

		second = Mathf.FloorToInt(used);
		minute = second / 60;
		second = second % 60;
		temp = minute + ":" + second + "/";

		second = Mathf.FloorToInt(total);
		minute = second / 60;
		second = second % 60;
		temp += (minute + ":" + second);

		timeText.text = temp;
	}

	// Update bottle text
	private void UpdateBottleText(int used, int total)
	{
		bottleCountText.text = used + "/" + total;
	}

	// Update tension gauge
	private void UpdateTensionGauge(float tensionValue)
	{
		if (tensionValueImg.fillAmount < 0.999f)
		{
			if (tensionValueImg.fillAmount != tensionValue)
			{
				tensionValueImg.fillAmount =
					Mathf.Lerp(tensionValueImg.fillAmount, tensionValue, Time.deltaTime * lerpSpeed);
			}
		}
	}

	/*==========[Call-back Methods]===========================================*/

	// Use tension gauge, then set tensionvalue to zero
	public void UseTensionGauge(float tensionValue)
	{
		if (tensionValue >= 0.9999f)
		{
			tensionValueImg.fillAmount =
				Mathf.Lerp(tensionValue, 0f, Time.deltaTime * lerpSpeed);
		}
		else
		{
			// Debug.Log("Tesnion value is not enough to use superpower.");
		}
	}

	// Close mission panel with window and
	// Start game
	public void CloseMissionWindow()
	{
		// Unactivate mission panel
		missionPanel.gameObject.SetActive(false);
		// Set time scale to 1, then start game
		Time.timeScale = 1.0f;
	}

	// Activate pause panel with window
	public void ShowPausePanel()
	{
		// Pause the game 
		Time.timeScale = 0.0f;
		// Activate pasue panel with window
		pausePanel.gameObject.SetActive(true);
	}

	// Resume game
	public void ResumeGame()
	{
		// Unactivate pause panel with window
		pausePanel.gameObject.SetActive(false);
		// Set time scale to 1, then resume current stage
		Time.timeScale = 1;
	}

	// Retry stage
	public void RetryStage()
	{
		int index = StageGameManager.Instance.GetCurrentStageNumber();
		SceneManager.LoadScene("Stage" + "-" + index);
		StageGameManager.Instance.InitializeStage();
	}

	// Go to select stage
	public void GoToSelectScene()
	{
		SceneManager.LoadScene("SelectStage");
	}

}
