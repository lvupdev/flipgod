using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 스테이지 레벨2
 */

public class StageManager2 : MonoBehaviour
{
    // Dynamic Structure
    Bird bird;

    void Start()
    {
        bird = GameObject.Find("Bird").GetComponent<Bird>();
        bird.setValue(2f);
        bird.MoveDynamicStructure();
    }

}
