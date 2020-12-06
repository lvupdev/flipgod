using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 자주 사용되는 메서드들을 담은 스크립트
 */

public class UsefullOperation : MonoBehaviour
{

	private static List<ObjectInformation> targetObject = new List<ObjectInformation>(); //메서드를 적용할 오브젝트들을 담는 리스트

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (targetObject.Count > 0)
		{
			for (int i = targetObject.Count - 1; i >= 0; i--) //리스트의 마지막 요소부터 검사
			{
				if (targetObject[i].fadeOut)
				{
					targetObject[i].color.a -= 4 * Time.deltaTime; //약 0.25초동안 페이드 아웃 효과
					targetObject[i].spriteRenderer.color = targetObject[i].color;
					if (targetObject[i].color.a < 0)
					{
						switch (targetObject[i].statusCase)
						{
							case 0: //오브젝트 상태 유지
								break;
							case 1:
								targetObject[i].spriteRenderer.gameObject.SetActive(false);
								break;
							case 2:
								BottleController bottleController = targetObject[i].spriteRenderer.gameObject.GetComponent<BottleController>();
								if (bottleController != null) bottleController.DestroyBottle();
								else Destroy(targetObject[i].spriteRenderer.gameObject);
								break;
						}
						targetObject.RemoveAt(i);
					}
				}
				else if (targetObject[i].fadeIn)
				{
					targetObject[i].color.a += 4 * Time.deltaTime; //약 0.25초동안 페이드 인 효과
					targetObject[i].spriteRenderer.color = targetObject[i].color;
					if (targetObject[i].color.a > 1)
					{
						targetObject.RemoveAt(i);
					}
				}
				else if (targetObject[i].shakeTime > 0)
				{
					targetObject[i].transform.position = Random.insideUnitSphere * targetObject[i].shakeAmount + targetObject[i].initialPosition;
					targetObject[i].shakeTime -= Time.deltaTime;
					if (targetObject[i].shakeTime < 0)
					{
						targetObject[i].transform.position = targetObject[i].initialPosition;
						targetObject.RemoveAt(i);
					}
				}
			}
		}
	}

	public void FadeOut(int statusCase, SpriteRenderer spriteRenderer) //매개변수 스프라이트에 페이드 아웃 효과를 준다. statusCase에 따라 오브젝트의 상태를 변경한다.
	{
		ObjectInformation objectInformation = new ObjectInformation(spriteRenderer, false);
		objectInformation.statusCase = statusCase;
		targetObject.Add(objectInformation);
	}

	public void FadeIn(SpriteRenderer spriteRenderer) //매개변수 스프라이트에 페이드인 효과를 준다.
	{
		ObjectInformation objectInformation = new ObjectInformation(spriteRenderer, true);
		objectInformation.color.a = 0;
		spriteRenderer.color = objectInformation.color;
		spriteRenderer.gameObject.SetActive(true);
		targetObject.Add(objectInformation);
	}

	public void ShakeObject(Transform transform, float shakeTime, float shakeAmount) //매개변수로 전달된 트랜스폼의 게임오브젝트를 진동시킨다.
	{
		ObjectInformation objectInformation = new ObjectInformation(transform, shakeTime, shakeAmount);
		targetObject.Add(objectInformation);
	}
}

class ObjectInformation
{

	public int statusCase; //오브젝트를 어떠한 상태로 변화시킬지의 여부 0: 변화 없음 1: setActive(false) 2: 오브젝트 파괴
	public bool fadeIn; //페이드인 효과를 적용할 지의 여부
	public bool fadeOut; //페이드 아웃 효과를 적용할지의 여부
	public float shakeAmount; // 진동 세기
	public float shakeTime; // 진동 시간

	public Vector3 initialPosition; //처음 위치
	public Transform transform; //오브젝트 트랜스폼
	public SpriteRenderer spriteRenderer;
	public Color color;

	public ObjectInformation(SpriteRenderer spriteRenderer, bool fadeIn) //페이드 효과 전용 생성자
	{
		this.spriteRenderer = spriteRenderer;
		color = spriteRenderer.color;
		statusCase = 0;
		shakeTime = 0;
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

	public ObjectInformation(Transform transform, float shakeTime, float shakeAmount) //진동 효과 전용 생성자
	{
		this.transform = transform;
		this.shakeTime = shakeTime;
		this.shakeAmount = shakeAmount;
		initialPosition = transform.position;

		fadeIn = false;
		fadeOut = false;
	}
}