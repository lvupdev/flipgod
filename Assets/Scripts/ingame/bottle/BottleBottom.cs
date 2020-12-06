using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleBottom : MonoBehaviour
{
    private GameObject bottle;
    private Rigidbody2D bottleRb;
    private float zRotation; //물병의 z회전값
    private bool standBottle; //물병을 세울건지의 여부
    private float delta;

    // Start is called before the first frame update
    void Start()
    {
        bottle = transform.parent.gameObject;
        bottleRb = bottle.GetComponent<Rigidbody2D>();
        standBottle = false;
        delta = 0;
    }

    // Update is called once per frame
    void Update()
    {
		if (standBottle)
		{
            if (delta > 1)
            {
                standBottle = false;
                bottleRb.constraints = RigidbodyConstraints2D.None;
                delta = 0;
            }
            else if((bottle.transform.eulerAngles.z > 355) || (bottle.transform.eulerAngles.z < 5))
			{
                if(bottle.transform.eulerAngles != Vector3.zero)bottle.transform.rotation = Quaternion.Euler(Vector3.zero);
                bottleRb.constraints = RigidbodyConstraints2D.FreezeRotation;
                delta += Time.deltaTime;
            }
			else
			{
                if (zRotation > 340) bottle.transform.Rotate(new Vector3(0, 0, Time.deltaTime), Space.World); //물병의 z축 값을 360으로 수렴
                else bottle.transform.Rotate(new Vector3(0, 0, -1 * Time.deltaTime), Space.World); //물병의 z축 값을 0으로 수렴
                delta += Time.deltaTime;;
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
