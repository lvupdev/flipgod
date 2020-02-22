using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
게임 UI 관리 스크립트
*/
public class GameDirector : MonoBehaviour
{
    BottleController bottleController;
    SPPController sppController;
    GameObject psychokinesis;
    GameObject membraneCreater;
    GameObject freezer;
    GameObject presentPlayer;

    void Start()
    {
        bottleController = GameObject.Find("BottlePrefab").GetComponent<BottleController>();
        sppController = GameObject.Find("SuperPowerPanel").GetComponent<SPPController>();
        psychokinesis = GameObject.Find("Psychokinesis");
        //membraneCreater = GameObject.Find("MembraneCreator");
        //freezer = GameObject.Find("Freezer");
        presentPlayer = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if(bottleController.isSuperPowerAvailabe)
        {
            presentPlayer = GameObject.FindWithTag("Player");
            if(presentPlayer == psychokinesis)
            {
                sppController.
            }
        }
    }

    public void ShowPausePanel() // 일시정지 메뉴 창 보여주는 함수
    {
        GameObject.Find("UICanvas").transform.Find("PausePanel").gameObject.SetActive(true);
    }

    public void Retry() // 스테이지를 재시작하는 함수
    {
        SceneManager.LoadScene("Stage");
    }

    public void Resume() // 게임을 계속해서 진행하는 함수
    {
        GameObject.Find("UICanvas").transform.Find("PausePanel").gameObject.SetActive(false);
    }

    public void GoStageScene() // 스테이지 선택 화면으로 넘어가는 함수
    {
        SceneManager.LoadScene("SelectStage");
    }

    public void PlayerReselectBottle() // 상황에 맞춰 플레이 캐릭터가 물병을 재선택하도록 한다.
    {
        bottleController = GameObject.FindWithTag("Player").GetComponent<BottleController>();
        if (presentPlayer == psychokinesis) psychokinesis.GetComponent<KinesisController>().ReselectBottle();
        //else if (presentPlayer == membraneCreater) membraneCreater.GetComponent<CreatorController>().ReselectBottle();
        //else freezer.GetComponent<FreezerController>().ReselectBottle();
    }

}
