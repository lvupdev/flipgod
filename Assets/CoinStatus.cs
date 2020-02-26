using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
동전의 값(value) 설정
*/

public class CoinStatus : MonoBehaviour
{

    public int value { get; set; }

    void Start()
    {
        value = 10;
    }

}
