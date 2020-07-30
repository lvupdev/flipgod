using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ParticleEffect : MonoBehaviour
{
    private Freezer freezer;// 다른 스크립트에서 가져옴
    private GameObject bottle;
    private BottleSelectController bottleSelectController;// 다른 스크립트에서 가져옴

     
   
    void Start()
    {
        freezer = GameObject.Find("Player").GetComponent<Freezer>(); //Freezer 스크립트를 참조하는 객체 생성
                                                                     //        bottle = GameObject.Find("BottleManager").GetComponent<BottleSelectController>(); //BottleSelectConroller를 참조하는 객체 생성
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>(); //BottleSelectConroller를 참조하는 객체 생성


    }

    void Update()
    {
     
        bottle = bottleSelectController.bottle;
        if (gameObject == bottle)
        {
            Explosion();
        }
    }

    
    private void Explosion() // 빙결 효과 메서드, 다른 것도 넣어도 될 듯
    {
        if(freezer.freezeAvailable == false){ // 빙결효과가 발동된다면
            Debug.Log("빙결");
            ParticleSystem particle = gameObject.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
            Debug.Log("빙결2");
            particle.Play();//시작
            Debug.Log("빙결3");
         }
    }


}
