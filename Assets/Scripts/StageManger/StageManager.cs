using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    Dictionary<int, StageManager> stages = new Dictionary<int, StageManager>();

    private int _stageNumber;
    public int StageNumber
    {
        get { return _stageNumber; }
        set { _stageNumber = value; }
    }

    private int _limitedBottleNum;
    public int LimitedBottleNum
    {
        get { return _limitedBottleNum; }
        set { _limitedBottleNum = value; }
    }

    private float _limitedTime;
    public float LimitedTime
    {
        get { return _limitedTime; }
        set { _limitedTime = value; }
    }

    public StageManager(int stageNumber, int limitedBottleNum, float limitedTime)
    {
        this.StageNumber = stageNumber;
        this.LimitedBottleNum = limitedBottleNum;
        this.LimitedTime = limitedTime;
        
        stages.Add(this.StageNumber, this);
        Debug.Log(this.GetType().Name);
    }

    private void Start()
    {
        
    }

}
    