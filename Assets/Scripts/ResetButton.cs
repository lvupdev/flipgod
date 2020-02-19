using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
리셋버튼 
*/

public class ResetButton : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene"); //씬을 불러온다.
    }
}
