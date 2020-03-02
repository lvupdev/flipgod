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
    }

    // 동적 구조물 해동 메서드
    public void ThawDynamicStructure(bool isFreezed)
    {
        if (isFreezed)
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
        MoveDynamicStructure(minVec2, maxVec2, moveSpeed);
    }

    // 동적 구조물 움직이기
    public void MoveDynamicStructure(Vector2 minVec2, Vector2 maxVec2, float moveSpeed)
    {
        Vector2 currPos = gameObject.transform.position;
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (currPos.x < -15)
        {
            spriteRenderer.flipX = false;
            currPos += new Vector2(moveSpeed * Time.deltaTime * 1, 0);
        }
        else if (currPos.x > 15)
        {
            spriteRenderer.flipX = true;
            currPos += new Vector2(moveSpeed * Time.deltaTime * - 1, 0);
        }


        /*
         * if (currPos.x <= minVec2.x || currPos.x >= minVec2.y)
         * {
         *    sign *= -1;
         * }
         */
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // empty
    }
}
