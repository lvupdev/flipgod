using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is used for saving data about stages.
 */

[CreateAssetMenu(fileName = "StageData-", menuName = "Scriptable Object/Stage Data")]
public class StageData : ScriptableObject
{
    // index number of current stage. start from 1.
    [SerializeField]
    private int stageIndexNumber = 0;
    public int StageIndexNumber { get { return stageIndexNumber; } }

    // number of limited bottle
    [SerializeField]
    private int limitedBottleNumber = 0;
    public int LimitedBottleNumber { get { return limitedBottleNumber; } }

    // number of limitedTime
    [SerializeField]
    private float limitedTime = 0.0f;
    public float LimitedTime { get { return limitedTime; } }

    /*====comments to be shown When stage cleared======
     * 0 : 강한 ()
     * 1 : 이시우 (탄성막)
     * 2 : 백하얀
     */
    [SerializeField]
    private string[] comment = new string[3];
    public string[] Comment { get { return comment; } }

    /*==============index number of mission=============
     *                              index number
     * (1) 물병 n개 세우기            0
     * (2) 어떤 물체 얼리기           1
     * (3) 특정 트리거 발동시키기     2
     * (4) 특정 발판 위에 세우기      3
     */
    //[SerializeField]
    //private int missionIndexNumber = 0;
    //public int MissionIndexNumber { get { return missionIndexNumber; } }

    public enum Mission
    {
        StandBottle,                // 물병 n개 세우기
        FreezeTarget,               // 어떤 물체 얼리기
        ActivateTrigger,            // 특정 트리거 발동시키기
        StandBottleOnTheTarget,     // 특정 발판 위에 세우기

        MissionNum
    }

    [SerializeField]
    private Mission missionType = Mission.MissionNum;
    public Mission MissionType { get { return missionType; } }


    // number of goal
    [SerializeField]
    private int goalNumber = 0;
    public int GoalNumber { get { return goalNumber; } }

}