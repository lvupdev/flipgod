using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * This script manage UI in Stage Clear scene
 */

public class StageClearUIManager : MonoBehaviour
{
    //================[Static Field]=====================================
    // Static instance of StageGameManager
    private static StageClearUIManager instance;
    public static StageClearUIManager Instance { get { return instance; } }

    private static Dictionary<string, Transform> uis = new Dictionary<string, Transform>();
    //===================================================================

	// a Transform of canvas of Stage Clear scene
	public Transform canvasTransform;

	// a Text that shows the passed test
	private Text testTitle;

    //=======Texts of record panel========================================
    private Text missionContent;
    private Text timeRecord;
    private Text bottleCountRecord;
    //====================================================================

    private Button resumeButton;
    private Button retryButton;
    private Button selectStageButton;
    private List<Button> commentButtonList;

    // Comment Section
    private Text comment;
    private Image commentLabel;

	// stage data of passed stage
	private StageData stageData;

    // user record of passed stage
    public UserRecord userRecord;

    public Sprite[] commentSprites;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        int stageNum    = ResourceManager.Instance.RecentlyTriedStage;
        stageData       = Resources.Load<StageData>("StageDatas/StageData-" + stageNum);
        userRecord      = UserRecordManager.GetUserRecord(stageNum);

        commentButtonList = new List<Button>();

        // Initialize ui
		uis.Clear();
		RecursiveRegisterChild(canvasTransform, uis);

        testTitle           = Find("Text_TestResult").GetComponent<Text>();
        missionContent      = Find("Text_Mission").GetComponent<Text>();
        timeRecord          = Find("Text_TimeRecord").GetComponent<Text>();
        bottleCountRecord   = Find("Text_BottleRecord").GetComponent<Text>();
        comment             = Find("Text_CommentContent").GetComponent<Text>();
        commentLabel        = Find("Image_CommentLabel").GetComponent<Image>();

        resumeButton        = Find("Button_Resume").GetComponent<Button>();
        retryButton         = Find("Button_Retry").GetComponent<Button>();
        selectStageButton   = Find("Button_SelectStage").GetComponent<Button>();

        resumeButton.onClick.AddListener(MoveToNextStage);
        retryButton.onClick.AddListener(RetryStage);
        selectStageButton.onClick.AddListener(GoToSelectScene);

        int i = 0;
        foreach (Button button in Find("Panel_CommentButtons").GetComponentsInChildren<Button>())
        {
            int idx = i++;
            commentButtonList.Add(button);
            button.onClick.AddListener(()=>OnClickCommentButton(idx));
        }

        SetRecordTexts();
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

    // Set texts of Record
    public void SetRecordTexts()
    {
        testTitle.text          = stageData.StageIndexNumber + "#" + " " + "RESULT";
        missionContent.text     = MissionUIFunction.FormatMissionContent(stageData.MissionType, stageData.GoalNumber);
        timeRecord.text         = MissionUIFunction.FormatTimeText(userRecord.UsedTime) + "/" + MissionUIFunction.FormatTimeText(stageData.LimitedTime);
        bottleCountRecord.text  = MissionUIFunction.FormatBottleText(userRecord.UsedBottleNumber,stageData.LimitedBottleNumber);
        comment.text            = stageData.Comment[0];
    }

    private void SetCommentButtons()
    {
        
    }

	//================[Call-back Method]=====================================

    // Show comment of clicked character
    public void OnClickCommentButton(int index)
    {
        comment.text = stageData.Comment[index];
        commentLabel.sprite = commentSprites[index];
    }

	public void MoveToNextStage()
	{
        int nextStageNum = stageData.StageIndexNumber + 1;
        SceneManager.LoadScene("Stage" + "-" + nextStageNum);
	}

	public void RetryStage()
	{
		SceneManager.LoadScene("Stage" + "-" + stageData.StageIndexNumber);
	}

	public void GoToSelectScene()
	{
		SceneManager.LoadScene("SelectStage");
	}
    //=======================================================================
}
