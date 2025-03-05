using System;
using UnityEngine;


public class GSC_SceneAction : MonoBehaviour
{
    public const string ID = "ID00";

    public string Name { get; set; }

    public virtual string GetID() => ID;

    public static GSC_SceneAction SetAction(string action)
    {
        return null;
    }

    public virtual GSC_ContainerUnit Compile() { return new GSC_ContainerUnit(""); }

    public virtual void Decompile(string json) { }

    protected GSC_ContainerUnit GetContainer(string json)
    {
        return GSC_ContainerUnit.FromJson(json);
    }

    public virtual bool Validate() {  return false; }
}
