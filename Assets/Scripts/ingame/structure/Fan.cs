using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : Structure
{
    void Update()
    {
        ThawDynamicStructure(isFreezed);
    }
}
