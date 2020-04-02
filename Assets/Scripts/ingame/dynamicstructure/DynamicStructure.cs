using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ingame;

/*
 * 동적 구조물 공통 스크립트
 */

public class DynamicStructure : MonoBehaviour
{

    public bool isFreezed = false;
    private float delta;


    private void Start()
    {
        delta = 0;
    }

    // 동적 구조물 해동 메서드
    public void ThawDynamicStructure(bool isFreezed)
    {
        if (isFreezed == true)
        {
            delta += Time.deltaTime;
            if (delta > 15)
            {
                isFreezed = false;
                delta = 0;
            }
        }
    }

}