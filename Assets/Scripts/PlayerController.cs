using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * 플레이어 관리 스크립트입니다.
 * 염동력자, 탄성막 생성자, 빙결자에게 배정한 숫자는 순서대로 0, 1, 2 입니다.
*/
public class PlayerController : MonoBehaviour
{
    public int playingChr; //조작할 수 있는 캐릭터
    public int firstSlotChr; //교체 슬롯 1번에 있는 캐릭터
    public int secondSlotChr; //교체 슬롯 2번에 있는 캐릭터
    private SpriteRenderer spriteRenderer;
    private BottleController bottleController;
    public Sprite[] standingSprites; // 스탠딩 이미지를 담아놓는 배열
    public Sprite[] iconSprites; //아이콘 이미지를 담아놓는 배열


    void Start()
    {
        playingChr = 0; //염동력자
        firstSlotChr = 1; //탄성막 생성자
        secondSlotChr = 2; //빙결자
        spriteRenderer = GetComponent<SpriteRenderer>();
        bottleController = GameObject.Find("BottlePrefab").GetComponent<BottleController>();
        spriteRenderer.sprite = standingSprites[0];
    }

    public void CharacterSlot1()
    {
        if(!bottleController.isSuperPowerAvailabe)
        {
            int temp;
            temp = playingChr;
            playingChr = firstSlotChr;
            firstSlotChr = temp;
            GameObject.Find("CharacterSlot1").GetComponent<Image>().sprite = iconSprites[firstSlotChr];
            spriteRenderer.sprite = standingSprites[playingChr];
        }
    }

    public void CharacterSlot2()
    {
        if (!bottleController.isSuperPowerAvailabe)
        {
            int temp;
            temp = playingChr;
            playingChr = secondSlotChr;
            secondSlotChr = temp;
            GameObject.Find("CharacterSlot2").GetComponent<Image>().sprite = iconSprites[secondSlotChr];
            spriteRenderer.sprite = standingSprites[playingChr];
        }
    }

    public void ReselectBottle()
    {
        bottleController = GameObject.FindWithTag("isActive").GetComponent<BottleController>();//물병을 태그에 따라 재설정
    }
}
