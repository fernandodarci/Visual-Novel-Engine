using System;
using UnityEngine;



[Serializable]
public class GSC_ShowDialogueData : GSC_ScriptData
{
    public string CharacterName;
    [TextArea] public string Dialogue;
    public bool Append;
    public float Duration;
    public float FadeTime;
    public bool WaitToComplete;
    public float WaitUntilComplete;
    public bool HideAfter;
   
    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new("ShowDialogue");
        if (CharacterName.Contains(" as "))
        {
            int index = CharacterName.IndexOf(" as ");
            unit.Set("Character", CharacterName[..index].Trim());
            unit.Set("As", CharacterName[(index + 4)..].Trim());
        }
        else
        {
            unit.Set("Character", CharacterName.Trim());
        }
        unit.Set("Dialogue", Dialogue);
        unit.Set("Append", Append);
        unit.Set("Duration", Duration);
        unit.Set("Fade", FadeTime);
        unit.Set("WaitToComplete", WaitToComplete);
        unit.Set("WaitUntilComplete", WaitUntilComplete);
        unit.Set("HideAfter",HideAfter);
        return unit;
    }
    
    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit.Calling == "ShowDialogue" && unit.HasString("Character") && unit.HasString("Dialogue")
            && unit.HasBoolean("Append") && unit.HasFloat("Duration") && unit.HasFloat("Fade")
            && unit.HasBoolean("WaitToComplete") && unit.HasFloat("WaitUntilComplete") 
            && unit.HasBoolean("HideAfter");
    }

    public override bool Decompile(GSC_ContainerUnit result)
    {
        if (result != null && Validate(result))
        {
            if (result.HasString("As"))
            {
                string character = result.GetString("Character");
                string alias = result.GetString("As");
                CharacterName = $"{character} as {alias}".Trim();
            }
            else CharacterName = result.GetString("Character").Trim();

            Dialogue = result.GetString("Dialogue");
            Append = result.GetBoolean("Append");
            Duration = result.GetFloat("Duration");
            FadeTime = result.GetFloat("Fade");
            WaitToComplete = result.GetBoolean("WaitToComplete");
            WaitUntilComplete = result.GetFloat("WaitUntilComplete");
            HideAfter = result.GetBoolean("HideAfter");
            return true;
        }
        else return false;
    }

 
}
