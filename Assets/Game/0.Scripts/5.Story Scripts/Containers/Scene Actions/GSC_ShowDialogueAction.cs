using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GSC_ShowDialogueAction : GSC_ScriptAction
{
    public string CharacterName;
    public string NickName;
    [TextArea] private string Dialogue;
    public bool Append;
    public float Duration;
    public float FadeTime;
    public bool WaitToComplete;
    public float WaitUntilComplete;
    public bool HideAfterFinished;

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
        return unit;
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
            return true;
        }
        return false;
    }

    public override bool Validate(GSC_ContainerUnit unit)
    {
        throw new NotImplementedException();
    }

    protected override IEnumerator InnerAction(Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        var dialogueController = GSC_DialogueManager.Instance.GetDialogueController();
        dialogueController.ClearText();
        string characterName = GSC_DataManager.Instance.ProcessCharacterName(CharacterName,NickName);
        dialogueController.ChangeCharacterName(characterName);
        yield return dialogueController.ShowDialoguePanel(FadeTime, paused, ends);
        string dialogue = GSC_DataManager.Instance.ProcessString(Dialogue);
        yield return dialogueController.ShowDialogue(dialogue, Duration, Append, paused, ends);
        if (WaitToComplete == true)
        {
            yield return dialogueController.ShowCompleted(Duration, paused, ends);
            yield return GSC_Constants.WaitForComplete(ends);
        }
        else if (WaitUntilComplete > 0)
        {
            yield return GSC_Constants.WaitForSeconds(WaitUntilComplete, paused, ends);
        }

        if (HideAfterFinished == true)
        {
            yield return dialogueController.FadeOut(Duration, paused, ends);
        }
        onEnd();
    }


}