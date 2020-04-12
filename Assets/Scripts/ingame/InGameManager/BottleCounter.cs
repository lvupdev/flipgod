using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Bottle의 개수를 세는 스크립트
 * BottleController 안에 해당 변수들을 static으로 선언해도 됨.
 */

public class BottleCounter : MonoBehaviour
{
    /*
     * 여태 던진 보틀의 개수 / (미션 성공을 위한) 보틀 제한 개수
     * (1) 세워져 있는 보틀 개수 -> isStanding == true
     * (2) 씬 안에 존재하는 보틀 개수 // 사용하는 경우 아직 없음
     * (3) 던진 보틀의 개수 -> 보틀의 동작이 끝나면 count
     */

    public static int usedBottleCount;
    public static int standingBottleCount;

    private static GameObject[] bottles;

    private void Awake()
    {
        usedBottleCount = 0;
        standingBottleCount = 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        standingBottleCount = CountStandingBottle();
    }

    // 세워져있는 Bottle의 개수를 세는 메서드
    public int CountStandingBottle()        
    {
        // Find..of... 메서드는 최적화 필요
        // 해결 1: Bottle들의 상위 객체(Bottles, parentBottle)를 생성해서 Start에서 그것만 받아오는 것
        //                         이후 Update에서 Bottles의 하위 객체(childBottle)들을 배열에 담도록.
        bottles = GameObject.FindGameObjectsWithTag("unActBottle");

        int numStandingBottle = 0;              // 세워져있는 Bsottle의 개수
        BottleController bottleController;      // 각 Bottle의 BottleController 스크립트 저장

        if (bottles != null)                    // 세워져있는 Bottle이 있다면
        {
            for (int i = 0; i < bottles.Length; i++)
            {
                bottleController = bottles[i].GetComponent<BottleController>();

                if (bottleController.isStanding == true)
                {
                    numStandingBottle++;
                }
            }
        }

        return numStandingBottle;
    }

}
