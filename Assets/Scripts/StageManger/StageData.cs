using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is used for saving data about stages.
 */

[CreateAssetMenu(fileName = "Stage Data", menuName = "Scriptable Object/Stage Data")]
public class StageData : ScriptableObject
{
    [SerializeField]
    private int stageNumber;
    public int StageNumber { get { return stageNumber; } }

    [SerializeField]
    private int limitedBottleNumber;
    public int LimitedBottleNumber { get { return limitedBottleNumber; } }

    [SerializeField]
    private float limitedTime;
    public float LimitedTime { get { return limitedTime; } }

    [SerializeField]
    private string[] comment;
    public string[] Comment { get { return comment; } }
}
