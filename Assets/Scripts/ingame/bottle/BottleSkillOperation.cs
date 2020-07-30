using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 염동력자, 빙결능력자 필살기 사용시 물병 클릭과 관련된 상호작용을 관리하는 스크립트입니다.
 */
public class BottleSkillOperation : MonoBehaviour
{
    private PlayerImageController playerImageController;
    private ResourceManager gameResourceValue;
    private SkillButton skillButton;
    private GameObject redAura;
    private GameObject freezeRange;
    private static int usingSkillNum = 0; // 초능력을 사용하는 정도;
    private Rigidbody2D rb;

    public int getUsingSkillNum() { return usingSkillNum; }
    public void setUsingSkillNum(int num) { usingSkillNum = num; }

    // Start is called before the first frame update
    void Start()
    {
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        gameResourceValue = GameObject.Find("GameResourceValue").GetComponent<ResourceManager>();
        skillButton = GameObject.Find("Button_Skill").GetComponent<SkillButton>();
        rb = this.GetComponent<Rigidbody2D>();
        redAura = this.transform.GetChild(0).gameObject;
        freezeRange = this.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() //물병 콜라이더가 터치되었을 때
    {
        if (skillButton.getUsingSkill())
        {
            if (playerImageController.GetPlayingChr() == 0) //염동력자의 경우
            {
                if (!redAura.activeSelf && usingSkillNum < gameResourceValue.GetSkillLV(0)) //오러가 꺼져 있고 오러가 켜져있는 물병의 개수가 업그레이드 수 미만일 때
                {
                    redAura.SetActive(true);
                    usingSkillNum++;
                }
                else if (redAura.activeSelf) //오러가 켜져있을 때
                {
                    redAura.SetActive(false);
                    usingSkillNum--;
                }
            }
            else if (playerImageController.GetPlayingChr() == 1) // 탄성막 생성자의 경우
            {

            }
            else //빙결자의 경우
            {
                if (!freezeRange.activeSelf && usingSkillNum < gameResourceValue.GetSkillLV(2)) // 빙결 범위 표시기가 꺼져있고 빙결 범위 표시기가 쳐져 있는 개수가 업그레이드 수 미만일 때
                {
                    freezeRange.SetActive(true);
                    rb.AddForce(new Vector2(0.01f, 0)); //빙결 범위 이미지를 움직이지 않으면 구조물 인식이 안 됨.
                    usingSkillNum++;
                }
                else if (freezeRange.activeSelf)
                {
                    freezeRange.SetActive(false);
                    usingSkillNum--;
                }

            }
        }
    }
}
