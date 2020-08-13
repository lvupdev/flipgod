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
    // a Transform of canvas of Stage Clear scene
    public Transform canvasTransform;

    // a Text that shows the passed test
    private Text testTitle;

    //=======Texts of record panel===================
    private Transform recordPanel;
    private Text missionContent;
    private Text timeRecord;
    private Text bottleCountRecord;

    // a Text that shows commets of characters
    private Text comment;

    // TO DO : private
    // stage data of passed stage
    public StageData stageData;

    // user record of passed stage
    private UserRecord userRecord;

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

    private void SetRecordTexts()
    {

    }

    // Show comment of clicked character
    public void OnClickCommentButton(int index)
    {
        comment.text = stageData.Comment[index];
    }

    public void GoSelectStageScene()
    {

    }
}
