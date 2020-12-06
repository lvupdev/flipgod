using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleBottom : MonoBehaviour
{
    private GameObject bottle;
    private float zRotation; //물병의 z회전값
    private bool standBottle; //물병을 세울건지의 여부
    private float delta;

    // Start is called before the first frame update
    void Start()
    {
        bottle = transform.parent.gameObject;
        standBottle = false;
        delta = 0;
    }

    // Update is called once per frame
    void Update()
    {
		if (standBottle)
		{
            bottle.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(bottle.transform.eulerAngles.z, 0, Time.deltaTime)); //물병의 z축 값을 0으로 수렴
            delta += Time.deltaTime;

            if((delta > 1f) &&((bottle.transform.eulerAngles.z > 359) || (bottle.transform.eulerAngles.z < 1))) //1초가 지나고 물병의 z축 값이 0에 가까우면
			{
                standBottle = false;
                delta = 0;
			}
		}
        
    }

    private void OnTriggerEnter2D(Collider2D col)
	{
        Structure structure = col.transform.GetComponent<Structure>();

        if (structure == null) return;
        
        zRotation = bottle.transform.eulerAngles.z;

        if((zRotation > 340) || (zRotation < 20)) //처음 충돌했을 때 각도가 30도 이하 또는 330도 이상이면
        {
            Debug.Log("실행됨");
            standBottle = true;
		}

    }
}
