﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionUIFunction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*==============index number of mission=============
    *                              index number
    * (1) 물병 n개 세우기            0
    * (2) 어떤 물체 얼리기           1
    * (3) 특정 트리거 발동시키기     2
    * (4) 특정 발판 위에 세우기      3
    */
    // Return mission content using index number
    public static string FormatMissionContent(int missionIndexNumber, int targetNumber)
    {
        string missionContent;

        switch (missionIndexNumber)
        {
            case 0:
                missionContent = "물병 " + targetNumber + "개 세우기";
                break;
            case 1:
                missionContent = "특정 물체" + targetNumber + "개 얼리기";
                break;
            case 2:
                missionContent = "특정 트리거 " + targetNumber + "개 발동시키기";
                break;
            case 3:
                missionContent = "특정 발판 위에 물병 " + targetNumber + "개 발동시키기";
                break;
            default:
                missionContent = "지정 미션이 없습니다";
                break;
        }

        return missionContent;
    }

    // Convert value to target string format
    public static string FormatTargetText(int complete, int total)
    {
        string targetRecord = complete + "/" + total;

        return targetRecord;
    }

    // Convert value to time string format
    public static string FormatTimeText(float floatValue)
    {
        string timeRecord;
        int minute, second;

        second = Mathf.FloorToInt(floatValue);
        minute = second / 60;
        second = second % 60;
        timeRecord = minute + ":" + second;

        return timeRecord;
    }

    // Convert value to bottle string format
    public static string FormatBottleText(int used, int total)
    {
        string bottleRecord = used + "/" + total;

        return bottleRecord;
    }
}
