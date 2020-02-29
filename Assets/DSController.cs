using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 동적 구조물 컨트롤 스크립트
*/
public class DSController : MonoBehaviour
{ 
    public bool isFreezed = false;

    private float delta;//얼고 지난 시간

    void Start()
    {
        delta = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFreezed)
        {
            delta += Time.deltaTime;
            if(delta>15)
            {
                isFreezed = false;
                delta = 0;
            }
        }
    }
}
