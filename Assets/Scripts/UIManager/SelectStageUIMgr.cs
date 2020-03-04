using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStageUIMgr : MonoBehaviour
{
    public void ReturnToLobby()
    {
        SceneManager.LoadScene("Start");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Stage");
    }
}
