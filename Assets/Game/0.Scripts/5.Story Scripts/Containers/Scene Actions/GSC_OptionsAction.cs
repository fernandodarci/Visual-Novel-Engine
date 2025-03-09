using System;
using System.Collections.Generic;
using UnityEngine;


public class GSC_OptionsAction : GSC_Action
{
    [TextArea(2, 5)] public string Message;
    public bool System;
    public string TargetResult;
    public float Fade;
    public float Duration;
    public List<string> Options;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit<string[]> result = new GSC_ContainerUnit<string[]>("ShowOptions");
        result.Set("System", System);
        result.Set("Message", Message);
        result.Set("TargetResult", TargetResult);
        result.Set("Fade", Fade);
        result.Set("Duration", Duration);
        result.Set(Options.ToArray());
        return result;
    }
}

