using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
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
}
