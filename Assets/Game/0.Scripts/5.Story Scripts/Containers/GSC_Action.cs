using System;
using UnityEngine;


public class GSC_Action : MonoBehaviour
{
    public const string ID = "ID00";

    public string Name { get; set; }

    public virtual string GetID() => ID;

    public static GSC_Action SetAction(string action)
    {
        return null;
    }

    public virtual GSC_ContainerUnit Compile() { return new GSC_ContainerUnit(""); }

    public virtual bool Decompile(string json) { return false; }

    protected GSC_ContainerUnit GetContainer(string json)
    {
        return GSC_ContainerUnit.FromJson(json);
    }

    public virtual bool Validate(GSC_ContainerUnit unit) {  return false; }
}
public abstract class GSC_ScriptData
{
    public abstract GSC_ContainerUnit Compile();

    public bool Decompile(string json)
    {
        GSC_ContainerUnit result = GSC_ContainerUnit.FromJson(json);
        return Decompile(result);
    }

    public abstract bool Decompile(GSC_ContainerUnit unit);
    public abstract bool Validate(GSC_ContainerUnit unit);

}
