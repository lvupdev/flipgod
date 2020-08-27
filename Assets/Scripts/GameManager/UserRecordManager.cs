using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===========[ User Record Manager ]==========================
 * User Record Manager는 user의 스테이지 완료 기록을 관리합니다.
 * PlayerPrefs로 기록을 저장하며 삭제합니다.
 * 이 스크립트는 스테이지 완료 기록을 관리하기 위한 자료구조와 메서드만을 제공하며 
 * 어떤 data도 저장하지 않습니다.
 ============================================================*/

public class UserRecordManager : MonoBehaviour
{
    // This struct is used to save record of passed stage
    public struct UserRecord
    {
        public int usedBottleNum;
        public float usedTime;
    }

    // Save data of current passed stage
    // and Return it
    public static UserRecord SaveCurrentRecord(int usedBottleNumber, float usedTime)
    {
        UserRecord currentUserRecord;
        currentUserRecord.usedBottleNum = usedBottleNumber;
        currentUserRecord.usedTime = usedTime;

        return currentUserRecord;
    }

    // Save data with stage number as key
    //============================[PlayerPrefs]===================================
    // key(string)   : "[int : stage number]"
    // value(string) : "[int : used bottle number]/[float : used time]"
    public static void SaveUserRecord(int stageNumber, int usedBottleNumber, float usedTime)
    {
        string stageNumberKey;
        string userRecord;

        // In PlayerPrefs, the key and value is saved as string
        stageNumberKey = stageNumber.ToString();
        userRecord = usedBottleNumber + "/" + usedTime;

        // If the data is already saved in stage number
        if (PlayerPrefs.HasKey(stageNumberKey) == true)
        {
            // then Remove existing data
            RemoveUserRecord(stageNumberKey);
        }

        // Set new record data in stage number
        PlayerPrefs.SetString(stageNumberKey, userRecord);
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
        // Get value with stage number
        string userRecordStr = PlayerPrefs.GetString(stageNumberStr);

        // Parse string to get value
        // value(string) : "[int : used bottle number]/[float : used time]"
        string[] parsed = userRecordStr.Split('/');
        int usedBottleNum = int.Parse(parsed[0]);
        float usedTime = float.Parse(parsed[1]);

        // Format value to user record
        UserRecord userRecord;
        userRecord.usedBottleNum = usedBottleNum;
        userRecord.usedTime = usedTime;

        // and Return it
        return userRecord;
    }
    
}
