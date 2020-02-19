using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetCharacterUIMgr : MonoBehaviour
{
    private static SetCharacterUIMgr m_instance;
    public static SetCharacterUIMgr instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SetCharacterUIMgr>();
            }
        }
    }

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ReturnToLobby()
    {
        SceneManager.LoadScene("Start");
    }



    public void SelectMenu(GameObject panel)
    {
        bool isActive = panel.activeSelf;
        if(isActive == false)
        {
            panel.SetActive(true);
            panel.transform.SetAsFirstSibling();
            
        }

    }


}
