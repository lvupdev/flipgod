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
    // Canvas of stage UI
    public Canvas canvas;
    // Transform of canvas element
    private static Dictionary<string, Transform> uis = new Dictionary<string, Transform>();

    // Elements of UI
    private static Transform pausePanel;
    private static Transform missionPanel;

    // Elements of score panel
    private static Transform scorePanel;
    private static Text timeText;
    private static Text bottleCountText;

    // Elements of tension gague UI
    private static Image tensionValueImg;
    private float lerpSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize ui
        RecursiveRegisterChild(canvas.transform, uis);

        pausePanel = Find("Panel_Pause");
        scorePanel = Find("Panel_Score");
        timeText = scorePanel.GetChild(1).GetChild(1).GetComponent<Text>();
        bottleCountText = scorePanel.GetChild(2).GetChild(1).GetComponent<Text>();

        missionPanel = Find("Panel_Mission");

        tensionValueImg = Find("Image_TensionGaugeBar").GetComponent<Image>();
        tensionValueImg.fillAmount = 0.0f;

        // 아래 코루틴은 Stage Data를 참조하는데, Stage Data는 Awake에 할당되므로 그 이전에 실행하면 안 됨
        StartCoroutine(UpdateScoreTexts());
    }

    // Find object using transform
    public static Transform Find(string uiName)
    {
        return uis[uiName];
    }

    private static void RecursiveRegisterChild(Transform parent, Dictionary<string, Transform> dict)
    {
        if (!dict.ContainsKey(parent.name)) dict.Add(parent.name, parent);
        foreach (Transform child in parent) RecursiveRegisterChild(child, dict);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*=================<Update texts of score panel>================================*/

    private IEnumerator UpdateScoreTexts()
    {
        int usedBottle, totalBottle;
        float usedTime, totalTime;

        totalBottle = StageGameManager.Instance.StageData.LimitedBottleNumber;
        totalTime = StageGameManager.Instance.StageData.LimitedTime;

        while (true)
        {
            usedBottle = StageGameManager.Instance.UsedBottleNum;
            usedTime = StageGameManager.Instance.UsedTime;

            UpdateBottleText(usedBottle, totalBottle);
            UpdateTimeText(usedTime, totalTime);

            yield return new WaitForFixedUpdate();
        }
    }   

    // Update time text
    public void UpdateTimeText(float used, float total)
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
    public void UpdateBottleText(int used, int total)
    {
        bottleCountText.text = used + "/" + total;
    }

    // Update tension gauge
    public void UpdateTensionGauge(float tensionValue)
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

    /*================================<Callback Methods>================================*/

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
        // (오류) 재시작 이후 일시정지 버튼 누르면 할당이 안 되어 있음.
        // 현재 방식으로는 rect transform을 찾을 수 없는 문제가 있음
        // 아래와 같은 메서드로는 스크립트가 다시 실행되지 않음. (score panel 초기화 안 됨)
        // (To Do) 또한 stage를 특정하면 확장성이 없으므로 수정해야 함.
        int index = StageGameManager.Instance.GetCurrentStageNumber();
        SceneManager.LoadScene("Stage" + "-" + index);
        StageGameManager.Instance.InitializeCurrentStageData(index);
        // (To Do) 게임 재시작을 씬을 다시 로드하는 방식 말고 변수들을 초기화하는 방식으로 할 것.
        // StageGameManager.InitializeStage();
    }

    // Go to select stage
    public void GoToSelectScene()
    {
        // (오류) SelectStage 빌드 안 되어 있음.
        SceneManager.LoadScene("SelectStage");
    }

    //ui
}
