using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Structure
{
    void Update()
    {
        ThawDynamicStructure(isFreezed);
    }
}
