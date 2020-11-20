using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Freezer : MonoBehaviour
{
    private BottleSelectController bottleSelectController;
    private ScreenEffectController screenEffectController;
    private UsefullOperation usefullOperation;
    private GameObject bottles;

    public bool freezeAvailable { get; set; } // 2번 발동 방지 변수
    public GameObject freezeParticlePrefab;

    // Start is called before the first frame update
    void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        screenEffectController = GameObject.Find("Main Camera").GetComponent<ScreenEffectController>();
        usefullOperation = GameObject.Find("GameResource").GetComponent<UsefullOperation>();
        bottles = GameObject.Find("Bottles");

        freezeAvailable = true;
    }

    // Update is called once per frame

    public void Activate()
    {
        GameObject freezeParticle = Instantiate(freezeParticlePrefab) as GameObject;
        freezeParticle.transform.position = bottleSelectController.bottle.transform.position; //물병 위치에 파티클 이펙트 생성

        screenEffectController.FreezeEffect();
        bottleSelectController.bottle.transform.Find("FreezeRange").GetComponent<FreezeEffect>().Freeze();
        usefullOperation.FadeOut(false, bottleSelectController.bottle.transform.Find("FreezeRange").GetComponent<SpriteRenderer>());
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
                usefullOperation.FadeOut(false, freezeRange.GetComponent<SpriteRenderer>());
                GameObject freezeParticle = Instantiate(freezeParticlePrefab) as GameObject;
                freezeParticle.transform.position = bottle.transform.position;
                isBottleSelected = true;
            }
        }
        if(isBottleSelected) screenEffectController.FreezeEffect(); //물병이 하나 이상 선택되었을 때에만 화면진동 효과 줌
    }
}