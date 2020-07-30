using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * This script handles every methods and variables contained in each StageManagers.
 */
public class StageGameManager : MonoBehaviour
{
    // This class is to save value about stage information
    class StageInformation
    {
        public int limitedBottleNum;
        public float limitedTime;

        public StageInformation(int limitedBottleNum, float limitedTime)
        {
            this.limitedBottleNum = limitedBottleNum;
            this.limitedTime = limitedTime;
        }
    }

    // All value about mission of every stage   
    // (TO DO) stage numa만큼 생성해야 함
    private static StageInformation[] stageInformations = new StageInformation[10];

    // (TO DO)
    // bottle 3/3 -> 0/3 +++
    // time 도 위와 같이
    // Variable about mission of current stage
    // 1. limited bottle num
    // 2. limited time
    // 3. remaining bottle num : <limited bottle num> - <used bottle num> 
    public static int limitedBottleNum;
    public static float limitedTime;
    public static int remainingBottleNum;

    // Variable about goods to be gained
    public static int gainedCoinNum;

    // Start is called before the first frame update
    void Start()
    {
        SetStageInformations();
        SetCurrentStageInformationDefault(GetCurrentStageNumber());
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    //Initialize stage informations
    public void SetStageInformations()
    {
        for (int i = 0; i < stageInformations.Length; i++)
            stageInformations[i] = new StageInformation(3, 30.0f);
    }

    public static int GetCurrentStageNumber()
    {
        // scene name (stage name) : "Stage-<Stage num>"
        // Get current scene name
        string sceneName = SceneManager.GetActiveScene().name;
        string[] parsed = sceneName.Split('-');
        int stageNum = int.Parse(parsed[1]);

        // Return current stage number
        return stageNum;
    }

    // Initialize value of mission variable of current stage
    public static void SetCurrentStageInformationDefault(int stageNum)
    {
        // Initialize value using stage number
        limitedBottleNum = stageInformations[stageNum].limitedBottleNum;
        limitedTime = stageInformations[stageNum].limitedTime;
        remainingBottleNum = limitedBottleNum;
        gainedCoinNum = 0;
    }

    // (To Do) 이런 함수는 GameManager 같은 것을 따로 두어야 할 것 같다.
    // (To Do) 매개변수 이름이 현재 스크립트 내의 멤버 변수와 이름이 겹쳐서 애매함.
    public static void InitializeStage(int limitedBottleNum, int limitedTime)
    {
        // To do something

        // Initialize variable about mission 
        // Initialize variable about gained coin
        /*=========<(논의할 것) 얻은 coin도 없어지도록?>====================
         * 1. gained coin num은 stage를 완료한 후 user property에 반영한다.
         * 2. gained coin num은 얻는 즉시 user property에 반영한다.
         =================================================================*/
        // Initialize variable about tension value

        SetCurrentStageInformationDefault(GetCurrentStageNumber());
    }

    // Decrease a timer value every second
    public static void Timer()
    {
        if (limitedTime > 0)
        {
            limitedTime -= Time.deltaTime;
        }
    }

    // Increase gained coin num 
    public static void AddCoin()
    {
        gainedCoinNum += 1;
    }

    // Decrease remaining bottle num 
    public static void CountUsedBottle()
    {
        if (remainingBottleNum > 0)
        {
            remainingBottleNum -= 1;
        }
    }

    
}
