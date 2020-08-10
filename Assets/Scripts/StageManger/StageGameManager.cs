using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * This script handles every methods and variables contained in each StageManagers.
 */
public class StageGameManager : MonoBehaviour
{
    // Static instance of StageGameManager
    private static StageGameManager instance;
    public static StageGameManager Instance { get { return instance; } }


    // Container of stage data (initial value)
    [SerializeField]
    private StageData stageData;
    public StageData StageData { get { return stageData; } }

    // Get number of current stage
    // and Set current stage data using that value
    public static void InitializeCurrentStageData(int currentStageNumber)
    {
        /*=======Set Stage Data about stage========
         * name of Stage Data: StageData-*
         * * is the number of stage
         */
        instance.stageData = Resources.Load<StageData>("StageDatas/StageData-"+currentStageNumber);
    }

    // value about used resource in current Stage 
    private int usedBottleNum;
    public int UsedBottleNum { get { return usedBottleNum; } }

    private float usedTime;
    public float UsedTime { get { return usedTime; } }

    // Start is called before the first frame update

    private void Awake()
    {
        //======Init static instance of StageGameManager=========
        // If instance already exists, then destroy this
        // if not, set instance to this
        if (instance = null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        InitializeCurrentStageData(GetCurrentStageNumber());
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    // Get number of current stage
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

    // (To Do) 이런 함수는 GameManager 같은 것을 따로 두어야 할 것 같다.
    // (To Do) 매개변수 이름이 현재 스크립트 내의 멤버 변수와 이름이 겹쳐서 애매함.
    public static void InitializeStage(int limitedBottleNum, int limitedTime)
    {
        // To do something

        // Initialize variable about mission 
        // Initialize variable about tension value

    }

    // Increase a timer value every second
    public static void Timer()
    {
        if (instance.usedTime < instance.StageData.LimitedTime)
        {
            instance.usedTime += Time.deltaTime;
        }
    }

    // Increase number of used bottle 
    public static void CountUsedBottle()
    {
        if (instance.usedBottleNum < instance.StageData.LimitedBottleNumber)
        {
            instance.usedBottleNum += 1;
        }
    }

    
}
