using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Structure
{
    void Update()
    {
        ThawDynamicStructure(isFreezed);
    }
}
