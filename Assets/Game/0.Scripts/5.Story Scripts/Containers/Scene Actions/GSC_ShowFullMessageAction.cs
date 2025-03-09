using System;
using UnityEngine;

public class GSC_ShowFullMessageAction : GSC_Action
{
    public new const string ID = "ID04";

    public override string GetID() => ID;
    public GSC_ScreenMessageData data;
    
}

[Serializable]
public class GSC_ScreenMessageData : GSC_ScriptData
{
    [TextArea(2, 10)] public string Message;
    public bool Append;
    public float Duration;
    public float FadeTime;
    public bool WaitToComplete;
    public float WaitUntilComplete;
    public bool HideAfterComplete;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new("ShowMessage");
        unit.Set("MessageType", "FullMessage");
        unit.Set("Message", Message);
        unit.Set("Append", Append);
        unit.Set("Duration", Duration);
        unit.Set("Fade", FadeTime);
        unit.Set("WaitToComplete", WaitToComplete);
        unit.Set("WaitUntilComplete", WaitUntilComplete);
        unit.Set("HideAfterComplete", HideAfterComplete);
        return unit;
    }

    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit.Calling == "ShowMessage" && unit.HasString("MessageType") && unit.HasString("Message")
            && unit.HasBoolean("Append") && unit.HasFloat("Duration") && unit.HasFloat("Fade")
            && unit.HasBoolean("WaitToComplete") && unit.HasFloat("WaitUntilComplete")
            && unit.HasBoolean("HideAfterComplete");
    }

    public override bool Decompile(GSC_ContainerUnit result)
    {
        if (result != null)
        {
            Message = result.GetString("Message");
            Append = result.GetBoolean("Append");
            Duration = result.GetFloat("Duration");
            FadeTime = result.GetFloat("Fade");
            WaitToComplete = result.GetBoolean("WaitToComplete");
            WaitUntilComplete = result.GetFloat("WaitUntilComplete");
            HideAfterComplete = result.GetBoolean("HideAfterComplete");
            return true;
        }
        return false;
    }


}