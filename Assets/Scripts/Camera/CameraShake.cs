using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//빙결 작용 시 카메라 흔들리는 효과
public class CameraShake : MonoBehaviour
{
    public float shakeAmount;
    float shakeTime;
    Vector3 initialPosition;

    public void VibrateForTime(float time)
    {
        shakeTime = time;
    }

    void Start()
    {
        initialPosition = new Vector3(0, 0, -10f);
    }

    void FixedUpdate()
    {
        if(shakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * shakeAmount + initialPosition;
            shakeTime -= Time.fixedDeltaTime;
        }
        else
        {
            shakeTime = 0;
            transform.position = initialPosition;
        }
    }
}
