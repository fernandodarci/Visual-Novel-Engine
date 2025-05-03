using System;
using System.Collections;

public class GSC_LoadScriptAction : GSC_ScriptAction
{
    public string ScriptName;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new GSC_ContainerUnit("LoadScript");
        unit.Set("ScriptName", ScriptName);
        return unit;
    }

    public override bool Decompile(GSC_ContainerUnit unit)
    {
        if(Validate(unit))
        {
            ScriptName = unit.GetString("ScriptName");
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
        return unit != null && unit.Calling == "LoadScript" && unit.HasString("ScriptName");
    }

    protected override IEnumerator InnerAction(Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        //To Do: Load the script and add it to the command queue
        //GSC_ScriptUnit script = GSC_ScriptManager.Instance.GetScript(ScriptName);
        yield return null;
        onEnd();
    }
}
