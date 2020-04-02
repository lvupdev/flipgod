using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 스테이지 레벨 매니저
 */

public class StageManager : MonoBehaviour
{
    // Dynamic Structure
    Car car;

    void Start()
    {
        car = GameObject.Find("Car").GetComponent<Car>();
        car.setValue(new Vector2(-8.0f, -2.26f), new Vector2(-4.5f, -2.26f), 2f);
        car.MoveDynamicStructure();
    }

}
