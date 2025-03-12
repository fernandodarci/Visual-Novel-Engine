using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class GSC_ShowMessageAction : GSC_ScriptAction
{
    [TextArea(2, 10)] public string Message;
    public string MessageType;
    public Vector2 Position;
    public Vector2 Size;
    public bool Append;
    public float Duration;
    public float FadeTime;
    public bool WaitToComplete;
    public float WaitUntilComplete;
    public bool HideAfterComplete;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new("ShowMessage");
        unit.Set("MessageType", MessageType);
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
        if (result != null && Validate(result))
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

    protected override IEnumerator InnerAction(Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        GSC_DialogueManager.Instance
            .GetScreenMessageController(MessageType,new GSC_PositionSize(Position,Size));
        var currentController = GSC_DialogueManager.Instance.CurrentController;

        if (currentController != null && Validate(Compile()))
        {
            currentController.ClearText();
            yield return currentController.ShowMessagePanel(FadeTime, paused, ends);
            yield return currentController.ShowMessage(Message, Duration, Append, paused, ends);

            if (WaitToComplete == true)
            {
                yield return GSC_Constants.WaitForComplete(ends);
            }
            else
            {
                if (WaitUntilComplete > float.Epsilon)
                {
                    yield return GSC_Constants.WaitForSeconds(WaitUntilComplete, paused, ends);
                }
            }
        }

        onEnd();
    }
}