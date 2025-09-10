using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//물병의 왼쪽 바닥

public class LeftBottom : MonoBehaviour
{
    private List<GameObject> gameObjects; //왼쪽 바닥과 닿은 물체들의 리스트 

    public bool isLeftBottomTouched; //물병 왼쪽 바닥이 바닥과 닿고 있는지의 여부


    void Start()
	{
        gameObjects = new List<GameObject>();
        isLeftBottomTouched = false;
	}

    void Update()
	{
        if (gameObjects.Count > 0) isLeftBottomTouched = true;
        else isLeftBottomTouched = false;
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        Structure structure = col.transform.GetComponent<Structure>();

        //트리거에 닿은 물체가 구조물이거나 물병이면 gameObjects에 추가
        if (structure == null)
		{
            BottleController bottle = col.transform.GetComponent<BottleController>();

            if (bottle == null) return;
            else gameObjects.Add(col.gameObject);
		}
		else
		{
            gameObjects.Add(col.gameObject);
		}
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        Structure structure = col.transform.GetComponent<Structure>();

        //트리거에서 빠져나간 물체가 구조물이거나 물병이면 gameObjects에서 제거
        if (structure == null)
        {
            BottleController bottle = col.transform.GetComponent<BottleController>();

            if (bottle == null) return;
            else gameObjects.Remove(col.gameObject);
        }
        else
        {
            gameObjects.Remove(col.gameObject);
        }
    }
}
