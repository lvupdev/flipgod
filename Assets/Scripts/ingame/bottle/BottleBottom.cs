using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (structure == null) return;
        
        zRotation = bottleController.transform.eulerAngles.z;

        if((zRotation > 340) || (zRotation < 20)) //처음 충돌했을 때 각도가 30도 이하 또는 330도 이상이면
        {
            Debug.Log("실행됨");
            bottleController.standBottle = true;
		}

    }
}
