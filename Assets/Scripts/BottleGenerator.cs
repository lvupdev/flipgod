using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
물병을 생성하는 스크립트
*/
public class BottleGenerator : MonoBehaviour
{
    public GameObject bottlePrefab;
    public Vector3 startPosition; //물병을 생성할 위치

    public void GenerateBottle()
    {
        GameObject bottle = Instantiate(bottlePrefab) as GameObject;
        bottle.transform.position = startPosition;

        bottle.tag = "isActBottle"; //PadStrength.cs 75번째 줄 오류 때문에 수정


        StageGameMgr.instance.CountUsedBottle();    // (0229) 보틀 하나 생성할 때마다 보틀 사용했음을 UI에 표시

    }
}
