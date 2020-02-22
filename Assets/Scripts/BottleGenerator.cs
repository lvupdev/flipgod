using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleGenerator : MonoBehaviour
{
    public GameObject bottlePrefab;
    public Vector3 startPosition; //물병을 생성할 위치

    public void GenerateBottle()
    {
        GameObject bottle = Instantiate(bottlePrefab) as GameObject;
        bottle.transform.position = startPosition;
    }
}
