using System;
using System.Collections.Generic;

[Serializable]
public class GSC_SceneUnit
{
    public int Index;
    public List<GSC_ContainerUnit> Containers;

    public GSC_SceneUnit(int index)
    {
        Index = index;
        Containers = new List<GSC_ContainerUnit>();
    }
}
