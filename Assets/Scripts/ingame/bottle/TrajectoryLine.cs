using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is used for drawing trajectory line.
 */

public class TrajectoryLine : MonoBehaviour
{
    private const int trajectoryNumber = 15; //포물선 점 개수
    private const float fadingNum = 5.0f; //흐려지는 점의 개수

    private GameObject bottle;
    
    //포물선
    public GameObject trajectoryDotPrefab;
    private GameObject[] trajectoryDots;


    public void Start()
    {
        bottle = BottleController.ControllingBottle.gameObject;
        trajectoryDots = new GameObject[trajectoryNumber];
        for (int i = 0; i < trajectoryNumber; i++)
        {
            trajectoryDots[i] = Instantiate(trajectoryDotPrefab, bottle.transform);
            if (i > trajectoryNumber - 4) //끝쪽 점선은 갈수록 흐려짐
                trajectoryDots[i].gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, -(1 / fadingNum) * i + (trajectoryNumber / fadingNum));
        }
    }

    public void Draw(bool padStrengthTouched, Vector2 direction, float strengthFactor)
    {
        if (padStrengthTouched)
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
        return Physics2D.gravity * 2 * elapsedTime * elapsedTime * 0.5f +
                   direction * strengthFactor * elapsedTime + new Vector2(bottle.transform.position.x, bottle.transform.position.y);
    }

}
