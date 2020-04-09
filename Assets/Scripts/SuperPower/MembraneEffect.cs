using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MembraneEffect : MonoBehaviour
{
    private SuperPowerPanelController SPPController;
    private float delta;


    // Start is called before the first frame update

    void Start()
    {
        delta = 0;
        SPPController = GameObject.Find("SuperPowerPanel").GetComponent<SuperPowerPanelController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        delta += Time.fixedDeltaTime;
        if (delta < 0.2f)
        {
            transform.localScale += new Vector3(0.9f * Time.fixedDeltaTime, 0.9f * Time.fixedDeltaTime, 0.9f * Time.fixedDeltaTime);
        }
        else if(delta < 0.5f)
        {
            transform.localScale -= new Vector3(0.8f * Time.fixedDeltaTime, 0.8f * Time.fixedDeltaTime, 0.8f * Time.fixedDeltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
