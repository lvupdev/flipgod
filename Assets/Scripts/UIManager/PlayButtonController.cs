using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButtonController : MonoBehaviour
{
    private Button button;
    private TestButtonController testButtonController;

    void Start()
    {
        button = GetComponent<Button>();
        testButtonController = transform.parent.GetComponent<TestButtonController>();

        button.onClick.AddListener(MoveToStage);
    }

    public void MoveToStage()
    {
        SceneManager.LoadScene("Stage-" + testButtonController.stageNum);
    }
}
