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
    //===================================================================

    // a Transform of canvas of Stage Clear scene
    public Transform canvasTransform;

    // a Text that shows the passed test
    private Text testTitle;

    //=======Texts of record panel========================================
    private Transform recordPanel;
    private Text missionContent;
    private Text timeRecord;
    private Text bottleCountRecord;
    //====================================================================

    // a Text that shows commets of characters
    private Text comment;

    // stage data of passed stage
    public StageData stageData;

    // user record of passed stage
    public UserRecordManager.UserRecord userRecord;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        testTitle = canvasTransform.GetChild(0).GetComponent<Text>();

        recordPanel = canvasTransform.GetChild(1);
        missionContent = recordPanel.GetChild(0).GetChild(1).GetComponent<Text>();
        timeRecord = recordPanel.GetChild(1).GetChild(1).GetComponent<Text>();
        bottleCountRecord = recordPanel.GetChild(2).GetChild(1).GetComponent<Text>();

        comment = canvasTransform.GetChild(4).GetChild(2).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Set texts of Record
    public void SetRecordTexts()
    {
        testTitle.text = stageData.StageIndexNumber + "#" + " " + "RESULT";
        missionContent.text = MissionUIFunction.FormatMissionContent(stageData.MissionIndexNumber, stageData.GoalNumber);
        timeRecord.text = MissionUIFunction.FormatTimeText(userRecord.usedTime) + "/" + MissionUIFunction.FormatTimeText(stageData.LimitedTime);
        bottleCountRecord.text = MissionUIFunction.FormatBottleText(userRecord.usedBottleNumber,stageData.LimitedBottleNumber);
        comment.text = stageData.Comment[0];
    }

    //================[Call-back Method]=====================================

    // Show comment of clicked character
    public void OnClickCommentButton(int index)
    {
        comment.text = stageData.Comment[index];
    }

    public void GoSelectStageScene()
    {

    }
    //=======================================================================
}
