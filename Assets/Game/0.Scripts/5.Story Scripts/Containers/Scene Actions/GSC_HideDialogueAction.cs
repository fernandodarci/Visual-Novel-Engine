using System;
using System.Collections;

[Serializable]
public class GSC_HideDialogueAction : GSC_ScriptAction
{
    public float FadeTime;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new GSC_ContainerUnit("HideDialogue");
        unit.Set("Fade",FadeTime);
        return unit;
    }

    public override bool Decompile(GSC_ContainerUnit unit)
    {
        if(unit != null && Validate(unit))
        {
            FadeTime = unit.GetFloat("Fade");
            return true;
        }
        return false;
    }

    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit.Calling == "HideDialogue" && unit.HasFloat("Fade");
    }

    protected override IEnumerator InnerAction(Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        GSC_DialoguePanelController dialogue = GSC_DialogueManager.Instance.GetDialogueController();
        if(dialogue != null && dialogue.IsVisible && Validate(Compile()))
        {
            yield return dialogue.FadeOut(FadeTime, paused, ends);
        }
        onEnd();
    }
}