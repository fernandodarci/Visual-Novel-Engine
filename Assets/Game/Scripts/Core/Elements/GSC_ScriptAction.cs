using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GSC_CommandManager;

[Serializable]
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
    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit != null && unit.Calling == "Null";
    }
    protected override IEnumerator InnerAction(Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        yield break;
    }
}


public static class GSC_ScriptActionExtensions
{
    public static void GetScriptAction(this GSC_ScriptUnit unit, GSC_ContainerUnit data)
    {
        switch(data.Calling)
        {
            case "ShowDialogue":
                {
                    GSC_ShowDialogueBoxAction action = new GSC_ShowDialogueBoxAction();
                    if(action.Validate(data))
                    {
                        action.Decompile(data);
                        unit.Actions.Add(action);
                        return;
                    }
                }
                break;
        }    
    }
  
    
   
}
