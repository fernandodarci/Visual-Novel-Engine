using System;
using System.Collections;
using UnityEngine;

public class GSC_DialogueRegister : GSC_CommandRegister
{
    private static GSC_DialogueManager Dialogues => GSC_DialogueManager.Instance;

    public new static void AddCommands(GSC_CommandDatabase database)
    {
        database.AddCommand("ShowDialogue",
            new Func<GSC_ContainerUnit, Func<bool>, Func<bool>, IEnumerator>(ShowDialogue), null)
            .With(new GSC_StringParameter("Character"))
            .With(new GSC_StringParameter("Dialogue"))
            .With(new GSC_BooleanParameter("Append"))
            .With(GSC_FloatParameter.WithRangeMin("Duration", 1f))
            .With(GSC_FloatParameter.WithRangeMin("Fade", 0f))
            .With(new GSC_BooleanParameter("WaitToComplete"));

        database.AddCommand("HideDialogue",
            new Func<GSC_ContainerUnit, Func<bool>, Func<bool>, IEnumerator>(HideDialogue), null)
            .With(GSC_FloatParameter.WithRangeMin("Fade", 0f));

        database.AddCommand("ShowMessage",
            new Func<GSC_ContainerUnit, Func<bool>, Func<bool>, IEnumerator>(ShowMessage), null)
           .With(new GSC_StringParameter("MessageType"))
           .With(new GSC_StringParameter("Message"))
           .With(GSC_FloatParameter.WithRangeMin("Duration", 1f))
           .With(GSC_FloatParameter.WithRangeMin("Fade", 0f))
           .With(new GSC_BooleanParameter("WaitToComplete"));

        database.AddCommand("ShowInput",
            new Func<GSC_ContainerUnit, Func<bool>, Func<bool>, IEnumerator>(ShowInput), null)
            .With(new GSC_StringParameter("Description"))
            .With(GSC_FloatParameter.WithRangeMin("Duration", 1f))
            .With(GSC_FloatParameter.WithRangeMin("Fade", 0f))
            .With(new GSC_BooleanParameter("System"));


        database.AddCommand("ShowChoice",
            new Func<GSC_ContainerUnit, Func<bool>, Func<bool>, IEnumerator>(ShowChoices), null)
           .With(new GSC_StringParameter("Description"))
           .With(GSC_FloatParameter.WithRangeMin("Duration", 1f))
           .With(GSC_FloatParameter.WithRangeMin("Fade", 0f))
           .With(new GSC_StringParameter("TargetResult"))
           .With(new GSC_BooleanParameter("System"));
    }

    private static IEnumerator ShowMessage(GSC_ContainerUnit unit, Func<bool> paused, Func<bool> ends)
    {
        GSC_ScreenMessageController message = Dialogues.GetMessageController(unit.GetString("MessageType"));
        if (message != null)
        {
            message.ClearText();
            string dialogue = unit.GetString("Message");
            float textTime = unit.GetFloat("Duration");
            bool append = unit.GetBoolean("Append");
            yield return message.ShowMessagePanel(unit.GetFloat("Fade"),paused,ends);
            yield return message.ShowMessage(dialogue, textTime, append, paused, ends);
            if (unit.GetBoolean("WaitToComplete") == true)
            {
                //yield return WaitForComplete();
            }
            yield return message.HideMessage(unit.GetFloat("Fade"),paused,ends);
        }
        Command.Ends();
    }


    private static IEnumerator ShowDialogue(GSC_ContainerUnit unit, Func<bool> paused, Func<bool> ends)
    {
        var dialogueController = Dialogues.GetDialogue();
        dialogueController.ClearText();
        if (unit is GSC_ContainerUnit<Color> colorUnit)
            dialogueController.ChangeCharacterNameColor(colorUnit.Get());
        dialogueController.ChangeCharacterName(unit.GetString("Character"));
        yield return dialogueController.ShowDialoguePanel(unit.GetFloat("Fade"),paused,ends);
        string dialogue = unit.GetString("Dialogue");
        float textTime = unit.GetFloat("Duration");
        bool append = unit.GetBoolean("Append");
        yield return dialogueController.ShowDialogue(dialogue, textTime, append,paused,ends);
        if (unit.GetBoolean("WaitToComplete") == true)
        {
            yield return dialogueController.ShowCompleted(textTime,paused,ends);
            //yield return Game.WaitForComplete();
            dialogueController.ClearText();
            yield return dialogueController.FadeOut(unit.GetFloat("Fade"), paused, ends);
        }

        Command.Ends();
    }

    private static IEnumerator HideDialogue(GSC_ContainerUnit unit, Func<bool> paused, Func<bool> ends)
    {
        var controller = Dialogues.GetDialogue();
        if (controller != null && controller.IsVisible)
        {
            yield return controller.FadeOut(unit.GetFloat("Fade"), paused, ends);
        }
        Command.Ends();
    }

    private static IEnumerator ShowInput(GSC_ContainerUnit unit, Func<bool> paused, Func<bool> ends)
    {
        if (unit is GSC_ContainerUnit<GSC_Parameter> @param)
        {
            GSC_Parameter parameter = param.Get();
            if (parameter == null) Debug.Log("Error getting parameter");
            bool systemParam = param.GetBoolean("System");
            var inputController = Dialogues.GetInput();
            if (inputController != null)
            {
                inputController.InitializeInput(parameter);
                yield return inputController.FadeIn(unit.GetFloat("Fade"), paused, ends);
                inputController.Enable();
                yield return inputController
                    .ShowMessage(unit.GetString("Description"), unit.GetFloat("Duration"),paused,ends);
                yield return inputController.WaitForInput();
                yield return inputController.FadeOut(unit.GetFloat("Fade"), paused, ends);
                inputController.Hide();
            }
            Command.Ends();
        }

    }

    private static IEnumerator ShowChoices(GSC_ContainerUnit unit, Func<bool> paused, Func<bool> ends)
    {
        if (unit is GSC_ContainerUnit<string[]> @choiceUnit)
        {
            var choiceController = Dialogues.GetChoice();
            if (choiceController != null)
            {
                choiceController.Initialize(choiceUnit.GetString("TargetResult"));
                yield return choiceController.FadeIn(choiceUnit.GetFloat("Fade"), paused, ends);
                choiceController.Enable();
                yield return choiceController
                    .ShowMessage(choiceUnit.GetString("Description"), choiceUnit.GetFloat("Duration"),paused,ends);
                choiceController.SetupPanel(choiceUnit.Get());
                yield return choiceController.WaitForChoice();
                yield return choiceController.FadeOut(choiceUnit.GetFloat("Fade"), paused, ends);
                choiceController.Hide();
            }
            Command.Ends();
        }
    }
}