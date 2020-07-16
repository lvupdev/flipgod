using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGameManager : MonoBehaviour
{
    public static StageGameManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<StageGameManager>();
            return m_instance;
        }
    }

    private static StageGameManager m_instance;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    int coin = 0;
    int total = 3;
    int remain;
    float limitedTime = 30;


    public void AddCoin()
    {
        coin += 1;
        StageUIMgr.instance.UpdateCoinText(coin);
    }

    public void CountUsedBottle()
    {
        if (remain > 0)
        {
            remain -= 1;
            StageUIMgr.instance.UpdateBottleText(remain, total);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        remain = total;
        StageUIMgr.instance.UpdateBottleText(remain, total);
        StageUIMgr.instance.UpdateCoinText(coin);
        StageUIMgr.instance.UpdateTimeText(limitedTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (limitedTime > 0)
        {
            StageUIMgr.instance.UpdateTimeText(limitedTime);
            limitedTime -= Time.deltaTime;
        }
    }


}
