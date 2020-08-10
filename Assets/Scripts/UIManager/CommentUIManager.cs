using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CommentUIManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // SpriteRenderer를 쓰기위한 객체 생성
    public Sprite[] commentSprites; // comment 이미지를 담아놓는 배열
    public GameObject commentLabel;
    private int Kang = 0;
    private int Lee = 1;
    private int Baek = 2;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Changecomment1()
    {   
        commentLabel.GetComponent<Image>().sprite = commentSprites[Kang];
    }
    public void Changecomment2()
    {
        commentLabel.GetComponent<Image>().sprite = commentSprites[Lee];
    }
    public void Changecomment3()
    {
        commentLabel.GetComponent<Image>().sprite = commentSprites[Baek];
    }
}
