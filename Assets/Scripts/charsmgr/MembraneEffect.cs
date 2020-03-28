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
        SPPController = GameObject.Find("SuperPowerPanel").GetComponent<SuperPowerPanelController>();
        delta = 0;
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        if (delta < 0.1f)
        {
            transform.localScale += new Vector3(0.8f * Time.fixedDeltaTime, 0.8f * Time.fixedDeltaTime, 0.8f * Time.fixedDeltaTime);
            transform.Translate(SPPController.getDragDirection() * Time.deltaTime);
        }
        else if(delta < 0.4f)
        {
            transform.localScale -= new Vector3(0.3f * Time.fixedDeltaTime, 0.3f * Time.fixedDeltaTime, 0.3f * Time.fixedDeltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
