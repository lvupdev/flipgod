using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*
물병이 어딘가에 닿았을 때
*/
public class BottleCollision : MonoBehaviour
{
    public PadStrength padStrength;
    public List<BottleCollision> bottleChain = new List<BottleCollision>(); //현재 물병과 콜라이더로 연속적으로 이어져 있는 물병의 리스트
    public List<PlaneTile> contactPlaneTile = new List<PlaneTile>(); //물병이 접촉하고 있는 planetile 리스트
    public List<PlaneTile> rangePlaneTile = new List<PlaneTile>(); //물병이 접하고 있는 aboveRange의 planetile 리스트

    BottleSelectController bottleSelectController;
    private BottleGenerator bottleGenerator;
    private BottleController bottleController; //NEW: 오타 수정
    private BottleCollision thisBottleCollision; //현재 게임오브젝트의 bottleCollision 컴포넌트
    private UsefullOperation usefullOperation;
    private GameObject redAura;
    private GameObject freezeRange;
    private ScreenEffectController screenEffectController;
    private Psychokinesis psychokinesis; 



    void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        bottleGenerator = GameObject.Find("BottleManager").GetComponent<BottleGenerator>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        bottleController = GameObject.FindWithTag("isActBottle").GetComponent<BottleController>(); //NEW: 처음에 시작할 때 태그로 찾아줘야 함
        thisBottleCollision = gameObject.GetComponent<BottleCollision>();
        usefullOperation = GameObject.Find("GameResource").GetComponent<UsefullOperation>();
        redAura = transform.Find("RedAura").gameObject;
        freezeRange = transform.Find("FreezeRange").gameObject;
        screenEffectController = GameObject.Find("Main Camera").GetComponent<ScreenEffectController>();
        psychokinesis = GameObject.Find("Player").GetComponent<Psychokinesis>();
        bottleChain.Add(thisBottleCollision); //체인과 같이 연속적으로 이어진 물병들의 리스트에는 자신도 포함한다
    }

    //동전에 부딪혔을때. 동전은 isTrigger= True 상태여야함
    void OnTriggerEnter2D(Collider2D col)
    {
        // if 
        if (col.gameObject.tag == "coin")
        {
            Destroy(col.gameObject, 0f);
        }
    }

    //어딘가에 부딪혔을때
    void OnCollisionEnter2D(Collision2D col)
    {
        if(!(col.gameObject.CompareTag("Membrane"))) //반사막 생성 필살기로 생겨난 반사막을 제외하고
        {
            bottleController.isSuperPowerAvailabe = false; //더 이상 초능력을 적용할 수 없음
            if (gameObject.CompareTag("isActBottle"))
            {
                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                    Time.fixedDeltaTime = 0.02f * Time.timeScale;
                    screenEffectController.shadowEffect.enabled = false;
                    screenEffectController.screenEffectNum = 1;
                    screenEffectController.psychoTime = 0.4f;
                    usefullOperation.FadeOut(1, redAura.GetComponent<SpriteRenderer>());
                    psychokinesis.psychoAvailable = false;
                }

                gameObject.tag = "unActBottle";//태그 변경
                freezeRange.SetActive(false);

                bottleSelectController.bottleSelected = false;
                bottleGenerator.GenerateBottleWithDelay(1);//딜레이를 주고 물병 생성
                bottleSelectController.ReselectBottleWithDelay(1); //딜레이를 주고 물병 재선택
            }
        }
        
        if(col.gameObject.GetComponent<PlaneTile>() != null) //물병이 planeTile에 부딛힌 경우
		{
            PlaneTile planeTile = col.gameObject.GetComponent<PlaneTile>();
            contactPlaneTile.Add(planeTile);

            var list = new List<PlaneTile>();
            list.AddRange(rangePlaneTile);

            foreach(PlaneTile plane in list)
			{
                if(plane == planeTile)
				{
                    transform.SetParent(plane.transform);
                    break;
				}
			}
        }
        else if (col.gameObject.GetComponent<BottleCollision>() != null) //물병과 무딛혔을 경우
		{
            BottleCollision bottle = col.gameObject.GetComponent<BottleCollision>();

            var tempList = new List<BottleCollision>();
            tempList.AddRange(bottle.bottleChain);

            bool brk = false; //아래 foreach문 탈출값. 아래 체계는 가능하면 수정하도록 한다.

            if(rangePlaneTile != null) //물병이 aboveRange에 들어오지 않을 정도로 쌓이는 경우는 추후 필요한 경우가 생기면 작업한다.
			{
                foreach (BottleCollision bottleCollision in tempList)
                {
                    if (bottleCollision.contactPlaneTile != null)
                    {
                        foreach(PlaneTile plane in bottleCollision.contactPlaneTile)
						{
                            foreach(PlaneTile plane1 in rangePlaneTile)
							{
                                if(plane == plane1) //연쇄적으로 연결되어 있는 물병이 닿아 있는 planeTile과 본 물병이 range안에 들어가 있는 planeTile이 같은지의 여부를 확인
								{
                                    transform.SetParent(plane.transform);
                                    break; //물병을 추가 하는건 한 번만
								}
							}
                            if (brk) break;
						}
                    }
                    if (brk) break;
                }
            }
        }
        else if (col.gameObject.CompareTag("floor")) bottleController.onFloor = true; //물병이 바닥에 부딛힌 경우
    }

    void OnCollisionStay2D(Collision2D col)
	{
        BottleCollision bottle = col.gameObject.GetComponent<BottleCollision>();

        if (bottle == null) return; //부딛힌 물체가 물병이 아니면 리턴
		else
		{
            bottleChain.AddRange(bottle.bottleChain); //접한 물병의 리스트 추가
            bottleChain = bottleChain.Distinct().ToList(); //중복 요소 제거
		}
	}

    void OnCollisionExit2D(Collision2D col)
	{
        BottleCollision bottleCollision = col.gameObject.GetComponent<BottleCollision>();

        if (bottleCollision == null) return; //떨어진 물체가 물병이 아니면 리턴
        else
        {
            var tempList = new List<BottleCollision>();
            tempList.AddRange(bottleChain);

            foreach(BottleCollision bottle in tempList) //체인으로 연결되어있던 모든 물병들의 bottleChain 리스트 갱신
			{
                if (bottle == thisBottleCollision) //자기자신의 리스트에는 자기만 빼고 다 제거(어차피 OnCollisionStay2D에서 다시 갱신됨)
                {
                    bottleChain.Clear();
                    bottleChain.Add(thisBottleCollision);
				}
				else
				{
                    bottle.bottleChain.Remove(thisBottleCollision);   //다른 물병들의 bottleChain에서 자기를 제거

                    if (bottle.contactPlaneTile != null) //자기가 어떠한 PlaneTile과 연결되어 있는지의 여부
					{
                        foreach(PlaneTile plane in bottle.contactPlaneTile)
						{
                            plane.bottleAbove.Remove(thisBottleCollision); //연결되어 있는 모든 PlaneTile에서 물병 제거
						}
					}
				}
			}
        }
    }
}
