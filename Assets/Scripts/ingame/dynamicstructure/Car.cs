using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ingame;

/*
 * 자동차 컨트롤 스크립트
*/

public class Car : DynamicStructure 
{

    // 차의 움직임
    // minVec2 ~ maxVec2 사이에서 동작
    public Vector2 minVec2{get;set;}
    public Vector2 maxVec2{get;set;}
    public float moveSpeed{get;set;}
    private int direction = 1;


    void Start()
    {
        Rigidbody2D rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        PolygonCollider2D polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rigidbody2D.gravityScale = 1;
        spriteRenderer.sortingOrder = 1;
        rigidbody2D.mass = 10;
    }

    // Update is called once per frame
    void Update()
    {
        ThawDynamicStructure(isFreezed);
        MoveDynamicStructure();
    }

    public void setValue(Vector2 minVec2, Vector2 maxVec2, float moveSpeed){
        this.minVec2 = minVec2;
        this.maxVec2 = maxVec2;
        this.moveSpeed = moveSpeed;
    }

    // 자동차 움직이기
    public void MoveDynamicStructure()
    {
        Vector2 currPos = gameObject.transform.position;
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        gameObject.transform.position += new Vector3(direction * moveSpeed * Time.deltaTime * 1, 0);

        if (currPos.x >= maxVec2.x)
        {
            direction = -1;
            spriteRenderer.flipX = false;
        }

        if (currPos.x <= minVec2.x)
        {
            direction = 1;
            spriteRenderer.flipX = true;
        }
    }


}