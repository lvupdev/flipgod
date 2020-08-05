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
    private ControllButtonsUIManager controllButtonsUIManager;
    private PlayerImageController playerImageController;

    public void Start()
    {
        bottles = GameObject.Find("Bottles");
        controllButtonsUIManager = GameObject.Find("UIManager").GetComponent<ControllButtonsUIManager>();
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
    }

    public void GenerateBottle()
    {
        controllButtonsUIManager.setShowButtons(true, 0); //숨겼던 컨트롤 UI 버튼 표시
        playerImageController.ChangePlayerImage(0); //물병을 던지기 전의 이미지로 변경
 
        GameObject Bottle = Instantiate(bottlePrefab) as GameObject;
        Bottle.transform.SetParent(bottles.transform);

        Bottle.tag = "isActBottle"; //PadStrength.cs 75번째 줄 오류 때문에 수정

        // After generating bottle, decrease remaining bottle num
        StageGameManager.CountUsedBottle();
    }
}