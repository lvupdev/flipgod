using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffect : MonoBehaviour
{
    public List<GameObject> TargetObject = new List<GameObject>(); //범위 내에 있는 구조물들의 배열
    private GameObject structures;
    private TensionGaugeManager tensionGaugeManager;
    private ResourceManager gameResourceValue;
    private CircleCollider2D circleCollider;
    private int superPowerLV; //초능력 강화 레벨
    private float freezeRad; //빙결 가능 범위 반지름

    private void Awake()
    {
        structures = GameObject.Find("Structures");
        tensionGaugeManager = GameObject.Find("Image_TensionGaugeBar").GetComponent<TensionGaugeManager>();
        gameResourceValue = GameObject.Find("GameResource").GetComponent<ResourceManager>();
        circleCollider = GetComponent<CircleCollider2D>();
        superPowerLV = gameResourceValue.GetSuperPowerLV(2);

        //초기 반지름 설정
        freezeRad = superPowerLV * 5.1492f; //빙결자의 초능력 강화 레벨 수치의 배수 만큼이 빙결 가능 범위의 반지름이 된다.
        circleCollider.radius = freezeRad;
        transform.localScale = new Vector3(0.5f * superPowerLV, 0.47f * superPowerLV, 1);

    }

    public void Freeze()
    {   
        for (int i = 0; i < TargetObject.Count; i++)
        {
            TargetObject[i].GetComponent<Structure>().isFreezed = true;
            TargetObject[i].GetComponent<Structure>().delta = 0;
        }

        tensionGaugeManager.IncreaseTensionGauge(3, TargetObject.Count);

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