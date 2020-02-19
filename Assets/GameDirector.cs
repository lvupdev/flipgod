using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPauseWindow()
    {
        GameObject.Find("UICanvas").transform.Find("PauseWindow").gameObject.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene("Stage");
    }

    public void Resume()
    {
        GameObject.Find("UICanvas").transform.Find("PauseWindow").gameObject.SetActive(false);
    }

    public void GoStageScene()
    {
        SceneManager.LoadScene("SelectStage");
    }

}
