using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 동적 구조물 생성 스크립트
 */

public class DSGenerator : MonoBehaviour
{
    // 동적 구조물들을 나열할 부모 오브젝트
    private GameObject parentDS;

    private void Start()
    {
        parentDS = GameObject.Find("Dynamic Structure");
    }

    // 동적 구조물을 생성하고 이름을 변경
    public void GenerateDynamicStructure(string dSName)
    {
        GameObject childDS =
            Instantiate(Resources.Load(dSName)) as GameObject;
        childDS.name = dSName;
        childDS.transform.SetParent(parentDS.transform);
        SetDynamicStructure(childDS);
    }

    // 동적 구조물의 여러 속성을 설정
    public void SetDynamicStructure(GameObject childDS)
    {
        DSController dSController = childDS.AddComponent<DSController>();
        Rigidbody2D rigidbody2D = childDS.GetComponent<Rigidbody2D>();

        switch(childDS.name)
        {
            case "Car":
                Debug.Log("이 동적 구조물의 이름은" + childDS.name + "입니다");
                dSController.moveSpeed = 4f;
                break;
            case "Ex":
                break;
        }
    }
}
