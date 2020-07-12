using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 염동력자, 빙결능력자 필살기 사용시 물병 클릭과 관련된 상호작용을 관리하는 스크립트입니다.
 */
public class BottleSkillOperation : MonoBehaviour
{
    private PlayerImageController playerImageController;
    private GameResourceValue gameResourceValue;
    private SkillButton skillButton;
    private GameObject redAura;
    private GameObject freezeRange;
    public static int usingSkillNum = 0; // 초능력을 사용한 횟수;

    // Start is called before the first frame update
    void Start()
    {
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        gameResourceValue = GameObject.Find("GameResourceValue").GetComponent<GameResourceValue>();
        skillButton = GameObject.Find("Button_Skill").GetComponent<SkillButton>();
        redAura = this.transform.GetChild(0).gameObject;
        freezeRange = this.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() //물병 콜라이더가 터치되었을 때
    {
        if(playerImageController.GetPlayingChr() == 0 && skillButton.getUsingSkill())
        {
            if (!redAura.activeSelf && usingSkillNum < gameResourceValue.GetSkillLV(0)) //오러가 꺼져 있고 오러가 켜져있는 물병의 개수가 업그레이드 수 미만일 때
            {
                redAura.SetActive(true);
                usingSkillNum++;
            }
            else if(redAura.activeSelf) //오러가 켜져있을 때
            {
                redAura.SetActive(false);
                usingSkillNum--;
            }
        }
    }
}
