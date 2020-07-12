using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
물병을 생성하는 스크립트
*/
public class BottleGenerator : MonoBehaviour
{
    public GameObject bottles;
    public GameObject bottlePrefab;
    private GameObject controllButtons;

    public void Start()
    {
        bottles = GameObject.Find("Bottles");
        controllButtons = GameObject.Find("ControllButtons");
    }

    public void GenerateBottle()
    {
        controllButtons.SetActive(true); //숨겼던 컨트롤 UI 버튼 표시
        
        GameObject Bottle = Instantiate(bottlePrefab) as GameObject;
        Bottle.transform.SetParent(bottles.transform);

        Bottle.tag = "isActBottle"; //PadStrength.cs 75번째 줄 오류 때문에 수정

        StageGameMgr.instance.CountUsedBottle();    // (0229) 보틀 하나 생성할 때마다 보틀 사용했음을 UI에 표시

    }
}