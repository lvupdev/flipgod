using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
/*
* ControllButtons에 속하는 버튼들이 사라지고 나타나는 효과를 관리하는 스크립트입니다.
*/
public class ControllButtonsUIManager : MonoBehaviour
{
    private bool hideButtons; //버튼을 숨기고 싶을 때 값을 true로 한다.
    private bool showButtons; //버튼을 보이고 싶을 때 값을 true로 한다. 
    private float canvasHeightHalf; //Canvas height 값 절반
    private int whichCase; //값이 0이면 물병을 날리는 경우, 값이 1이면 염력/빙결 필살기를 사용하는 경우, 값이 2이면 탄성막 필살기를 사용하는 경우이다.
    private float delta; //시간 변수
    private float[] originalPositionofButtons = new float[7]; //각 버튼들의 원래 위치를 저장하는 배열
    private GameObject controllButtons; //숨시고 표시하고 싶은 오브젝트들의 부모 오브젝느
    
    // Start is called before the first frame update
    void Start()
    {
        hideButtons = false;
        showButtons = false;
        delta = 0;
        whichCase = 0;
        canvasHeightHalf = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.y / 2;
        controllButtons = GameObject.Find("ControllButtons");

        //각 버튼들의 원래 위치 저장
        for (int i = 0; i < controllButtons.transform.childCount; i++)
        {
            originalPositionofButtons[i] = controllButtons.transform.GetChild(i).gameObject.GetComponent<RectTransform>().localPosition.y;
        }
    }

    public void setShowButtonsWithDelay(float delay, int whichCase)
    {
        Invoke("setShowButtons", delay);
        this.whichCase = whichCase;
    }


    public void setHideButtons(bool value, int whichCase) 
    {
        hideButtons = value;
        this.whichCase = whichCase;
    }
    public void setShowButtons(bool value, int whichCase) 
    { 
        showButtons = value;
        this.whichCase = whichCase;
    }
    public void setShowButtons()
    {
        showButtons = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hideButtons)
        {
            delta += Time.fixedDeltaTime;

            switch (whichCase)
            {
                case 0: //물병을 던졌을 때
                    if(delta < 0.5f) //1초 동안 버튼들을 화면 아래로 내린다. 내려가는 속도는 처음에는 빠르고 갈수록 느려진다.
                    {
                        for (int i = 0; i < controllButtons.transform.childCount - 2; i++)
                        {
                            controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition =
                                new Vector2(controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x, 4 * canvasHeightHalf * (float)Math.Pow((delta - 0.5f), 2) + originalPositionofButtons[i] - canvasHeightHalf);
                        }
                    }
                    else
                    {
                        delta = 0;
                        hideButtons = false;
                    }
                    break;
                case 1: //염력, 빙결 필살기를 사용했을 때
                    if (delta < 0.5f) //1초 동안 버튼들을 화면 아래로 내린다. 내려가는 속도는 처음에는 빠르고 갈수록 느려진다.
                    {
                        for (int i = 1; i < controllButtons.transform.childCount - 2; i++)
                        {
                            controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition =
                                new Vector2(controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x, 4 * canvasHeightHalf * (float)Math.Pow((delta - 0.5f), 2) + originalPositionofButtons[i] - canvasHeightHalf);
                        }
                    }
                    else
                    {
                        delta = 0;
                        hideButtons = false;
                    }
                    break;
                case 2: //탄성막 필살기를 발동했을 때
                    if (delta < 0.5f)
                    {
                        for (int i = 1; i < controllButtons.transform.childCount - 3; i++)//1초 동안 버튼들을 화면 아래로 내린다. 내려가는 속도는 처음에는 빠르고 갈수록 느려진다.
                        {
                            controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition =
                                new Vector2(controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x, 4 * canvasHeightHalf * (float)Math.Pow((delta - 0.5f), 2) + originalPositionofButtons[i] - canvasHeightHalf);
                        }
                        for(int i = 5; i < controllButtons.transform.childCount; i++) //1초 동안 버튼들을 화면 쪽으로 올린다. 올라가는 속도는 처음에는 빠르고 갈수록 느려진다.
                        {
                            controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition =
                                new Vector2(controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x, (-4) * canvasHeightHalf * (float)Math.Pow((delta - 0.5f), 2) + originalPositionofButtons[i] + canvasHeightHalf);
                        }
                    }
                    else
                    {
                        delta = 0;
                        hideButtons = false;
                    }
                    break;
            }
        }
        else if (showButtons)
        {
            delta += Time.fixedDeltaTime;

            switch (whichCase)
            {
                case 0: //물병을 던졌을 때의 경우
                    if (delta < 0.5f) //1초 동안 버튼들을 화면 쪽으로 올린다. 올라가는 속도는 처음에는 빠르고 갈수록 느려진다.
                    {
                        for (int i = 0; i < controllButtons.transform.childCount - 2; i++)
                        {
                            controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition =
                                new Vector2(controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x, (-4) * canvasHeightHalf * (float)Math.Pow((delta - 0.5f), 2) + originalPositionofButtons[i]);
                        }
                    }
                    else
                    {
                        delta = 0;
                        showButtons = false;
                        for(int i = 0; i < controllButtons.transform.childCount; i++) //각 버튼들을 정확한 초기 위치로 배치
                        {
                            controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition =
                                new Vector2(controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x, originalPositionofButtons[i]);
                        }
                    }
                    break;
                case 1: //염력, 빙결 필살기를 사용했을 때
                    if (delta < 0.5f) //1초 동안 버튼들을 화면 쪽으로 올린다. 올라가는 속도는 처음에는 빠르고 갈수록 느려진다.
                    {
                        for (int i = 1; i < controllButtons.transform.childCount - 2; i++)
                        {
                            controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition =
                                new Vector2(controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x, (-4) * canvasHeightHalf * (float)Math.Pow((delta - 0.5f), 2) + originalPositionofButtons[i]);
                        }
                    }
                    else
                    {
                        delta = 0;
                        showButtons = false;
                        for (int i = 0; i < controllButtons.transform.childCount; i++) //각 버튼들을 정확한 초기 위치로 배치
                        {
                            controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition =
                                new Vector2(controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x, originalPositionofButtons[i]);
                        }
                    }
                    break;
                case 2: //탄성막 필살기를 발동했을 때
                    if (delta < 0.5f) //1초 동안 버튼들을 화면 쪽으로 올린다. 올라가는 속도는 처음에는 빠르고 갈수록 느려진다.
                    {
                        for (int i = 1; i < controllButtons.transform.childCount - 3; i++)
                        {
                            controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition =
                                new Vector2(controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x, (-4) * canvasHeightHalf * (float)Math.Pow((delta - 0.5f), 2) + originalPositionofButtons[i]);
                        }
                        for (int i = 5; i < controllButtons.transform.childCount; i++) //1초 동안 버튼들을 화면 아래로 내린다. 내려가는 속도는 처음에는 빠르고 갈수록 느려진다.
                        {
                            controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition =
                                new Vector2(controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x, 4 * canvasHeightHalf * (float)Math.Pow((delta - 0.5f), 2) + originalPositionofButtons[i]);
                        }
                    }
                    else
                    {
                        delta = 0;
                        showButtons = false;
                        for (int i = 0; i < controllButtons.transform.childCount; i++) //각 버튼들을 정확한 초기 위치로 배치
                        {
                            controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition =
                                new Vector2(controllButtons.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x, originalPositionofButtons[i]);
                        }
                    }
                    break;
            }
        }
    }

}
