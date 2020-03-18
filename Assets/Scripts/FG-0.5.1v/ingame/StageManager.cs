using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 스테이지 레벨 매니저
 */

public class StageManager : MonoBehaviour
{
    // Dynamic Structure
    DSGenerator dSGenerator;

    // Start is called before the first frame update
    void Start()
    {
        dSGenerator = GameObject.Find("Dynamic Structure").GetComponent<DSGenerator>();
        dSGenerator.GenerateDynamicStructure("Car");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
