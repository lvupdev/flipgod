using gamemgr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This script is used for scroll view control in Select Stage Scene
 */
public class Slider : MonoBehaviour
{
    // Variable about scroll bar
    public GameObject scrollbarObj;
    private Scrollbar scrollbar;

    // Current position of scroll bar
    private float scrollPos;

    // Positions of every button
    private float[] buttonPos;

    // new!! Variables about the array of buttons
    public GameObject buttonPrefab;
    private RectTransform rect_ScrollView;
    private RectTransform rect_Button;
    private ScrollRect scrollRect_ScrollView;
    private VerticalLayoutGroup layoutGroup;

    private float shrinkRatio; // Buttons shrink with this ratio when it's not focused 

    private float speed; // Speed of changing size of buttons
    private float paddingSpeed; // Speed of changing values of padding setting
    private int clickedButtonNum; // Stage Number of the clicked button

    // Variables about shrinking or stretching
    private bool isStretching;
    private bool isShrinking;

    private void Start()
    {
        // Init
        scrollbar = scrollbarObj.GetComponent<Scrollbar>();
        rect_ScrollView = transform.parent.transform.parent.gameObject.GetComponent<RectTransform>();
        rect_Button = buttonPrefab.GetComponent<RectTransform>();
        layoutGroup = transform.GetComponent<VerticalLayoutGroup>();
        scrollRect_ScrollView = rect_ScrollView.gameObject.GetComponent<ScrollRect>();

        scrollPos = 0; //When the Scene is loaded, this locates scroll bar at the bottom
        speed = 10;
        paddingSpeed = 10;
        shrinkRatio = 0.8f;

        isShrinking = false;
        isStretching = false;

        // Place first button and last button in the center 
        layoutGroup.padding.bottom = (int)((rect_ScrollView.rect.height / 2) - (rect_Button.rect.height / 2) * shrinkRatio);
        layoutGroup.padding.top = (int)((rect_ScrollView.rect.height / 2) - (rect_Button.rect.height / 2) * shrinkRatio);
    }

    private void Update()
    {
        buttonPos = new float[transform.childCount];
        float distance = 1f / (buttonPos.Length - 1f);
        for (int i = 0; i < buttonPos.Length; i++) // Assign button position 
        {
            buttonPos[(buttonPos.Length - 1) - i] = distance * i; 
        }

        if (Input.GetMouseButton(0))
        {
            if (scrollbar.value > 1) scrollPos = 1;
            else if (scrollbar.value < 0) scrollPos = 0;
            else if (isShrinking) scrollPos = buttonPos[clickedButtonNum];
            else scrollPos = scrollbar.value;
        }
        else
        {
            for (int i = 0; i < buttonPos.Length; i++)
            {
                if ((scrollPos < buttonPos[i] + (distance / 2) && scrollPos > buttonPos[i] - (distance / 2)))
                {
                    scrollbar.value = Mathf.Lerp(scrollbar.value, buttonPos[i], Time.deltaTime * speed);
                }
            }
        }

        for (int i = 0; i < buttonPos.Length; i++)
        {
            if (scrollPos < buttonPos[i] + (distance / 2) && scrollPos > buttonPos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), Time.deltaTime * speed);
                for (int j = 0; j < buttonPos.Length; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(shrinkRatio, shrinkRatio), Time.deltaTime * speed);
                    }
                }
            }
        }

        if (isShrinking) // Set the bottom and top value of the padding setting
        {
            layoutGroup.padding.top = (int)Mathf.Lerp(layoutGroup.padding.top, 0f, Time.deltaTime * paddingSpeed);
            layoutGroup.padding.bottom = (int)Mathf.Lerp(layoutGroup.padding.bottom, 0f, Time.deltaTime * paddingSpeed);

            if (layoutGroup.padding.top == 0 && layoutGroup.padding.bottom == 0)
            {
                isShrinking = false;
            }
        }

        if (isStretching) // Set the bottom and top value of the padding setting
        {
            layoutGroup.padding.top = (int)Mathf.Lerp(layoutGroup.padding.top, (int)((rect_ScrollView.rect.height / 2) - (rect_Button.rect.height / 2) * shrinkRatio) + 10, Time.deltaTime * paddingSpeed);
            layoutGroup.padding.bottom = (int)Mathf.Lerp(layoutGroup.padding.bottom, (int)((rect_ScrollView.rect.height / 2) - (rect_Button.rect.height / 2) * shrinkRatio) + 10, Time.deltaTime * paddingSpeed);
            if (layoutGroup.padding.top > (int)((rect_ScrollView.rect.height / 2) - (rect_Button.rect.height / 2) * shrinkRatio))
            {
                isStretching = false;
                layoutGroup.padding.top = (int)((rect_ScrollView.rect.height / 2) - (rect_Button.rect.height / 2) * shrinkRatio);
                layoutGroup.padding.bottom = (int)((rect_ScrollView.rect.height / 2) - (rect_Button.rect.height / 2) * shrinkRatio);
            }
        }
    }

    public void SetPadding(bool isSelected, int stageNum) // Called when the 'stageNum'th button is clicked
    {
        if (isSelected)
        {
            clickedButtonNum = stageNum - 1;
            scrollPos = buttonPos[clickedButtonNum];
            isShrinking = true;
            scrollbar.interactable = false;
            scrollRect_ScrollView.vertical = false;
        }
        else
        {
            isStretching = true;
            scrollbar.interactable = true;
            scrollRect_ScrollView.vertical = true;
        }
    }
}
