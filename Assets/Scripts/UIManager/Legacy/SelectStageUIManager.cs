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

	}

	private void InitializeScene()
	{

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
