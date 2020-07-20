using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * ControllButtons에 속하는 버튼들이 사라지고 나타나는 효과를 관리하는 스크립트입니다.
 */
public class ControllButtonsUIManager : MonoBehaviour
{
    private bool hideButtons;
    private bool showButtons;
    
    // Start is called before the first frame update
    void Start()
    {
        hideButtons = false;
        showButtons = false;
    }

    public void setHideButtons(bool value) { hideButtons = value; }
    public void setShowButtons(bool value) { showButtons = value; }

    // Update is called once per frame
    void Update()
    {
        if (hideButtons)
        {

        }

        if (showButtons)
        {

        }
    }

}
