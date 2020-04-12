using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffect : MonoBehaviour
{
    public List<GameObject> TargetObject = new List<GameObject>(); //범위 내에 있는 구조물들의 배열
    private GameObject structures;

    private void Awake()
    {
        structures = GameObject.Find("Structures");
    }

    public void freeze()
    {
        for (int i = 0; i < TargetObject.Count; i++)
        {
            TargetObject[i].GetComponent<Structure>().isFreezed = true;
            TargetObject[i].GetComponent<Structure>().delta = 0;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent == structures.transform) TargetObject.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent == structures.transform) TargetObject.Remove(collision.gameObject);
    }
}
