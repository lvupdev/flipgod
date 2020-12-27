using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public Sprite baseSprite;
    public Sprite freezedSprite;
    public SpriteRenderer spriteRenderer;
    public static UsefullOperation usefullOperation;

    public int collisionNum {get; set;} //물병과 충돌한 횟수
    public bool isFreezable; //얼릴 수 있는지의 여부
    public bool isFreezed;
    public bool getFreezeBonus; //해당 구조물을 얼림으로써 빙결 보너스를 받는지의 여부
    public float delta; //빙결 시간 변수

    public void Start()
    {
        usefullOperation = GameObject.Find("GameResource").GetComponent<UsefullOperation>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        delta = 0;
        collisionNum = 0;

    }



    // 동적 구조물 해동 메서드
    public void ThawDynamicStructure(bool isFreezed)
    {
        if (isFreezed)
        {
            spriteRenderer.sprite = freezedSprite;
            delta += Time.deltaTime;
            if (delta > 20)
            {
                this.isFreezed = false;
                delta = 0;
                spriteRenderer.sprite = baseSprite;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        collisionNum++;
    }
}
