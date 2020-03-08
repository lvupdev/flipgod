using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUIMgr : MonoBehaviour
{
    public GameObject missionPanel;
    public Text coinText;
    public Text timeText;
    public Text bottleText;

    private static StageUIMgr m_instance;

    public static StageUIMgr instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<StageUIMgr>();

            return m_instance;
        }
    }

    public void CloseMissionWindow()
    {
        missionPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void UpdateCoinText(int remain)
    {
        coinText.text = remain.ToString();
    }

    public void UpdateTimeText(float limitedTime)
    {
        int minute, second;
        second = Mathf.FloorToInt(limitedTime);
        minute = second / 60;
        second = second % 60;
        timeText.text = minute + " : " + second;
    }

    public void UpdateBottleText(int remain, int total)
    {
        bottleText.text = remain + " / " + total;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
