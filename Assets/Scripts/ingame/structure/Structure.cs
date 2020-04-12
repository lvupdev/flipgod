using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public int collisionNum {get; set;} //물병과 충돌한 횟수
    public bool isFreezed = false;
    public float delta;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        delta = 0;
        collisionNum = 0;
    }

    // 동적 구조물 해동 메서드
    public void ThawDynamicStructure(bool isFreezed)
    {
        if (isFreezed == true)
        {
            spriteRenderer.color = new Color(0, 0, 1, 1);
            delta += Time.deltaTime;
            if (delta > 15)
            {
                this.isFreezed = false;
                delta = 0;
                spriteRenderer.color = new Color(1, 1, 1, 1);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        collisionNum++;
    }
}
