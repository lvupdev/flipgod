using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    private GameObject bottle;

    //포물선
    public GameObject trajectoryDotPrefab;
    private GameObject[] trajectoryDots;
    private GameObject[] directionDots;
    private int trajectoryNumber = 13; //포물선 점 개수
    private int directionNumber = 5;
    private float normalStrength = 10.0f; //NEW: 포물선에 적용되는 기본 힘

    public void Start()
    {


        bottle = GameObject.FindWithTag("isActive");

        trajectoryDots = new GameObject[trajectoryNumber];
        for (int i = 0; i < trajectoryNumber; i++)
        {
            trajectoryDots[i] = Instantiate(trajectoryDotPrefab, bottle.transform);
        }

        directionDots = new GameObject[directionNumber];
        for (int i = 0; i < directionNumber; i++)
        {
            directionDots[i] = Instantiate(trajectoryDotPrefab, bottle.transform);
        }
    }


    public void draw(bool padStrengthTouched, Vector2 direction, float strengthFactor)
    {
        // 포물선 그리기
        if (!padStrengthTouched) //NEW: 방향 포물선 그리기
        {
            strengthFactor = normalStrength;
            for (int i = 0; i < directionNumber; i++)
            {
                directionDots[i].transform.position = CalculatePosition(i * 0.1f, direction, strengthFactor);
            }
        }
        else 
        {
            for (int i = 0; i < trajectoryNumber; i++)
            {
                trajectoryDots[i].transform.position = CalculatePosition(i * 0.1f, direction, strengthFactor);
            }
        }
    }

    public void delete()
    {
        for (int i = 0; i < trajectoryNumber; i++)
        {
            Destroy(trajectoryDots[i]);
        }
        for (int i = 0; i < directionNumber; i++)
        {
            Destroy(directionDots[i]);
        }
    }

    private Vector2 CalculatePosition(float elapsedTime, Vector2 direction, float strengthFactor)
    {
        return Physics2D.gravity * elapsedTime * elapsedTime * 0.5f +
                   direction * strengthFactor * elapsedTime + new Vector2(bottle.transform.position.x, bottle.transform.position.y);
    }

}
