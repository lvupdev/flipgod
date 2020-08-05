using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MembraneEffect : MonoBehaviour
{
    private float delta;


    // Start is called before the first frame update

    void Start()
    {
        delta = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        delta += Time.fixedDeltaTime;
        if (delta < 0.2f)
        {
            transform.localScale += new Vector3(4f * Time.fixedDeltaTime, 4f * Time.fixedDeltaTime, 4f * Time.fixedDeltaTime);
        }
        else if(delta < 0.5f)
        {
            transform.localScale -= new Vector3(3.6f * Time.fixedDeltaTime, 3.6f * Time.fixedDeltaTime, 3.6f * Time.fixedDeltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
