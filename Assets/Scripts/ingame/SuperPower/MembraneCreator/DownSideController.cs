using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 탄성막의 아랫면 콜라이더 충돌 관련 스크립트입니다.
 */
public class DownSideController : MonoBehaviour
{
    private MembraneUsingSkillEffect membrane;
    private GameObject bottles;

    private void Start()
    {
        membrane = gameObject.transform.parent.gameObject.GetComponent<MembraneUsingSkillEffect>();
        bottles = GameObject.Find("Bottles");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform.parent.transform == bottles.transform)
            membrane.Avtivate(-1, collision.gameObject.GetComponent<BottleController>()); //접촉한 물볃의 bottleController 컴포넌트 전달
    }
}
