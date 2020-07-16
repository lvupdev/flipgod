using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 총 코인 개수, 캐릭터별 업그레이드 정도 등을 저장하는 스크립트입니다.
public class ResourceManager : MonoBehaviour
{
    private int[] superPowerLV = { 1, 1, 1 }; //초능력 레벨 1로 모두 초기화. 인덱스 0,1,2 순대로 각각 염동력자, 탄성막생성자, 빙결자
    private int[] skillLV = { 1, 1, 1 }; //필살기 레벨 1로 모두 초기화. 인덱스 0,1,2 순대로 각각 염동력자, 탄성막생성자, 빙결자

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public int GetSuperPowerLV(int index) { return superPowerLV[index]; }
    public int GetSkillLV(int index) { return skillLV[index]; }

    // Update is called once per frame
    void Update()
    {
        
    }
}