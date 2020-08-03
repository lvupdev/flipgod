using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    private GameObject bottle;

    //포물선
    public GameObject[] trajectoryDotPrefab;
    private GameObject[] trajectoryDots;
    private int trajectoryNumber = 13; //포물선 점 개수
    private float normalStrength = 10.0f; //포물선에 적용되는 기본 힘
    private PadDirection rotate; // rotate값 가져오기 

    public void Start()
    {


        bottle = GameObject.FindWithTag("isActBottle");

        trajectoryDots = new GameObject[trajectoryNumber];
        for (int i = 0; i < trajectoryNumber; i++)
        {
            trajectoryDots[i] = Instantiate(trajectoryDotPrefab[0], bottle.transform);
        }

        rotate = GameObject.Find("Joystick").GetComponent<PadDirection>(); // Pad direction의 rotate값 가져오기(rotate를 위함)
        
        
       
    }
    public void Update()
    {
        if()
        {

        }
    }


    public void Draw(bool padStrengthTouched, Vector2 direction, float strengthFactor)
    {
        // 포물선 그리기
        if (!padStrengthTouched) //방향 포물선 그리기
        {
            strengthFactor = normalStrength;
            
        }
        else
        {
        

            for (int i = 0; i < trajectoryNumber; i++)
            {
                trajectoryDots[i].transform.position = CalculatePosition(i * 0.1f, direction, strengthFactor);
            }
        }
    }

    public void Delete()
    {
        for (int i = 0; i < trajectoryNumber; i++)
        {
            Destroy(trajectoryDots[i]);
        }
       
    }

    private Vector2 CalculatePosition(float elapsedTime, Vector2 direction, float strengthFactor)
    {
        return Physics2D.gravity * elapsedTime * elapsedTime * 0.5f +
                   direction * strengthFactor * elapsedTime + new Vector2(bottle.transform.position.x, bottle.transform.position.y);
    }

}
