using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MembraneManager : MonoBehaviour
{
    public GameObject membranePrefab;
    
    private GameObject membranes;
    private BottleSelectController bottleSelectController;
    private ResourceManager resourceManager;
    private int  skillLV; //필살기 레벨

    private void Start()
    {
        membranes = GameObject.Find("Membranes");
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
        resourceManager = GameObject.Find("GameResourceValue").GetComponent<ResourceManager>();
        skillLV = resourceManager.GetSkillLV(1);
    }

    public void AddMembrane()
    {
        int usingSkillNum = bottleSelectController.bottleSkillOperation.getUsingSkillNum();
        if ((usingSkillNum < skillLV) && (membranes.transform.childCount < skillLV)) //스테이지에 생성할 수 있는 탄성막의 총 개수는 skillLV 개이다.
        {
            bottleSelectController.bottleSkillOperation.setUsingSkillNum(++usingSkillNum); //usingSkillNum을 1 증가시킨다
            GameObject membrane = Instantiate(membranePrefab) as GameObject;
            membrane.transform.position = new Vector3(0, usingSkillNum - 2, 0); // 생성하는 순서에 따라 생성되는 위치가 달라진다
            membrane.transform.SetParent(membranes.transform);
        }
    }

    public void SubtractMembrane()
    {
        int usingSkillNum = bottleSelectController.bottleSkillOperation.getUsingSkillNum();
        if (usingSkillNum > 0)
        {
            Destroy(membranes.transform.GetChild(--usingSkillNum).gameObject); //usingSkillNum을 1 감소시킨다
            bottleSelectController.bottleSkillOperation.setUsingSkillNum(usingSkillNum); //가장 최근에 추가된 membrane 파괴

        }
    }
}
