using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Freezer : MonoBehaviour
{
    private int superPowerLV; //초능력 강화 레벨
    private BottleSelectController bottleSelectController;
    private ScreenEffectController screenEffectController;
    private ResourceManager gameResourceValue;
    private GameObject bottles;

    public bool freezeAvailable; // 2번 발동 방지 변수

    // Start is called before the first frame update
    void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        screenEffectController = GameObject.Find("Main Camera").GetComponent<ScreenEffectController>();
        gameResourceValue = GameObject.Find("GameResource").GetComponent<ResourceManager>();
        bottles = GameObject.Find("Bottles");

        superPowerLV = gameResourceValue.GetSuperPowerLV(2);
        freezeAvailable = true;
    }

    // Update is called once per frame

    public void Activate()
    {
        screenEffectController.FreezeEffect();
        bottleSelectController.bottle.transform.Find("FreezeRange").GetComponent<FreezeEffect>().Freeze();
        bottleSelectController.bottle.transform.Find("FreezeRange").gameObject.SetActive(false);
        freezeAvailable = false;
    }

    public void SkillActivate()
    {
        bool isBottleSelected = false; // 물병이 선택되었는지의 여부
        for (int i = 0; i < bottles.transform.childCount; i++) // 필살기 발동 매개가 되는 물병 찾기
        {
            GameObject bottle = bottles.transform.GetChild(i).gameObject;
            GameObject freezeRange = bottle.transform.GetChild(1).gameObject;
            if (freezeRange.activeSelf)
            {
                freezeRange.GetComponent<FreezeEffect>().Freeze();
                freezeRange.SetActive(false);
                isBottleSelected = true;
            }
        }
        if(isBottleSelected) screenEffectController.FreezeEffect(); //물병이 하나 이상 선택되었을 때에만 화면진동 효과 줌
    }
}
