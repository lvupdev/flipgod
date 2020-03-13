﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * 플레이어 관리 스크립트입니다.
 * 염동력자, 탄성막 생성자, 빙결자에게 배정한 숫자는 순서대로 0, 1, 2 입니다.
*/
public class PlayerImageController : MonoBehaviour
{
    public int playingChr; //조작할 수 있는 캐릭터
    public int firstSlotChr; //교체 슬롯 1번에 있는 캐릭터
    public int secondSlotChr; //교체 슬롯 2번에 있는 캐릭터
    private SpriteRenderer spriteRenderer;
    private GameObject bottle;
    private BottleController bottleController;
    private PadStrength padStrength;
    public PadDirection padDirection;
    public Sprite[] standingSprites; // 스탠딩 이미지를 담아놓는 배열
    public Sprite[] iconSprites; //아이콘 이미지를 담아놓는 배열

    private Vector3 bottlePosition; //물병이 위치해야 하는 포지션
    private int key; //플레이어의 스프라이트 방향 결정 겂

    //값 반환 함수
    public Vector3 GetBottlePosition() { return bottlePosition; }

    private void Awake()
    {
        bottlePosition = transform.GetChild(0).transform.position;
    }

    void Start()
    {
        playingChr = 0; //염동력자
        firstSlotChr = 1; //탄성막 생성자
        secondSlotChr = 2; //빙결자
        spriteRenderer = GetComponent<SpriteRenderer>();
        bottle = GameObject.Find("BottlePrefab");
        bottleController = bottle.GetComponent<BottleController>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        padDirection = GameObject.Find("Joystick").GetComponent<PadDirection>();
        spriteRenderer.sprite = standingSprites[0];
    }

    private void Update()
    {
        if (padDirection.direction.x <= 0) key = 1;
        if (padDirection.direction.x > 0) key = -1;
        transform.localScale = new Vector3(key * 0.4f, 0.4f, 1); //패드 위치에 따라 캐릭터가 향하는 방향이 바뀜

        bottlePosition = transform.GetChild(playingChr).transform.position;
    }

    public void CharacterSlot1()
    {
        if((!bottleController.isSuperPowerAvailabe) && (!padStrength.isThrowing) &&(!padStrength.isTouch))
        {
            int temp;
            temp = playingChr;
            playingChr = firstSlotChr;
            firstSlotChr = temp;
            GameObject.Find("CharacterSlot1").GetComponent<Image>().sprite = iconSprites[firstSlotChr];
            spriteRenderer.sprite = standingSprites[playingChr];

            bottle.transform.position = transform.GetChild(playingChr).transform.position;
            bottlePosition = transform.GetChild(playingChr).transform.position; //  물병 위치 결정
        }
    }

    public void CharacterSlot2()
    {
        if ((!bottleController.isSuperPowerAvailabe) && (!padStrength.isThrowing) && (!padStrength.isTouch))
        {
            int temp;
            temp = playingChr;
            playingChr = secondSlotChr;
            secondSlotChr = temp;
            GameObject.Find("CharacterSlot2").GetComponent<Image>().sprite = iconSprites[secondSlotChr];
            spriteRenderer.sprite = standingSprites[playingChr];

            bottle.transform.position = transform.GetChild(playingChr).transform.position;
            bottlePosition = transform.GetChild(playingChr).transform.position; //  물병 위치 결정
        }
    }

    public void ReselectBottle()
    {
        bottle = GameObject.FindWithTag("isActBottle");//물병을 태그에 따라 재설정
        bottleController = bottle.GetComponent<BottleController>();
    }
}
