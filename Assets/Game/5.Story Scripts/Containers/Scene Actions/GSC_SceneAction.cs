using System;
using UnityEngine;


public class GSC_SceneAction : MonoBehaviour
{
    public virtual string ID => "None";

    public string Name { get; set; }

    public virtual GSC_ContainerUnit Compile() { return new GSC_ContainerUnit(""); }
}
