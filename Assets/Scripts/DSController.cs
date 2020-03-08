using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 동적 구조물 컨트롤 스크립트
*/

public class DSController : MonoBehaviour
{

    // 동적 구조물이 얼었는지 저장
    public bool isFreezed = false;
    // 동적 구조물의 언 상태가 지속된 시간
    private float delta;

    // 동적 구조물의 움직임
    // minVec2 ~ maxVec2 사이에서 동작
    public Vector2 minVec2;
    public Vector2 maxVec2;
    public float moveSpeed;

    void Start()
    {
        delta = 0;
    }


    // Update is called once per frame
    void Update()
    {
        ThawDynamicStructure(isFreezed);
        MoveDynamicStructure(minVec2, maxVec2, moveSpeed);
    }

    // 동적 구조물 해동 메서드
    public void ThawDynamicStructure(bool isFreezed)
    {
        if (isFreezed == true)
        {
            delta += Time.deltaTime;
            if (delta > 15)
            {
                isFreezed = false;
                delta = 0;
            }
        }
    }

    private void FixedUpdate()
    {

    }

    // 동적 구조물 움직이기
    public void MoveDynamicStructure(Vector2 minVec2, Vector2 maxVec2, float moveSpeed)
    {
        // 현재 dynamic structure의 위치
        Vector2 currPos = gameObject.transform.position;
        // dynamic structure의 rigidbody2D
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        // dynamic structure의 spriteRenderer
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (currPos.x <= maxVec2.x)
        {
            spriteRenderer.flipX = false;
            gameObject.transform.position += new Vector3(moveSpeed * Time.deltaTime * 1, 0);
            //            rigidbody2D.velocity = new Vector2(moveSpeed * 1, 0);
        }
        else if (currPos.x >= minVec2.x)
        {
            spriteRenderer.flipX = true;
            gameObject.transform.position += new Vector3(moveSpeed * Time.deltaTime * -1, 0);
            //            gameObject.transform.Translate(new Vector3(moveSpeed * Time.deltaTime * -1, 0));
            //            rigidbody2D.velocity = new Vector2(moveSpeed * -1, 0);
        }


        /*
         * if (currPos.x <= minVec2.x || currPos.x >= minVec2.y)
         * {
         *    sign *= -1;
         * }
         */
    }
}
