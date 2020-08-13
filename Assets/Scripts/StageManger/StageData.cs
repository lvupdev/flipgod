using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is used for saving data about stages.
 */

[CreateAssetMenu(fileName = "StageData-", menuName = "Scriptable Object/Stage Data")]
public class StageData : ScriptableObject
{
    // number of current stage
    [SerializeField]
    private int stageNumber = 0;
    public int StageNumber { get { return stageNumber; } }

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

    [SerializeField]
    private bool isCleared = false;
    public bool IsCleared { get { return isCleared; } }
}
