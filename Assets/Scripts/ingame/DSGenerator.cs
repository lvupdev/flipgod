using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ingame;

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
        GameObject childDS = Instantiate(Resources.Load(dSName)) as GameObject;
        childDS.name = dSName;
        childDS.transform.SetParent(parentDS.transform);
        SetDynamicStructure(childDS);
    }

    // 동적 구조물의 여러 속성을 설정
    public void SetDynamicStructure(GameObject childDS)
    {
        DSController dSController = childDS.AddComponent<DSController>();
        Rigidbody2D rigidbody2D = childDS.AddComponent<Rigidbody2D>();
        PolygonCollider2D polygonCollider2D = childDS.AddComponent<PolygonCollider2D>();
        SpriteRenderer spriteRenderer = childDS.GetComponent<SpriteRenderer>();
        rigidbody2D.gravityScale = 1;
        spriteRenderer.sortingOrder = 1;
        rigidbody2D.mass = 10;

        switch (childDS.name)
        {
            case "Car":
                Debug.Log("이 동적 구조물의 이름은" + childDS.name + "입니다");
                dSController.moveSpeed = 2f;
                dSController.minVec2 = new Vector2(-8.0f, -2.26f); //만들 때 어느 구조물 위에 있느냐에 따라 달라지니까 수정이 필요함.
                dSController.maxVec2 = new Vector2(-4.5f, -2.26f); 
                /* 구조물에 붙어있다면 collider box가 적용될텐데 그 collider를 감지해서
                그 구조물의 x의 최대 최소를 입력받아서 어떤 구조물 위에서도 떨어지지 않는 차가 될 수 있도록
                해야함
                */
                break;
            case "Ex":
                break;
        }
    }
}