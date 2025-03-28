using gamemgr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scroll view control for stage selection.
/// </summary>
public class Slider : MonoBehaviour
{
    public GameObject scrollbarObj;
    public GameObject buttonPrefab;

    private Scrollbar scrollbar;
    private RectTransform rect_ScrollView;
    private RectTransform rect_Button;
    private ScrollRect scrollRect_ScrollView;
    private VerticalLayoutGroup layoutGroup;

    private float[] buttonPos;
    private float scrollPos;
    private float distance;
    private const int stretchedPadding = 60; // 버튼이 늘어났을 때 ScrollView와 갖는 padding크기
    private const float shrinkRatio = 0.8f;
    private const float scaleSpeed = 10f;
    private const float paddingSpeed = 10f;
    private int clickedButtonIndex;

    private bool isStretching = false;
    private bool isShrinking = false;

    private void Start()
    {
        Time.timeScale = 1.0f;

        // Init UI references
        scrollbar = scrollbarObj.GetComponent<Scrollbar>();
        rect_ScrollView = transform.parent.parent.GetComponent<RectTransform>();
        rect_Button = buttonPrefab.GetComponent<RectTransform>();
        scrollRect_ScrollView = rect_ScrollView.GetComponent<ScrollRect>();
        layoutGroup = GetComponent<VerticalLayoutGroup>();

        // Init layout padding to center first/last buttons
        int centerPadding = GetCenterPadding();
        layoutGroup.padding.top = centerPadding;
        layoutGroup.padding.bottom = centerPadding;

        SetButtonPositions();
    }

    private void Update()
    {
        HandleScrollInput();
        AdjustButtonScales();
        AnimatePadding();
    }

    private void HandleScrollInput()
    {
        if (Input.GetMouseButton(0))
        {
            scrollPos = Mathf.Clamp(scrollbar.value, 0f, 1f);
            if (isShrinking) scrollPos = buttonPos[clickedButtonIndex];
        }
        else
        {
            for (int i = 0; i < buttonPos.Length; i++)
            {
                if (IsWithinRange(scrollPos, buttonPos[i], distance / 2))
                {
                    scrollbar.value = Mathf.Lerp(scrollbar.value, buttonPos[i], Time.deltaTime * scaleSpeed);
                    break;
                }
            }
        }
    }

    private void AdjustButtonScales()
    {
        for (int i = 0; i < buttonPos.Length; i++)
        {
            Transform button = transform.GetChild(i);
            if (IsWithinRange(scrollPos, buttonPos[i], distance / 2))
            {
                button.localScale = Vector2.Lerp(button.localScale, Vector2.one, Time.deltaTime * scaleSpeed);
            }
            else
            {
                button.localScale = Vector2.Lerp(button.localScale, Vector2.one * shrinkRatio, Time.deltaTime * scaleSpeed);
            }
        }
    }

    private void AnimatePadding()
    {
        if (isShrinking)
        {
            AnimateToPadding(stretchedPadding/2);
            if (layoutGroup.padding.top == stretchedPadding / 2 && layoutGroup.padding.bottom == stretchedPadding/2)
                isShrinking = false;
        }
        else if (isStretching)
        {
            int targetPadding = GetCenterPadding();
            AnimateToPadding(targetPadding);
            if (layoutGroup.padding.top >= targetPadding - 5)
            {
                isStretching = false;
                layoutGroup.padding.top = targetPadding;
                layoutGroup.padding.bottom = targetPadding;
            }
        }
    }

    private void AnimateToPadding(int target)
    {
        layoutGroup.padding.top = (int)Mathf.Lerp(layoutGroup.padding.top, target, Time.deltaTime * paddingSpeed);
        layoutGroup.padding.bottom = (int)Mathf.Lerp(layoutGroup.padding.bottom, target, Time.deltaTime * paddingSpeed);
    }

    private bool IsWithinRange(float value, float target, float range)
    {
        return value < target + range && value > target - range;
    }

    private int GetCenterPadding()
    {
        return (int)((rect_ScrollView.rect.height / 2) - (rect_Button.rect.height / 2));
    }

    public void SetButtonPositions()
    {
        int count = transform.childCount;
        buttonPos = new float[count];
        distance = 1f / (count - 1f);

        for (int i = 0; i < count; i++)
        {
            buttonPos[count - 1 - i] = distance * i;
        }
    }

    // Called when a stage button is clicked.
    public void SetPadding(bool isSelected, int stageNum)
    {
        if (isSelected)
        {
            clickedButtonIndex = stageNum - 1;
            scrollPos = buttonPos[clickedButtonIndex];
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
