using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Stage의 UI를 관리하는 스크립트
 */

public class StageUIManager : MonoBehaviour
{
    // Canvas of Stage UI
    public Canvas canvas;
    // Transform of Canvas Element
    private static Dictionary<string, Transform> uis = new Dictionary<string, Transform>();

    // Elements of UI
    private static Transform pausePanel;
    private static Transform missionPanel;

    // (수정) Score 말고 다른 이름?
    // Elements of Score Panel
    private static Transform scorePanel;
    private static Text coinCountText;
    private static Text timeText;
    private static Text bottleCountText;

    // Variable about tension gague UI
    private static Image tensionValueImg;
    private float lerpSpeed = 0.5f;



    // Start is called before the first frame update
    void Start()
    {
        // Initialize ui
        RecursiveRegisterChild(canvas.transform, uis);

        pausePanel = Find("Panel_Pause");
        pausePanel.gameObject.SetActive(false);

        scorePanel = Find("Panel_Score");
        coinCountText   = scorePanel.GetChild(0).GetChild(1).GetComponent<Text>();
        timeText        = scorePanel.GetChild(1).GetChild(1).GetComponent<Text>();
        bottleCountText = scorePanel.GetChild(2).GetChild(1).GetComponent<Text>();

        missionPanel = Find("Panel_Mission");

        tensionValueImg = Find("Image_TensionValue").GetComponent<Image>();
        tensionValueImg.fillAmount = 0.0f;

        // StartCoroutine(UpdateScoreTexts());

    }

    // Find Object Using Transform
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
        UpdateCoinText(StageManager1.gainedCoinNum);
        UpdateBottleText(StageManager1.remainBottleNum, StageManager1.limitedBottleNum);
        UpdateTimeText(StageManager1.limitedTimeSec);
    }

    /*=================<Update texts of score panel>================================*/
    /*
    private IEnumerator UpdateScoreTexts()
    {
        int coin;
        int remain, total;
        float time;

        total = StageManager1.limitedBottleNum;

        while (true)
        {
            coin = StageManager1.gainedCoinNum;
            remain = StageManager1.remainBottleNum;
            time = StageManager1.limitedTimeSec;

            UpdateCoinText(coin);
            UpdateBottleText(remain, total);
            UpdateTimeText(time);
        }
    }

     */


    public void UpdateCoinText(int gainedCoin)
    {
        coinCountText.text = gainedCoin.ToString();
    }

    public void UpdateTimeText(float limitedTime)
    {
        int minute, second;
        second = Mathf.FloorToInt(limitedTime);
        minute = second / 60;
        second = second % 60;
        timeText.text = minute + " : " + second;
    }

    public void UpdateBottleText(int remain, int total)
    {
        bottleCountText.text = remain + " / " + total;
    }

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

    public void UseTensionGauge(float tensionValue)
    {
        if (tensionValue >= 0.9999f)
        {
            tensionValueImg.fillAmount =
                Mathf.Lerp(tensionValue, 0f, Time.deltaTime * lerpSpeed);
        }
        else
        {
            // Debug.Log("Tesnion Value is not enough to use superpower.");
        }
    }

    // Close mission panel with window and
    // Start game
    public void CloseMissionWindow()
    {
        // Unactivate mission panel
        missionPanel.gameObject.SetActive(false);
        // 게임 시작
        Time.timeScale = 1.0f;
    }

    // Activate pause panel with window
    public void ShowPausePanel()
    {
        // 진행 중인 게임 일시정지
        Time.timeScale = 0.0f;
        // 일시정지 메뉴 활성화
        pausePanel.gameObject.SetActive(true);
    }

    // Resume Game
    public void ResumeGame()
    {
        // 일시정지 메뉴 비활성화
        pausePanel.gameObject.SetActive(false);
        // 게임 진행
        Time.timeScale = 1;
    }

    // Retry stage
    public void RetryStage() 
    {
        // (오류) 재시작 이후 일시정지 버튼 누르면 할당이 안 되어 있음.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // 게임 재시작을 씬을 다시 로드하는 방식 말고 변수들을 초기화하는 방식으로 할 것.
        StageManager1.InitializeStage();
    }

    // Go to select stage
    public void GoToStageScene()
    {
        // (오류) SelectStage 빌드 안 되어 있음.
        SceneManager.LoadScene("SelectStage");
    }

    //ui
}
