using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleStanding : MonoBehaviour
{
    public GameObject[] bottle;
    public int bottlenumber;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    

    public int countBottle()
    {
        bottle = GameObject.FindGameObjectsWithTag("unActBottle");
        bottlenumber = 0;
        for (int i = 0; i < bottle.Length; i++)
        {
            if (bottle[i].GetComponent<BottleController>().isStanding)
            {
                bottlenumber++;
            }
        }
        return bottlenumber;
    }
}
