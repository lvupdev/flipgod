using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 자주 사용되는 메서드들을 담은 스크립트
 */

public class UsefullOperation : MonoBehaviour
{

    private List<ObjectInformation> objectSpriteRenderer = new List<ObjectInformation>(); //목표 오브젝트들의 스프라이트 렌더러를 담는 어레이

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (objectSpriteRenderer.Count>0)
        {
            for(int i = objectSpriteRenderer.Count-1; i>=0; i--) //리스트의 마지막 요소부터 검사
            {
                if (objectSpriteRenderer[i].fadeOut) 
                {
                    objectSpriteRenderer[i].color.a -= 4 * Time.deltaTime; //약 0.25초동안 페이드 아웃 효과
                    objectSpriteRenderer[i].spriteRenderer.color = objectSpriteRenderer[i].color;
                    if (objectSpriteRenderer[i].color.a < 0)
                    {
                        if (objectSpriteRenderer[i].destroy) Destroy(objectSpriteRenderer[i].spriteRenderer.gameObject);
                        else objectSpriteRenderer[i].spriteRenderer.gameObject.SetActive(false);
                        objectSpriteRenderer.RemoveAt(i);
                    }
                }
                else
                {
                    objectSpriteRenderer[i].color.a += 4 * Time.deltaTime; //약 0.25초동안 페이드 아웃 효과
                    objectSpriteRenderer[i].spriteRenderer.color = objectSpriteRenderer[i].color;
                    if (objectSpriteRenderer[i].color.a > 1)
                    {
                        objectSpriteRenderer.RemoveAt(i);
                    }
                }
            }
        }
    }

    public void FadeOut(bool destroy, SpriteRenderer spriteRenderer) //인수 스프라이트에 페이드 아웃 효과를 준다. destroy가 true이면 해당 오브젝트를 파괴한다.
    {
        ObjectInformation objectInformation = new ObjectInformation(spriteRenderer, false);
        objectInformation.destroy = destroy;
        objectSpriteRenderer.Add(objectInformation);
    }

    public void FadeIn(SpriteRenderer spriteRenderer)
    {
        ObjectInformation objectInformation = new ObjectInformation(spriteRenderer, true);
        objectInformation.color.a = 0;
        spriteRenderer.color = objectInformation.color;
        spriteRenderer.gameObject.SetActive(true);
        objectSpriteRenderer.Add(objectInformation);
    }
}

class ObjectInformation {

    public bool destroy; //오브젝트를 파괴할지의 여부
    public bool fadeIn; //페이드인 효과를 적용할 지의 여부
    public bool fadeOut; //페이드 아웃 효과를 적용할지의 여부

    public SpriteRenderer spriteRenderer;
    public Color color;

    public ObjectInformation(SpriteRenderer spriteRenderer, bool fadeIn)
    {
        this.spriteRenderer = spriteRenderer;
        color = spriteRenderer.color;
        destroy = false;
        if (fadeIn)
        {
            this.fadeIn = true;
            this.fadeOut = false;
        }
        else
        {
            this.fadeIn = false;
            this.fadeOut = true;
        }
    }
}

