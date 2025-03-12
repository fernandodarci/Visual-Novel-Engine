using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GSC_OptionActionAction : GSC_ScriptAction
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

    public override bool Decompile(GSC_ContainerUnit unit)
    {
        throw new NotImplementedException();
    }

    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit.Calling == "ShowOptions" && unit is GSC_ContainerUnit<string[]> @opt
            && unit.HasBoolean("System") && unit.HasString("Message") && unit.HasString("TargetResult")
            && unit.HasFloat("Fade") && unit.HasFloat("Duration") && !opt.Get().IsNullOrEmpty();
    }

    protected override IEnumerator InnerAction(Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        var OptionController = GSC_DialogueManager.Instance.GetOptionController();
        if (OptionController != null && Validate(Compile()))
        {
            OptionController.ClearMessage();
            yield return OptionController.FadeIn(Fade, paused, ends);
            OptionController.Enable();
            string message = GSC_DataManager.Instance.ProcessString(Message);
            yield return OptionController.ShowMessage(message, Duration, paused, ends);
                
            OptionController.SetupPanel(Options.ToArray());
            yield return OptionController.WaitForChoice();
            string result = OptionController.GetOption();
            GSC_DataManager.Instance.AddOrChangeValue(TargetResult, result, System);

            yield return OptionController.FadeOut(Fade, paused, ends);
            OptionController.Hide();
        }
        onEnd();
    }

}
