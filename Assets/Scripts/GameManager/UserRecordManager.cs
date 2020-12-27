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
		public int stageNumber;
		public int usedBottleNumber;
		public float usedTime;

		public UserRecord(int stageNumber, int usedBottleNumber, float usedTime)
		{
			this.stageNumber = stageNumber;
			this.usedBottleNumber = usedBottleNumber;
			this.usedTime = usedTime;
		}
	}

	// Save data of current passed stage
	// and Return it
	public static UserRecord CreateRecord(int stageNumber, int usedBottleNumber, float usedTime)
	{
		UserRecord currentUserRecord;
		currentUserRecord.stageNumber = stageNumber;
		currentUserRecord.usedBottleNumber = usedBottleNumber;
		currentUserRecord.usedTime = usedTime;

		return currentUserRecord;
	}

	// Save data with stage number as key
	//============================[PlayerPrefs]===================================
	// key(string)   : "[int : stage number]"
	// value(string) : "[int : used bottle number]/[float : used time]"
	public static void SaveUserRecord(UserRecord userRecord)
	{
		string stageNumberKey;
		string userRecordValue;

		// In PlayerPrefs, the key and value is saved as string
		stageNumberKey = userRecord.stageNumber.ToString();
		userRecordValue = userRecord.usedBottleNumber + "/" + userRecord.usedTime;

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
		UserRecord userRecord;

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
			userRecord.stageNumber = stageNum;
			userRecord.usedBottleNumber = usedBottleNum;
			userRecord.usedTime = usedTime;
		}
		// If there is no data in stage number
		else
		{
			// then Initialize user record to zero
			userRecord = new UserRecord(0, 0, 0f);
		}

		// Return user record
		return userRecord;
	}

	// Judge whether current user record is the new record
	// and Set new Record
	public static void JudgeNewRecord(UserRecord currentUserRecord)
	{
		// Evaluate current user record
		float currentScore = EvaluateUserRecord(currentUserRecord);
		// Get current best user record
		UserRecord currentBestUserRecord = GetUserRecord(currentUserRecord.stageNumber);
		// Evaluate current best user record
		float currentBestScore = EvaluateUserRecord(currentBestUserRecord);

		// If score of current user record is larger than score of current best record
		if (currentScore > currentBestScore)
		{
			// then Set new record to current user record 
			SaveUserRecord(currentUserRecord);
		}
	}

	// Evaluate user record
	private static float EvaluateUserRecord(UserRecord userRecord)
	{
		float score = 0.0f;

		return score;
	}

	public static void SaveRecentlyPlayInformation(int stageIndexNumber, int usedBottleNumber, float usedTime)
	{
		string key = "recentlyPlayInformation";
		string value = stageIndexNumber + "/" + usedBottleNumber + "/" + usedTime;
		if (PlayerPrefs.HasKey(key) == true)
		{
			// then Remove existing data
			RemoveUserRecord(key);
		}

		PlayerPrefs.SetString(key, value);
		// Save data
		PlayerPrefs.Save();
	}

	public static UserRecord GetRecentlyPlayInformation()
	{
		UserRecord userRecord = new UserRecord(0, 0, 0f);
		string key = "recentlyPlayInformation";

		if (PlayerPrefs.HasKey(key) == true)
		{
			string userRecordStr = PlayerPrefs.GetString(key);
			// and Parse string to get value
			// value(string) : "[int : stage index number]/[int : used bottle number]/[float : used time]"
			string[] parsed = userRecordStr.Split('/');
            int stageIndexNumber = int.Parse(parsed[0]);
			int usedBottleNum = int.Parse(parsed[1]);
			float usedTime = float.Parse(parsed[2]);

            userRecord = new UserRecord(stageIndexNumber, usedBottleNum, usedTime);
		}

        return userRecord;
	}
}
