using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//트리거 관리 스크립트
public class TriggerFunction : MonoBehaviour
{
    public BottleSelectController bottleSelectController;
    public List <GameObject> TargetObject = new List<GameObject>(); //트리거와 상호작용 중인 오브젝트 배열

    public bool conditionFullfilled; //트리거 발동 조건이 충족되었는지의 여부 / 트리거 작동 권한
    public bool isActTrigger; //트리거가 활성화 상태인지의 여부
    public bool isFreezed; //빙결 상태인지의 여부
    public int collisionNum; //충돌 횟수
    public float intervalTime; //트리거 발동 시간 간격
    public float operatingTime; //트리거가 발동을 지속한 시간;
    public float freezedTime; //얼어있던 시간


    private void Start()
    {
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();

        conditionFullfilled = false;
        isFreezed = false;
        collisionNum = 0;
        intervalTime = 0;
        operatingTime = 0;
        freezedTime = 0;
    }





    // 트리거 발동 조건 함수


    /*
     * 상시 발동
     */
    public bool Always()
    {
        return true;
    }

    /*
     * n초 주기 발동
     * 게임 시작 후 time(초)을 주기로 트리거에 한 번 작동 권한을 부여한다.
     */
    public bool TimeInterval(float time)
    {
        intervalTime += Time.deltaTime;
        if (intervalTime > time)
        {
            conditionFullfilled = true;
            intervalTime = 0;
            return true;
        }
        else
            return false;
    }

    /*
     * n번 충돌 발동
     * num 번만큼 충돌하면 트리거에 작동 권한을 한 번 부여한다.
     */
    public bool EnoughCrash(int num)
    {
        if (collisionNum == num)
        {
            collisionNum = 0;
            return true;
        }
        else
            return false;
    }


    /*
     * n개 누적 발동
     * num개 이상의 물병이 해당 구조물 위에 쌓이는 순간부터 계속해서 트리거 작동 권한 부여
     */
    public bool EnoughStack(int num)
    {
        if (TargetObject.Count >= num) return true;
        else return false;
    }



    //트리거 효과 함수


    /*
     * 물병 자석
     * key값이 1이면 척력, -1이면 인력 작용
     * strength는 자석의 힘을 나타내는 값
     */
    public void BottleMagnet(int key, float strength)
    {
        for (int i = 0; i < TargetObject.Count; i++)
        {
            Vector2 directionToMagnet = (TargetObject[i].transform.position - gameObject.transform.position).normalized; // 자석으로 향하는 벡터설정 
            float distance = Vector2.Distance(gameObject.transform.position, TargetObject[i].transform.position);// Distance 로 거리를 a,b사이의 거리를 구함
            TargetObject[i].GetComponent<Rigidbody2D>().AddForce((key * strength * directionToMagnet/distance), ForceMode2D.Force);// 힘의 크기와 방향이 있으니까 물리적 힘 구현 rigidbody가 있어야 가능
        }
    }



    /*
     * 트리거 활성화
     * name에 활성화 할 트리거의 이름을 입력한다.
     */
    public void Activate(string name)
    {
        GameObject.Find(name).GetComponent<TriggerFunction>().isActTrigger = true;
    }


    /*
     * 트리거 비활성화
     * name에 비활성화 할 트리거의 이름을 입력한다.
     */
    public void UnActivate(string name)
    {
        GameObject.Find(name).GetComponent<TriggerFunction>().isActTrigger = false;
    }






    //트리거 발동 중단 함수

    /*
     * n초 경과
     * time(초)만큼의 시간이 지나면 트리거의 발동을 중단시킨다.
     */
    public bool TimeOut(float time)
    {
        if (conditionFullfilled) operatingTime += Time.deltaTime;

        if (time < operatingTime)
        {
            operatingTime = 0;
            return true;
        }
        else
            return false;
    }

    /*
     * n번 충돌 중단
     * num 횟수 만큼 충돌하면 트리거의 발동을 중단시킨다.
     */
    public bool ToMuchCrash(int num)
    {
        if (conditionFullfilled && (collisionNum == num))
        {
            collisionNum = 0;
            return true;
        }
        else
            return false;
    }


    /*
     * n개 누적 미달
     * num개 미만의 오브젝트가 콜라이더 안에 있으면 트리거의 발동이 중단된다.
     */
    public bool LowStack(int num)
    {
        if (TargetObject.Count < num) return true;
        else return false;
    }








    //충돌 판정 함수

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("isActBottle")) TargetObject.Add(collision.gameObject);
        collisionNum++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("isActBottle")) TargetObject.Remove(collision.gameObject);
    }
}
