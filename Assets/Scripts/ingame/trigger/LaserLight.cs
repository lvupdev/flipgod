using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLight : MonoBehaviour
{
	private LaserTrigger laserTrigger;

	void Start()
	{
		laserTrigger = transform.parent.GetComponent<LaserTrigger>();
		laserTrigger.laserLight = gameObject;
	}

    void OnTriggerEnter2D(Collider2D col)
	{
		BottleCollision bottle = col.gameObject.GetComponent<BottleCollision>();

		if (bottle != null)
		{
			laserTrigger.TargetObject.Add(bottle.gameObject);
		}
	}
}
