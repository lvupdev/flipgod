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
        bottle.tag = "isActive"; //NEW: PadStrength.cs 75번째 줄 오류 때문에 수정
    }
}
