using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using static UserRecordManager;
using static MissionUIFunction;
/*
* 스테이지 선택 버튼을 관리하는 스크립트입니다
*/
public class TestButtonController : MonoBehaviour
{
    public Sprite defaultImage; //선택 전 이미지
    public Sprite stretchedImage; //선택 후 이미지

    private const float defaultHeight = 140; // 선택 전 버튼의 기본 높이
    private const float defaultWidth = 830; // 선택 전 버튼의 기본 넓이
    private const float stretchedPadding = 60; // 버튼이 늘어났을 때 ScrollView와 갖는 padding크기

    private RectTransform rect_ScrollView;
    private RectTransform rect_Button;
    private Button testButton;
    private Slider slider;
    private Image image;
    private int stageNum; //연결되는 스테이지 번호
    private float speed; //버튼 사이즈 변경 속도
    private float colorSpeed; //버튼 색깔 변경 속도
    private bool isSelected; //이 버튼이 선택되었는지의 여부
    private bool isStretching; //버튼이 늘어나고 있는지의 여부
    private bool isShrinking; // 버튼이 줄어들고 있는지의 여부

    //자식 오브젝트
    private Text testNum;
    private Image missionImage;
    private Text missionText;
    private Image timeImage;
    private Text timeText;
    private Image bottleNumImage;
    private Text bottleNumText;
    private Image playImage;
    private Image commentImage;
    private Button playButton;

    // Start is called before the first frame update
    private void Start()
    {
        rect_ScrollView = GameObject.Find("Scroll View").GetComponent<RectTransform>();
        rect_Button = GetComponent<RectTransform>();
        slider = transform.parent.GetComponent<Slider>();
        testButton = GetComponent<Button>();
        image = GetComponent<Image>();
        testNum = transform.Find("Text_TestNum").GetComponent<Text>();
        missionImage = transform.Find("Image_Mission").GetComponent<Image>();
        missionText = transform.Find("Text_Mission").GetComponent<Text>();
        timeImage = transform.Find("Image_Time").GetComponent<Image>();
        timeText = transform.Find("Text_Time").GetComponent<Text>();
        bottleNumImage = transform.Find("Image_BottleNum").GetComponent<Image>();
        bottleNumText = transform.Find("Text_BottleNum").GetComponent<Text>();
        playImage = transform.Find("Button_Play").GetComponent<Image>();
        commentImage = transform.Find("Button_Comment").GetComponent<Image>();
        playButton = transform.Find("Button_Play").GetComponent<Button>();
        
        testButton.onClick.AddListener(ResizeTestButton);
        playButton.onClick.AddListener(MoveToStage);

        speed = 10;
        colorSpeed = 4;
        isSelected = false;
        isStretching = false;
        isShrinking = false;

        for(int i = 0; i < transform.parent.childCount; i++)
        {
            if(transform.parent.GetChild(i) == gameObject.transform)
            {
                stageNum = i + 1;
                break;
            } 
        }

        SetTestInfo();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isShrinking)
        {
            rect_Button.sizeDelta = Vector2.Lerp(rect_Button.sizeDelta, new Vector2(defaultWidth, defaultHeight), Time.deltaTime * speed);


            //버튼 자식 오브제트 투명도 조절
            if (missionImage.color.a > 0) 
            {
                missionImage.color -= new Color(0, 0, 0, Time.deltaTime * 3f * colorSpeed);
                missionText.color -= new Color(0, 0, 0, Time.deltaTime * 3f * colorSpeed);
                timeImage.color -= new Color(0, 0, 0, Time.deltaTime * 3f * colorSpeed);
                timeText.color -= new Color(0, 0, 0, Time.deltaTime * 3f * colorSpeed);
                bottleNumImage.color -= new Color(0, 0, 0, Time.deltaTime * 3f * colorSpeed);
                bottleNumText.color -= new Color(0, 0, 0, Time.deltaTime * 3f * colorSpeed);
                playImage.color -= new Color(0, 0, 0, Time.deltaTime * 3f * colorSpeed);
                commentImage.color -= new Color(0, 0, 0, Time.deltaTime * 3f * colorSpeed);
            }

            if (rect_Button.sizeDelta.y < 141)
            {
                rect_Button.sizeDelta = new Vector2(defaultWidth, defaultHeight);
                isShrinking = false;
                for (int i = 1; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        if (isStretching)
        {
            rect_Button.sizeDelta = Vector2.Lerp(rect_Button.sizeDelta, new Vector2(defaultWidth, rect_ScrollView.rect.height - stretchedPadding), Time.deltaTime * speed);

            //버튼 자식 오브제트 투명도 조절
            if(missionImage.color.a < 1)
            {
                missionImage.color += new Color(0, 0, 0, Time.deltaTime * colorSpeed);
                missionText.color += new Color(0, 0, 0, Time.deltaTime * colorSpeed);
                timeImage.color += new Color(0, 0, 0, Time.deltaTime * colorSpeed);
                timeText.color += new Color(0, 0, 0, Time.deltaTime * colorSpeed);
                bottleNumImage.color += new Color(0, 0, 0, Time.deltaTime * colorSpeed);
                bottleNumText.color += new Color(0, 0, 0, Time.deltaTime * colorSpeed);
                playImage.color += new Color(0, 0, 0, Time.deltaTime * colorSpeed);
                commentImage.color += new Color(0, 0, 0, Time.deltaTime * colorSpeed);
            }

            if (rect_Button.sizeDelta.y > rect_ScrollView.rect.height - stretchedPadding - 1)
            {
                rect_Button.sizeDelta = new Vector2(defaultWidth, rect_ScrollView.rect.height - stretchedPadding);
                isStretching = false;
            }
        }
    }


    private void SetTestInfo()
    {
        StageData stageData     = Resources.Load<StageData>( "StageDatas/StageData-" + stageNum );
        UserRecord userRecord   = GetUserRecord(stageNum);
        testNum.text            = "TEST#" + stageNum;
        missionText.text        = FormatMissionContent( stageData.MissionIndexNumber, stageData.GoalNumber );

        if ( null == userRecord) // Stage which is not cleared yet
        {
            timeText.text       = "- / " + FormatTimeText( stageData.LimitedTime );
            bottleNumText.text  = "- / " + stageData.LimitedBottleNumber;
        }
        else
        {
            timeText.text       = FormatTimeText( userRecord.UsedTime ) + " / " + FormatTimeText( stageData.LimitedTime );
            bottleNumText.text  = FormatBottleText( userRecord.UsedBottleNumber, stageData.LimitedBottleNumber );
        }
    }

    private void ResizeTestButton()
    {
        if (!isStretching && !isShrinking) //사이즈 변경 중의 중복 클릭 방지
        {
            if (isSelected) // 선택을 취소하는 경우
            {
                isSelected = false;
                isShrinking = true;
                image.sprite = defaultImage;
            }
            else // 선택한 경우
            {
                isSelected = true;
                isStretching = true;
                image.sprite = stretchedImage;

                for (int i = 1; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            slider.SetPadding(isSelected, stageNum);
        }
    }

    private void MoveToStage()
    {
        SceneManager.LoadScene("Stage-" + stageNum);
    }
}
