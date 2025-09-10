using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 물병 밑 면에 설치한 트리거 콜라이더를 통해 해당 본 물병을 세울지 결정한다.
 */

public class BottleBottom : MonoBehaviour
{
    private BottleController bottleController;
    private float zRotation; //물병의 z회전값

    // Start is called before the first frame update
    void Start()
    {
        bottleController = transform.parent.gameObject.GetComponent<BottleController>();

    }


    private void OnTriggerEnter2D(Collider2D col)
	{
        Structure structure = col.transform.GetComponent<Structure>();

        if (structure == null)
		{
            BottleController bottle = col.transform.GetComponent<BottleController>();

            if (bottle == null) return; //바닥에 닿은 물체가 구조물도 아니고 물병도 아니면 return
		}
        
        zRotation = bottleController.transform.eulerAngles.z;

        //처음 충돌했을 때 각도가 20도 이하 또는 340도 이상이고 양쪽 바닥면이 모두 바닥과 닿아 있으면
        if ((zRotation > 335) || (zRotation < 25))
        {
            bottleController.standBottle = true;
		}

    }
}
