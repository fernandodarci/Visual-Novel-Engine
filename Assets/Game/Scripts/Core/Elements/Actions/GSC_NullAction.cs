using System;
using System.Collections;

public class GSC_NullAction : GSC_ScriptAction
{
    public override GSC_ContainerUnit Compile()
    {
        return new GSC_ContainerUnit("Null");
    }
    public override bool Decompile(GSC_ContainerUnit unit)
    {
        return true;
    }

    public override GSC_ContainerUnit TryDecodeScript(string[] line)
    {
        throw new NotImplementedException();
    }

    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit != null && unit.Calling == "Null";
    }
    protected override IEnumerator InnerAction(Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        yield break;
    }
}
