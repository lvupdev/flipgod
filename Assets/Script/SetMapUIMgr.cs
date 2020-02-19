using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetMapUIMgr : MonoBehaviour
{
    public void ReturnToLobby()
    {
        SceneManager.LoadScene("Start");
    }
}
