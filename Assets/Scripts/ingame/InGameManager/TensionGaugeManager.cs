using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class TensionGaugeManager : MonoBehaviour
{
    /* =====<시스템 기획 -텐션 게이지>==================================================================================
     * 각 캐릭터는 각자 고유 필살기를 사용할 수 있는데, 필살기를 쓰기 위해서는 텐션 게이지를 소모해야 한다. 
     * 필살기는 텐션 게이지가 꽉 찼을 때 발동할 수 있으며, 텐션 게이지는 세 명의 캐릭터가 공유한다. 
     * 텐션 게이지는 기본적으로 물병을 하나 던질 때 마다 10%씩 차며, 물병을 세울 경우 추가로 10%가 차오른다.  
     * 콤보(연속으로 물병을 세우는 것)를 달성하면 물병이 세워질 때 (10%) X (콤보 수) 만큼의 텐션 게이지가 차오른다. 
     * 그 외에도 트리거를 발동시키면 10%, 동적 장애물을 빙결시키면 (10%) X (빙결된 동적 장애물 개수) 만큼 텐션 게이지가 증가한다.
     * 
     * 
     * (1) 물병을 하나 던질 때마다              +10%
     * (2) 물병을 세우는 경우                  +10% * 콤보수
     * (3) 동적 장애물/트리거를 빙결시키는 경우        +10% * 빙결된 동적 장애물 개수
     * (4) 트리거의 발동                       +10%
     =================================================================================================*/

    private GameObject tensionGaugeBar; //텐션게이지 이미지
    private UsefullOperation usefullOperation;
    private bool increaseConditionFullfilled; // 텐션게이지를 상승시켜야 하는지의 여부
    private bool decreaseConditionFullfilled; // 텐션게이지를 감소시켜야 하는지의 여부
    private bool noticeAlarmed; //알림을 한 번 표시 했는지의 여부
    private int whichCase; // 4가지 경우 중 어떤 경우인지
    private int increaseValue; // 텐션게이지 증가 계수
    private int decreaseValue; // 텐션게이지 감소 계수
    private int tensionGauge; //텐션게이지 퍼센트 수치
    private Text percentText; //텐션게이지 퍼센트 텍스트
    private Text noticeText; //트리거 발동/ 빙결, 콤보 등을 알리는 텍스트

    public string comboText { get; set; } //콤보 스트링

    public void IncreaseTensionGauge(int whichCase, int increaseValue) //텐션게이지를 증가시키고 싶을 때 호출하는 메서드
    {
        increaseConditionFullfilled = true;
        this.whichCase = whichCase;
        this.increaseValue = increaseValue;
        tensionGauge += 10 * increaseValue;
    }

    public void DecreaseTensionGauge(int decreaseValue)
    {
        decreaseConditionFullfilled = true;
        this.decreaseValue = decreaseValue;
        tensionGauge -= decreaseValue;
    }

    private void Start()
    {
        tensionGaugeBar = GameObject.Find("Image_TensionGaugeBar");
        usefullOperation = GameObject.Find("GameResource").GetComponent<UsefullOperation>();
        percentText = GameObject.Find("Text_GaugePercent").GetComponent<Text>();
        noticeText = GameObject.Find("Text_NoticeBoard").GetComponent<Text>();
        tensionGaugeBar.GetComponent<Image>().fillAmount = 0;
        tensionGauge = 0;
        increaseConditionFullfilled = false;
        decreaseConditionFullfilled = false;
        noticeAlarmed = false;
    }

    private void FixedUpdate()
    {
        if (increaseConditionFullfilled)
        {
            tensionGaugeBar.GetComponent<Image>().fillAmount += (0.1f * increaseValue) * Time.fixedDeltaTime;
            percentText.text = (int)(100 * tensionGaugeBar.GetComponent<Image>().fillAmount) + "%";
            if (tensionGaugeBar.GetComponent<Image>().fillAmount * 100 > tensionGauge || tensionGaugeBar.GetComponent<Image>().fillAmount == 1)
            {
                if (tensionGauge > 100) tensionGauge = 100;
                percentText.text = tensionGauge + "%";
                tensionGaugeBar.GetComponent<Image>().fillAmount = 0.01f * tensionGauge; //이미지 길이 조절
                increaseConditionFullfilled = false;
            }

            if (!noticeAlarmed) //알림 관리. 알림은 한 번만 실행된다.
            {
                switch (whichCase) //알림 관리
                {
                    case 1: //(1)의 경우
                        break;
                    case 2: //(2)의 경우 현재 콤보 수를 표시한다.
                        usefullOperation.ShakeObject(noticeText.transform, 0.3f, 8);
                        comboText = increaseValue + "COMBO!! \n";
                        noticeText.text = comboText;
                        break;
                    case 3: //(3)의 경우 텐션게이지가 꽉차있거나 아무것도 얼리지 않은 경우를 제외하면 FREEZE BONUS!! 문구가 표시된다.
                        if ((tensionGaugeBar.GetComponent<Image>().fillAmount != 1) && increaseValue != 0) noticeText.text += "FREEZE BONUS!!";
                        break;
                    case 4: //(4)의 경우
                        if (tensionGaugeBar.GetComponent<Image>().fillAmount != 1) noticeText.text += "TRIGGER BONUS!!";
                        break;
                }
                noticeAlarmed = true;
            }
        }
        else
        {
            noticeText.text = comboText;
            noticeAlarmed = false;
        }

        if (decreaseConditionFullfilled)
        {
            tensionGaugeBar.GetComponent<Image>().fillAmount -= (0.01f * decreaseValue) * Time.fixedDeltaTime;
            percentText.text = (int)(100 * tensionGaugeBar.GetComponent<Image>().fillAmount) + "%";
            if (tensionGaugeBar.GetComponent<Image>().fillAmount * 100 < tensionGauge || tensionGaugeBar.GetComponent<Image>().fillAmount == 1)
            {
                if (tensionGauge < 0) tensionGauge = 0;
                percentText.text = tensionGauge + "%";
                tensionGaugeBar.GetComponent<Image>().fillAmount = 0.01f * tensionGauge; //이미지 길이 조절
                decreaseConditionFullfilled = false;
            }
        }
    }
}