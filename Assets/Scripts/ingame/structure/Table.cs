using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Structure
{
    private new void Start()
    {
        base.Start();
        
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        delta = 0;
        collisionNum = 0;
    }

    void Update()
    {
        ThawDynamicStructure(isFreezed);
    }
}
