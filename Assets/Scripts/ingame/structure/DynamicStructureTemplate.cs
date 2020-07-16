using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 동적구조물 새로 추가할때 템플릿
*/

public class DSTemplate : Structure  //DynamicStructure 상속
{

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        delta = 0;
        collisionNum = 0;
    }

    void Update()
    {
        ThawDynamicStructure(isFreezed);
        if (!isFreezed) MoveDynamicStructure();
    }

    public void setValue() //스크립트 필드의 값을 설정하는 메소드.
    {}

    public void MoveDynamicStructure() // 동적구조물 실행 메소드. 이 메소드 실행으로 계속 해야함
    {}

}