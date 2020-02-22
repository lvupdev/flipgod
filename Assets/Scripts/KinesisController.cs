using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinesisController : MonoBehaviour
{
    public int superPowerLV; //초능력 강화 레벨
    public int skillLV; //필살기 강화 레벨
    GameObject bottlePrefab;

    void Start()
    {
        bottlePrefab = GameObject.Find("BottlePrefab");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetMouseButton(0) && bottlePrefab.GetComponent<BottleController>().isSuperPowerAvailabe)
        {
            Psychokinesis();
        }
    }

    public void Psychokinesis()
    {
        if (Input.mousePosition.x > Screen.width / 2.0f)
        {
            bottlePrefab.GetComponent<BottleController>().rb.AddTorque(-superPowerLV/10.0f, ForceMode2D.Impulse);
        }
        if(Input.mousePosition.x <= Screen.width / 2.0f)
        {
            bottlePrefab.GetComponent<BottleController>().rb.AddTorque(superPowerLV/10.0f, ForceMode2D.Impulse);
        }
    }

    public void ReselectBottle()
    {
        bottlePrefab = GameObject.FindWithTag("isActive");//힘을 적용할 물병을 태그에 따라 재설정
    }
}
