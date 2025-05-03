using System;
using System.Collections;

public class GSC_WaitAction : GSC_ScriptAction
{
    public float Duration;
    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new GSC_ContainerUnit("Wait");
        unit.Set("Duration", Duration);
        return unit;
    }
    public override bool Decompile(GSC_ContainerUnit unit)
    {
        if (Validate(unit))
        {
            Duration = unit.GetFloat("Duration");
            return true;
        }
        return false;
    }

    public override GSC_ContainerUnit TryDecodeScript(string[] line)
    {
        throw new NotImplementedException();
    }

    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit != null && unit.Calling == "Wait" && unit.HasFloat("Duration");
    }
    protected override IEnumerator InnerAction(Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        yield return GSC_Constants.WaitForSeconds(Duration, paused, ends);
        onEnd();
    }
}

