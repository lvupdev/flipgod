using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    // 자석구현을 위한 스크립트

    //public Variables
    public Vector2 newPosition; // 

    //private Variables
    private Transform trans; //  

    void Awake() //를 쓰는 이유 스크립트에서 한번만 호출, 스크립트 비활성화라도 호출
    {
        trans = transform;
    }
    /*
    void Update()
    {
        trans.position = Vector2.Lerp(trans.position, newPosition, Time.deltaTime * 1.5f); // 움직임을 t(시간)에 대한 근사 사실 필요없음

        if (Mathf.Abs(newPosition.x - trans.position.x) < 0.05) // mathf 를 호출해서 연산 중 하나인 abs 호출>> 절댓값을 반환하는 함수임
            trans.position = newPosition;
    }

   /* void OnTriggerEnter2D(Collider2D other)  // 충돌이 일어났을 때
    {
        if (other.tag == "Coin")
            Destroy(other.gameObject); // 오브젝트 없애버림
    }
    */
}