using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserRecord : MonoBehaviour
{
    public static Dictionary<int, UserRecord> userRecords = new Dictionary<int, UserRecord>();

    private int usedBottle;
    public int UsedBottle { get { return usedBottle; } set { usedBottle = value; } }

    private float usedTime;
    public float UsedTime { get { return usedTime; } set { usedTime = value; } }

    private int achivedGoalNum;
    public int AchivedGoalNum { get { return achivedGoalNum; } set { achivedGoalNum = value; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void AddUserRecord(int stageNumber, int usedBottle, float usedTime, int achivedGoalNum)
    {
        UserRecord userRecord = new UserRecord();

        userRecord.UsedBottle = usedBottle;
        userRecord.UsedTime = usedTime;
        userRecord.AchivedGoalNum = achivedGoalNum;

        userRecords.Add(stageNumber, userRecord);
    }

    public static void RemoveAllUserRecord()
    {
        userRecords.Clear();
    }
}
