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

    void Start()
    {

    }


    public void ShowPausePanel() // 일시정지 메뉴 창 보여주는 함수
    {
        GameObject.Find("Canvas").transform.Find("Panel_Pause").gameObject.SetActive(true);
    }

    public void Retry() // 스테이지를 재시작하는 함수
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Resume() // 게임을 계속해서 진행하는 함수
    {
        GameObject.Find("Canvas").transform.Find("Panel_Pause").gameObject.SetActive(false);
    }

    public void GoStageScene() // 스테이지 선택 화면으로 넘어가는 함수
    {
        SceneManager.LoadScene("SelectStage");
    }
}
