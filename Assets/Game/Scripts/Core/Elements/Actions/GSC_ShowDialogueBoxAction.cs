using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class GSC_ShowDialogueBoxAction : GSC_ScriptAction
{
    public string Character;
    public string NameToShow;
    [TextArea(2,5)] public string Dialogue;
    public float FadeTime;
    public bool Append;
    public float Duration;
    public float WaitTime;
    public bool HideAfterEnd;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new GSC_ContainerUnit("ShowDialogue");
        unit.Set("Character", Character);
        unit.Set("NameToShow", NameToShow);
        unit.Set("Dialogue", Dialogue);
        unit.Set("FadeTime", FadeTime);
        unit.Set("Duration", Duration);
        unit.Set("Append", Append);
        unit.Set("WaitTime", WaitTime);
        unit.Set("HideAfterEnd", HideAfterEnd);
        return unit;
    }

    public override bool Decompile(GSC_ContainerUnit unit)
    {
        if (Validate(unit))
        {
            Character = unit.GetString("Character");
            NameToShow = unit.GetString("NameToShow");
            Dialogue = unit.GetString("Dialogue");
            Duration = unit.GetFloat("Duration");
            Append = unit.GetBoolean("Append");
            FadeTime = unit.GetFloat("FadeTime");
            WaitTime = unit.GetFloat("WaitTime");
            HideAfterEnd = unit.GetBoolean("HideAfterEnd");
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
        return unit != null && unit.Calling == "ShowDialogue" && unit.HasString("Character")
            && unit.HasString("NameToShow") && unit.HasString("Dialogue") && unit.HasFloat("Duration")
            && unit.HasBoolean("Append") && unit.HasFloat("FadeTime") && unit.HasFloat("WaitTime") 
            && unit.HasBoolean("HideAfterEnd");
    }

    protected override IEnumerator InnerAction(Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        GSC_DialogueBox box = GSC_ElementViewManager.Instance.GetDialogueBox();
        if (box != null)
        {
            box.ClearMessages();
            box.SetCharacterName(Character, NameToShow);
            if (!box.IsVisible)
                yield return box.FadeIn(FadeTime, false, paused, ends);
            
            yield return box.SetDialogue(Dialogue,Duration,Append,paused,ends);

            if (WaitTime < 0)
            {
                yield return box.ShowMessageToComplete(paused,ends);
                yield return GSC_Constants.WaitForComplete(ends);
            }

            else
                yield return GSC_Constants.WaitForSeconds(WaitTime, paused, ends);

            if (HideAfterEnd == true)
            {
                Debug.Log("Hiding Dialogue Box");
                yield return box.FadeOut(FadeTime, paused, ends);
            }
                
        }
        onEnd();
    }
}

