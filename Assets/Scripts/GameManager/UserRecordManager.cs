using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/*===========[ User Record Manager ]==========================
 * User Record Manager는 user의 스테이지 완료 기록을 관리합니다.
 * PlayerPrefs로 기록을 저장하며 삭제합니다.
 * 이 스크립트는 스테이지 완료 기록을 관리하기 위한 자료구조와 메서드만을 제공하며 
 * 어떤 data도 저장하지 않습니다.
 ============================================================*/

// This class is used to save record of passed stage
public class UserRecord
{
    public int StageNumber { get; private set; }
    public int UsedBottleNumber { get; private set; }
    public float UsedTime { get; private set; }

    public UserRecord(int stageNumber, int usedBottleNumber, float usedTime)
    {
        StageNumber         = stageNumber;
        UsedBottleNumber    = usedBottleNumber;
        UsedTime            = usedTime;
    }
}

public class UserRecordManager : MonoBehaviour
{
    // Save data with stage number as key
    //============================[PlayerPrefs]===================================
    // key(string)   : "[int : stage number]"
    // value(string) : "[int : used bottle number]/[float : used time]"
    public static void SaveUserRecord(UserRecord userRecord)
    {
        Assert.IsTrue( userRecord != null, "저장할 UserRecord는 null이면 안 됩니다." );

        string stageNumberKey;
        string userRecordValue;

        // In PlayerPrefs, the key and value is saved as string
        stageNumberKey = userRecord.StageNumber.ToString();
        userRecordValue = userRecord.UsedBottleNumber + "/" + userRecord.UsedTime;

        // If the data is already saved in stage number
        if (PlayerPrefs.HasKey(stageNumberKey) == true)
        {
            // then Remove existing data
            RemoveUserRecord(stageNumberKey);
        }

        // Set new record data in stage number
        PlayerPrefs.SetString(stageNumberKey, userRecordValue);
        // and Save data
        PlayerPrefs.Save();
    }

    // Remove record data in given stage number
    public static void RemoveUserRecord(string stageNumber)
    {
        PlayerPrefs.DeleteKey(stageNumber);
    }

    // Remove all record data
    public static void RemoveAllUserRecord()
    {
        PlayerPrefs.DeleteAll();
    }

    // Get value with stage number
    // and Parse it to return as user record
    public static UserRecord GetUserRecord(int stageNum)
    {
        // In PlayerPrefs, the key and value is saved as string
        string stageNumberStr = stageNum.ToString();
        UserRecord userRecord = null;
        
        // If there is the saved data in stage number
        if (PlayerPrefs.HasKey(stageNumberStr) == true)
        {
            // then Get value with stage number
            string userRecordStr = PlayerPrefs.GetString(stageNumberStr);
            // and Parse string to get value
            // value(string) : "[int : used bottle number]/[float : used time]"
            string[] parsed = userRecordStr.Split('/');
            int usedBottleNum = int.Parse(parsed[0]);
            float usedTime = float.Parse(parsed[1]);

            // Set user record with value
            userRecord = new UserRecord(stageNum, usedBottleNum, usedTime);
        }

        // Return user record
        return userRecord;
    }
    
    // Judge whether current user record is the new record
    // and Set new Record
    public static void JudgeNewRecord(int stageNum, int usedBottleNumber, float usedTime)
    {
        UserRecord currentBestUserRecord = GetUserRecord(stageNum);
        int newUsedBottleNum = usedBottleNumber;
        float newUsedTime = usedTime;

        if ( null != currentBestUserRecord )
        {
            newUsedBottleNum = newUsedBottleNum < currentBestUserRecord.UsedBottleNumber ? newUsedBottleNum : currentBestUserRecord.UsedBottleNumber;
            newUsedTime = newUsedTime < currentBestUserRecord.UsedTime ? newUsedTime : currentBestUserRecord.UsedTime;
        }

        SaveUserRecord(new UserRecord(stageNum, newUsedBottleNum, newUsedTime));
    }
}
