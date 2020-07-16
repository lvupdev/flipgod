using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUIManager : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject btnPanel;
    private bool inGame;

    private void Awake()
    {
        inGame = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && inGame == false)
        {
            titlePanel.SetActive(false);
            btnPanel.SetActive(true);
            inGame = true;
        }
    }

    public void ToSetMap()
    {
        SceneManager.LoadScene("SetMap");
    }

    public void ToSelectStage()
    {
        SceneManager.LoadScene("SelectStage");
    }

    public void ToSetCharacter()
    {
        SceneManager.LoadScene("SetCharacter");
    }
}
