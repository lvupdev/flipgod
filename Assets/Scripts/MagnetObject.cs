using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetObject : MonoBehaviour
{

    public float magnetStrength = 5f;
    public float distanceStretch = 10f; // Strength, based on the distance
    public int magnetDirection = -1;  // attact = 1  -1 = repel
    public bool looseMagnet = true;

    private Transform trans; // 자력을 받는 물체의 transform
    private Rigidbody2D thisRb;// 자력을 받는 물체의 rb
    private Transform magnetTrans;// 자석의 위치
    private bool magnetInZone; //자력 범위 구분
    private GameObject bottle;

    void Awake()
    {
        this.bottle = GameObject.Find("BottlePrefab");
        trans = this.bottle.transform;
        thisRb = bottle.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() // 규칙적인 시간간격으로 호출됨, 호출 사이의 시간이 같다.
    {
        if (magnetInZone) // 자력 범위 내에 있으면
        {

            Vector2 directionToMagnet = magnetTrans.position - trans.position; // 자석으로 향하는 벡터설정 
            float distance = Vector2.Distance(magnetTrans.position, trans.position);// Distance 로 거리를 a,b사이의 거리를 구함
            float magnetDistanceStr = (distanceStretch / distance) * magnetStrength;// 거리에따른 힘이 달라져야하니까 거리로 나눔
            thisRb.AddForce(magnetDistanceStr * (directionToMagnet * magnetDirection), ForceMode2D.Force);// 힘의 크기와 방향이 있으니까 물리적 힘 구현 rigidbody가 있어야 가능

        }
    }

    void OnTriggerEnter2D(Collider2D other) //collider에 충돌체가 빠져 들어가는 순간 호출되는 함수이다.
    {
        if (other.tag == "Fan")
        {
            magnetTrans = other.transform;
            magnetInZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) //OnTriggerExit는 collider에 충돌체가 빠져 나오는 순간 호출되는 함수이다.
    {
        if (other.tag == "Fan" && looseMagnet)
        {
            magnetInZone = false;
        }
    }
}
