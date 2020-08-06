using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalPlane2 : Structure
{
    // Start is called before the first frame update
    void Update()
    {
        ThawDynamicStructure(isFreezed);
    }
}
