using System;
using System.Collections;
using static GSC_CommandManager;

public abstract class GSC_ScriptAction
{
    public abstract GSC_ContainerUnit Compile();

    public bool Decompile(string json)
    {
        GSC_ContainerUnit result = GSC_ContainerUnit.FromJson(json);
        return Decompile(result);
    }

    public abstract bool Decompile(GSC_ContainerUnit unit);
    public abstract bool Validate(GSC_ContainerUnit unit);

    public GSC_CommandAction GetAction()
    {
        return () => InnerAction;
    }

    protected abstract IEnumerator InnerAction(Func<bool> paused, Func<bool> ends, Action onEnd);
    
}
