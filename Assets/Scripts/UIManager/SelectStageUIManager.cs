using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectStageUIManager : MonoBehaviour
{
	//================[Static Field]=====================================
	// Static instance of StageGameManager
	private static SelectStageUIManager instance = null;
	public static SelectStageUIManager Instance { get { return instance; } }
	//===================================================================

	private int clearIndex = 0;
	public Transform content;
	private GameObject textButton;

	private void Awake()
	{
		// itself
		instance = this;

		// Init
		InitializeScene();
	}

	private void Start()
	{
		textButton = Resources.Load<GameObject>("UI/Button_Test");

		for (var i = 0; i < clearIndex + 1; i++)
		{
			GameObject newButton = Instantiate(textButton);
			StageData stageData = Resources.Load<StageData>("StageDatas/StageData-" + (i + 1));
			newButton.transform.localPosition = Vector3.zero;
			newButton.transform.SetParent(content, false);
			newButton.transform.GetChild(0).GetComponent<Text>().text = "TEST#" + (i + 1);
			newButton.transform.GetChild(2).GetComponent<Text>().text = "임시 텍스트";
			newButton.transform.GetChild(4).GetComponent<Text>().text = "임시 시간 텍스트";
			newButton.transform.GetChild(6).GetComponent<Text>().text = "임시 물병 텍스트";
			newButton.transform.GetChild(7).GetComponent<Button>().onClick.AddListener(() => StartGame(i + 1));
		}
	}

	private void InitializeScene()
	{
		clearIndex = UserRecordManager.GetSavedIndexOfClearScene();
	}

	public void ReturnToLobby()
	{
		SceneManager.LoadScene("Start");
	}

	public void StartGame(int index)
	{
		SceneManager.LoadScene("Stage-" + index);
		if (Time.timeScale == 0.0f)
		{
			Time.timeScale = 1.0f;
		}
	}
}
