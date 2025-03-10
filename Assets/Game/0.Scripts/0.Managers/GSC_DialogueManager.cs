using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GSC_DialogueManager : GSC_Singleton<GSC_DialogueManager>
{
    [Header("Dialogue Controllers")]
    [SerializeField] private GSC_DialoguePanelController DialogueController;
    [SerializeField] private GSC_InputPanelController InputPanelController;
    [SerializeField] private GSC_OptionsPanelController ChoicePanelController;
    [SerializeField] private List<GSC_ScreenMessageController> Controllers;

    private GSC_ScreenMessageController CurrentController;

    public void GetScreenMessageController(string controllerName)
    {
        CurrentController = Controllers.Find(x => x.ID == controllerName);
    }

    public void HideAllMessages()
    {
        InputPanelController.Hide();
        DialogueController.Hide();
        ChoicePanelController.Hide();
        foreach (var controller in Controllers)
            controller.Hide();
    }

    private IEnumerator ShowMessage(GSC_ContainerUnit unit, Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        GSC_ScreenMessageData result = new GSC_ScreenMessageData();
        if (result.Decompile(unit))
        {
            GetScreenMessageController(unit.GetString("MessageType"));
            if (CurrentController != null)
            {
                CurrentController.ClearText();
                yield return CurrentController.ShowMessagePanel(result.FadeTime, paused, ends);
                yield return CurrentController.ShowMessage(
                    result.Message, result.Duration, result.Append, paused, ends);

                if (result.WaitToComplete == true)
                {
                    yield return GSC_Constants.WaitForComplete(ends);
                }
                else
                {
                    if (result.WaitUntilComplete > float.Epsilon)
                    {
                        yield return GSC_Constants.WaitForSeconds(result.WaitUntilComplete, paused, ends);
                    }
                }
            }
        }
        onEnd();
    }


    private IEnumerator ShowDialogue(GSC_ContainerUnit unit, Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        GSC_ShowDialogueData result = new GSC_ShowDialogueData();
        if (result.Decompile(unit.ToJson()))
        {
            DialogueController.ClearText();
            string characterName = GSC_DataManager.Instance.ProcessString($"@name({result.CharacterName})");
            DialogueController.ChangeCharacterName(characterName);
            yield return DialogueController.ShowDialoguePanel(unit.GetFloat("Fade"), paused, ends);
            string dialogue = GSC_DataManager.Instance.ProcessString(result.Dialogue).Trim();
            yield return DialogueController.ShowDialogue
                (dialogue, result.Duration, result.Append, paused, ends);
            Debug.Log("Dialogue Ends");
            if (result.WaitToComplete == true)
            {
                yield return DialogueController.ShowCompleted(result.Duration, paused, ends);
                yield return GSC_Constants.WaitForComplete(ends);
            }
            else
            {
                if (result.WaitUntilComplete > float.Epsilon)
                {
                    yield return GSC_Constants.WaitForSeconds(result.WaitUntilComplete, paused, ends);
                }
            }

            if (result.HideAfter == true)
            {
                yield return DialogueController.FadeOut(result.FadeTime, paused, ends);
                DialogueController.Hide();
            }
        }
        onEnd();
    }

    private IEnumerator HideDialogue(GSC_ContainerUnit unit, Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        if (DialogueController.IsVisible && unit.Calling == "HideDialogue" && unit.HasFloat("Fade"))
        {
            yield return DialogueController.FadeOut(unit.GetFloat("Fade"), paused, ends);
            DialogueController.Hide();
        }
        onEnd();
    }

    public bool TryGetAction(GSC_ContainerUnit unit, out GSC_CommandManager.CommandAction action)
    {
        action = null;
        switch (unit.Calling)
        {
            case "ShowDialogue":
                action = () => ShowDialogue;
                return true;
            case "HideDialogue":
                action = () => HideDialogue;
                return true;
            case "ShowMessage":
                action = () => ShowMessage;
                return true;
            case "ShowOptions":
                action = () => ShowOptions;
                return true;
            default:
                return false;
        }
    }

    private IEnumerator ShowOptions(GSC_ContainerUnit unit, Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        if (unit is GSC_ContainerUnit<string[]> @choiceUnit)
        {
            ChoicePanelController.ClearMessage();
            yield return ChoicePanelController.FadeIn(choiceUnit.GetFloat("Fade"), paused, ends);
            ChoicePanelController.Enable();
            string message = GSC_DataManager.Instance.ProcessString(unit.GetString("Message"));
            yield return ChoicePanelController
                .ShowMessage(choiceUnit.GetString("Message"),
                choiceUnit.GetFloat("Duration"), paused, ends);
            ChoicePanelController.SetupPanel(choiceUnit.Get().ToArray());
            yield return ChoicePanelController.WaitForChoice();
            bool system = choiceUnit.GetBoolean("System");
            string target = choiceUnit.GetString("TargetResult");
            string result = ChoicePanelController.GetOption().Trim();
            Debug.Log($"{target} => {result}");
            GSC_DataManager.Instance.AddOrChangeValue(target, result, system);

            yield return ChoicePanelController.FadeOut(choiceUnit.GetFloat("Fade"), paused, ends);
            ChoicePanelController.Hide();
        }
        onEnd();
    }



    //private static IEnumerator ShowInput(GSC_ContainerUnit unit, Func<bool> paused, Func<bool> ends)
    //{

    //    if (unit is GSC_ContainerUnit<GSC_Parameter> @param)
    //    {
    //        GSC_Parameter parameter = param.Get();
    //        if (parameter == null) Debug.Log("Error getting parameter");
    //        bool systemParam = param.GetBoolean("System");
    //        var inputController = Dialogues.GetInput();
    //        if (inputController != null)
    //        {
    //            inputController.InitializeInput(parameter);
    //            yield return inputController.FadeIn(unit.GetFloat("Fade"), paused, ends);
    //            inputController.Enable();
    //            string message = Data.ProcessString(unit.GetString("Message"));
    //            yield return inputController
    //                .ShowMessage(message, unit.GetFloat("Duration"), paused, ends);
    //            yield return inputController.WaitForInput();
    //            yield return inputController.FadeOut(unit.GetFloat("Fade"), paused, ends);
    //            inputController.Hide();
    //        }
    //        Command.Ends();
    //    }

    //}


}
