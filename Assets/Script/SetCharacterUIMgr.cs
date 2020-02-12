using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetCharacterUIMgr : MonoBehaviour
{
    public GameObject profilePanel;
    public GameObject storyPanel;
    public GameObject upgradePanel;

    private void Awake()
    {
        profilePanel.SetActive(true);
        upgradePanel.SetActive(false);
        storyPanel.SetActive(false);
    }

    public void ReturnToLobby()
    {
        SceneManager.LoadScene("Start");
    }

    public void ShowStory()
    {
        profilePanel.SetActive(false);
        storyPanel.SetActive(true);
    }

    public void ShowProfile()
    {
        profilePanel.SetActive(true);
        storyPanel.SetActive(false);
    }
}
