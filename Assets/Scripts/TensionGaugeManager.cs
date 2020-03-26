using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 캐릭터의 프로필과 텐션 게이지를 관리하고 
 * UI에 표시하는 스크립트
 */

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
     * (1) 물병을 하나 던질 때마다
     * (1-1) 해당 물병을 세울 경우             +20%
     * (1-2) 해당 물병을 세우지 못할 경우       +10%
     * (2) 물병을 연속으로 세우는 경우          +10% * 콤보수
     * (3) 트리거의 발동                       +10%
     * (4) 동적 장애물을 빙결시키는 경우        +10% * 빙결된 동적 장애물 개수
     =================================================================================================*/

    private Image Img_tensionGauage;        // 텐션 게이지를 표시하는 이미지

    private static float tensionValue;

    private bool isBottleThrown;
    private bool isBottleStanding;
    private bool isComboOngoing;
    private bool isTriggerAct;
    private bool isSucceedToFreeze;


    private void Awake()
    {
        Img_tensionGauage = gameObject.GetComponent<Image>();
        Img_tensionGauage.fillAmount = 0f;  // 텐션 게이지 이미지 초기화

        isBottleThrown = false;
        isBottleStanding = false;
        isComboOngoing = false;
        isTriggerAct = false;
        isSucceedToFreeze = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tensionValue = Img_tensionGauage.fillAmount;

        if (isBottleThrown == true)
        {
            if (isBottleStanding == true)
            {
                UpdateTensionGauge(2);
            }
            else
            {
                UpdateTensionGauge(1);
            }

            isBottleStanding = false;
            isBottleThrown = false;
        }

        if (isComboOngoing == true)
        {

            isComboOngoing = false;
        }

        if (isTriggerAct == true)
        {
            UpdateTensionGauge(1);
            isTriggerAct = false;
        }

        if (isSucceedToFreeze == true)
        {
            UpdateTensionGauge(1);
        }
    }

    public void UpdateTensionGauge(int fillCount)
    {
        if (Img_tensionGauage.fillAmount < 1.0f)
        {
            Img_tensionGauage.fillAmount += 0.1f * fillCount;
        }
    }



    /*===========<call-back method>===================================================*/
    public void UseTensionGaue()
    {
        Img_tensionGauage.fillAmount = 0f;
    }


    /*=================================================================================*/


}
