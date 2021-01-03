using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//모든 평평한 타일, 즉 물병을 주로 세우게 될 타일을 관리하는 스크립트

public class PlaneTile : Structure
{
	//=================[private static field]=========================
	private static List<PlaneTile> planeTiles = new List<PlaneTile>();
	//=================[private static field]=========================
	
    public bool isSpinning; //타일이 회전하는지의 여부
    public float spinningSpeed;  //타일이 회전하는 속도
    public bool isMoving; //타일이 이동하는지의 여부
    public Vector3 direction; //이동방향
    public float movingSpeed; //타일이 이동하는 속도
    public float movingRange; //이동 범위
    public bool isNumLimitTile; //개수 제한 타일인지의 여부
    public int LimitNum; //개수 제한 타일에 올라갈수 있는 최대 물병의 개수
    public bool isFlagTile; //깃발 타일인지의 여부
    public int requiredNum; //깃발 타일에서 세워져야 하는 물병의 개수
    public bool numFullfilled; //깃발 타일에서 요구하는 개수만큼의 물병이 위에 세워져 있는지의 여부
    public List<BottleCollision> bottleAbove { get; set; } = new List<BottleCollision>(); //위에 올려져 있는 물병들의 리스트

    private Vector3 originPos; //처음 배치 위치
    private Vector3 spinAxis; //회전 축
    private PolygonCollider2D col;
    private int key; //이동 방향 전환 키
    private bool turnSucees; //성공적으로 방향을 전환했는지의 여부
    private bool isInvisible; //현재 타일이 투명한 지의 여부
    private float timeGap; //개수 제한 타일이 사라졌다가 나타나기까지의 시간 

    public new void Start()
    {
        base.Start();

        col = GetComponent<PolygonCollider2D>();
        key = 1;
        direction = Vector3.ClampMagnitude(direction, 1);
        originPos = transform.position;
        spinAxis = transform.GetChild(1).transform.position;
        turnSucees = false;
        isInvisible = false;
        numFullfilled = false;
        timeGap = 1;

		if (isMoving)
		{
			transform.GetChild(0).gameObject.SetActive(true);
		}

		planeTiles.Add(this);
	}


	void Update()
	{
		ThawDynamicStructure(isFreezed);

		if (!isFreezed)
			MoveDynamicStructure();

		if (isNumLimitTile) //제한 개수 타일인 경우
		{
			if (bottleAbove.Count > LimitNum) //타일 위에 올라간 물병의 개수가 제한 개수를 초과했을 때
			{
				usefullOperation.FadeOut(0, spriteRenderer); //상태는 변화 없이 투명도만 높임
				col.enabled = false; //콜라이더 없앰
				isInvisible = true;
				bottleAbove.Clear();
			}

			if (isInvisible)
			{
				timeGap -= Time.deltaTime;
				if (timeGap < 0)
				{
					usefullOperation.FadeIn(spriteRenderer);
					col.enabled = true;
					timeGap = 1;
					isInvisible = false;
				}
			}
		}

		if (isFlagTile) //깃발 타일인 경우
		{
			if (bottleAbove.Count >= requiredNum) //위에 올려져있는 물병의 개수가 요구 개수 이상일 때부터 검사 시작
			{
				int count = 0;

				var list = new List<BottleCollision>();
				list.AddRange(bottleAbove);

				foreach (BottleCollision bottle in list)
				{
					if (bottle.gameObject.GetComponent<BottleController>().isStanding) count++;
				}

				if (count >= requiredNum) numFullfilled = true;
				else numFullfilled = false;
			}
		}
	}

    public void MoveDynamicStructure()
    {
        if (isSpinning)
            transform.RotateAround(spinAxis, new Vector3(0,0,1) , -360 * spinningSpeed * Time.smoothDeltaTime);

		if (isMoving)
		{
			transform.Translate(key * direction * movingSpeed * Time.smoothDeltaTime);
		

			if ((originPos - transform.position).magnitude > movingRange)
			{
				if (!turnSucees)
				{
					key = -key;
					turnSucees = true;
				}
			}
			else if (turnSucees)
			{
				turnSucees = false;
			}
		}
	}

	public void OnCollisionStay2D(Collision2D col)
	{
		BottleCollision bottle = col.gameObject.GetComponent<BottleCollision>();

		if (bottle == null) return; //부딛힌 물체가 물병이 아니면 리턴
		else
		{
			bottleAbove.AddRange(bottle.bottleChain); //접한 물병의 리스트 추가
			bottleAbove = bottleAbove.Distinct().ToList(); //중복 요소 제거
		}
	}

	void OnCollisionExit2D(Collision2D col)
	{
		BottleCollision bottleCollision = col.gameObject.GetComponent<BottleCollision>();

		if (bottleCollision == null) return; //떨어진 물체가 물병이 아니면 리턴
		else
		{
			bottleAbove = bottleAbove.Except(bottleCollision.bottleChain).ToList(); //떨어져 나가는 물병의 bottleChain 리스트 요소들을 제거
			bottleCollision.contactPlaneTile.Remove(GetComponent<PlaneTile>());
		}
	}

	public static int CountStandingBottleOnPlaneTile()
	{
		int count = 0;

		foreach (var planeTile in planeTiles)
		{
			if (planeTile.numFullfilled == true)
			{
				count += 1;
			}
		}

		return count;
	}
}
