﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffectParticle : MonoBehaviour
{
    private float delta; //파괴되기까지의 카운트 다운 변수
    
    // Start is called before the first frame update
    void Start()
    {
        delta = 0;
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;

        if (delta > 6) Destroy(gameObject);
    }
}
