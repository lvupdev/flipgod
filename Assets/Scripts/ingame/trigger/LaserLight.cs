using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLight : MonoBehaviour
{
	private LaserTrigger laserTrigger;

	void Start()
	{
		laserTrigger = transform.parent.GetComponent<LaserTrigger>();
	}

    void OnTriggerEnter2D(Collider2D col)
	{
		BottleCollision bottle = col.gameObject.GetComponent<BottleCollision>();

		if (bottle == null) return;
		else
		{
			laserTrigger.TargetObject.Add(bottle.gameObject);
		}
	}
}
