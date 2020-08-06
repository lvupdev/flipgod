using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public Sprite baseSprite;
    public Sprite freezedSprite;
    public SpriteRenderer spriteRenderer;

    public int collisionNum {get; set;} //물병과 충돌한 횟수
    public bool isFreezed = false;
    public float delta;

    private void Start()
    {
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
            if (delta > 15)
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
