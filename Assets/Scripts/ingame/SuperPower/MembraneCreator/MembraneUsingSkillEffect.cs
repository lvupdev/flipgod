using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 탄성막 생성 필살기를 사용할 때 만들어지는 탄성막을 관리하는 스크립트입니다.
 */
public class MembraneUsingSkillEffect : MonoBehaviour
{
    private Vector3 init; //처음 터치했을 때 좌표
    private Vector3 curr; //현재 터치하고 있는 좌표 
    private Vector3 recentPos; // 오브젝트의 가장 최근 위치
    private Vector3 membraneDirection; // 탄성막이 향하는 방향
    private GameObject glowingEffect;
    private PadDirection padDirection;
    private UsefullOperation usefullOperation;
    private bool startDelta; //시간초를 세어야하는지의 여부
    private float destroyDelta; //탄성막이 파괴되기까지 걸리는 시간

    public static GameObject selectedMembrane; //현재 어떤 탄성막을 선택중인지 가르키는 변수
    public static float presentStrength = 0; //현재 물병을 던진만큼의 힘

    public void setStartDelta(bool value) { startDelta = value; } 

    private void Start()
    {
        recentPos = transform.position;
        glowingEffect = gameObject.transform.Find("GlowingEffect").gameObject;
        padDirection = GameObject.Find("Joystick").GetComponent<PadDirection>();
        usefullOperation = GameObject.Find("GameResource").GetComponent<UsefullOperation>();
        membraneDirection = new Vector3(0, 1, 0);
        startDelta = false;
        destroyDelta = 20;
    }

    private void Update()
    {
        if(this.gameObject == selectedMembrane)
        {
            glowingEffect.SetActive(true); //터치하면 선택 이미지를 활성화한다.
            if (padDirection.getIsTouch())
            {
                double angle = Mathf.Atan2(padDirection.getDirection().x, padDirection.getDirection().y) * (180.0 / Mathf.PI);
                this.transform.rotation = Quaternion.Euler(0, 0, -(float)angle); //조이스틱의 방향에 따라 방향을 변경
                membraneDirection = padDirection.getDirection(); //탄성막이 향하는 방향을 저장해 둔다.
            }
        }
        else
        {
            glowingEffect.SetActive(false);
        }

        if (startDelta)//선택이 끝나면 파괴되기까지의 카운트 다운 시작
        {
            destroyDelta -= Time.deltaTime;
            if (destroyDelta < 0) usefullOperation.FadeOut(true, GetComponent<SpriteRenderer>()); //설치하고 15초가 지나면 파괴
        }
    }

    public void Avtivate(int key, BottleController bottleController) //upside에 맞았으면 1이, downside에 맞았으면 -1이 전달된다.
    {
        bottleController.rb.velocity = key * membraneDirection * presentStrength; //물병 튕겨냄
    }

    private void OnMouseDown() // 탄성막을 터치했을 때
    {
        if (!startDelta)
        {
            init = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)); //마우스 좌표를 월드 좌표로 변환
            selectedMembrane = this.gameObject;
        }
    }

    private void OnMouseDrag()
    {
        if (!startDelta)
        {
            curr = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)); //현재 터치 중인 마우스 좌표를 월드 좌표로 변횐
            Vector3 distance = curr - init;
            transform.position = recentPos + distance; //드래그 거리만큼 오브젝트를 최근위치에서 이동
        }
    }

    private void OnMouseUp()
    {
        if (!startDelta)
        {
            recentPos = transform.position; //최근 위치 갱신
            padDirection.setDirection(Vector2.zero); // 방향 초기화
        }
    }

    public void Activate()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
